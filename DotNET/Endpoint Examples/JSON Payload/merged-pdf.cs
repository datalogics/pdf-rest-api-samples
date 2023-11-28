
using Newtonsoft.Json.Linq;
using System.Text;

using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com") })
{
    using (var firstUploadRequest = new HttpRequestMessage(HttpMethod.Post, "upload"))
    {
        firstUploadRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
        firstUploadRequest.Headers.Accept.Add(new("application/json"));

        var firstUploadByteArray = File.ReadAllBytes("/path/to/first_file");
        var firstUploadByteAryContent = new ByteArrayContent(firstUploadByteArray);
        firstUploadByteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
        firstUploadByteAryContent.Headers.TryAddWithoutValidation("Content-Filename", "first_filename.pdf");


        firstUploadRequest.Content = firstUploadByteAryContent;
        var firstUploadResponse = await httpClient.SendAsync(firstUploadRequest);

        var firstUploadResult = await firstUploadResponse.Content.ReadAsStringAsync();

        Console.WriteLine("First upload response received.");
        Console.WriteLine(firstUploadResult);

        JObject firstUploadResultJson = JObject.Parse(firstUploadResult);
        var firstUploadedID = firstUploadResultJson["files"][0]["id"];

        using (var secondUploadRequest = new HttpRequestMessage(HttpMethod.Post, "upload"))
        {
            secondUploadRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
            secondUploadRequest.Headers.Accept.Add(new("application/json"));

            var secondUploadByteArray = File.ReadAllBytes("/path/to/second_file");
            var secondUploadByteAryContent = new ByteArrayContent(secondUploadByteArray);
            secondUploadByteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
            secondUploadByteAryContent.Headers.TryAddWithoutValidation("Content-Filename", "second_filename.pdf");


            secondUploadRequest.Content = secondUploadByteAryContent;
            var secondUploadResponse = await httpClient.SendAsync(secondUploadRequest);

            var secondUploadResult = await secondUploadResponse.Content.ReadAsStringAsync();

            Console.WriteLine("Second upload response received.");
            Console.WriteLine(secondUploadResult);

            JObject secondUploadResultJson = JObject.Parse(secondUploadResult);
            var secondUploadedID = secondUploadResultJson["files"][0]["id"];
            using (var mergeRequest = new HttpRequestMessage(HttpMethod.Post, "merged-pdf"))
            {
                mergeRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
                mergeRequest.Headers.Accept.Add(new("application/json"));

                mergeRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");


                JObject parameterJson = new JObject
                {
                    ["id"] = new JArray(firstUploadedID, secondUploadedID),
                    ["pages"] = new JArray(1, 1),
                    ["type"] = new JArray("id", "id"),

                };

                mergeRequest.Content = new StringContent(parameterJson.ToString(), Encoding.UTF8, "application/json"); ;
                var mergeResponse = await httpClient.SendAsync(mergeRequest);

                var mergeResult = await mergeResponse.Content.ReadAsStringAsync();

                Console.WriteLine("Processing response received.");
                Console.WriteLine(mergeResult);
            }
        }
    }
}

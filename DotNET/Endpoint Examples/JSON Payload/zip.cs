
using Newtonsoft.Json.Linq;
using System.Text;

using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com") })
{
    using (var firstUploadRequest = new HttpRequestMessage(HttpMethod.Post, "upload"))
    {
        firstUploadRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
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
            secondUploadRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
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
            using (var zipRequest = new HttpRequestMessage(HttpMethod.Post, "zip"))
            {
                zipRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
                zipRequest.Headers.Accept.Add(new("application/json"));

                zipRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");


                JObject parameterJson = new JObject
                {
                    ["id"] = new JArray(firstUploadedID, secondUploadedID),


                };

                zipRequest.Content = new StringContent(parameterJson.ToString(), Encoding.UTF8, "application/json"); ;
                var zipResponse = await httpClient.SendAsync(zipRequest);

                var zipResult = await zipResponse.Content.ReadAsStringAsync();

                Console.WriteLine("Processing response received.");
                Console.WriteLine(zipResult);
            }
        }
    }
}

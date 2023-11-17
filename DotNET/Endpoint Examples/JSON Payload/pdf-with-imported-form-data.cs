
using Newtonsoft.Json.Linq;
using System.Text;

using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com") })
{
    using (var pdfUploadRequest = new HttpRequestMessage(HttpMethod.Post, "upload"))
    {
        pdfUploadRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
        pdfUploadRequest.Headers.Accept.Add(new("application/json"));

        var pdfUploadByteArray = File.ReadAllBytes("/path/to/pdf_file");
        var pdfUploadByteAryContent = new ByteArrayContent(pdfUploadByteArray);
        pdfUploadByteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
        pdfUploadByteAryContent.Headers.TryAddWithoutValidation("Content-Filename", "filename.pdf");


        pdfUploadRequest.Content = pdfUploadByteAryContent;
        var pdfUploadResponse = await httpClient.SendAsync(pdfUploadRequest);

        var pdfUploadResult = await pdfUploadResponse.Content.ReadAsStringAsync();

        Console.WriteLine("PDF upload response received.");
        Console.WriteLine(pdfUploadResult);

        JObject pdfUploadResultJson = JObject.Parse(pdfUploadResult);
        var pdfUploadedID = pdfUploadResultJson["files"][0]["id"];

        using (var dataUploadRequest = new HttpRequestMessage(HttpMethod.Post, "upload"))
        {
            dataUploadRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
            dataUploadRequest.Headers.Accept.Add(new("application/json"));

            var dataUploadByteArray = File.ReadAllBytes("/path/to/data_file");
            var dataUploadByteAryContent = new ByteArrayContent(dataUploadByteArray);
            dataUploadByteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
            dataUploadByteAryContent.Headers.TryAddWithoutValidation("Content-Filename", "data_filename.xml");


            dataUploadRequest.Content = dataUploadByteAryContent;
            var dataUploadResponse = await httpClient.SendAsync(dataUploadRequest);

            var dataUploadResult = await dataUploadResponse.Content.ReadAsStringAsync();

            Console.WriteLine("Data upload response received.");
            Console.WriteLine(dataUploadResult);

            JObject dataUploadResultJson = JObject.Parse(dataUploadResult);
            var dataUploadedID = dataUploadResultJson["files"][0]["id"];
            using (var attachRequest = new HttpRequestMessage(HttpMethod.Post, "pdf-with-imported-form-data"))
            {
                attachRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
                attachRequest.Headers.Accept.Add(new("application/json"));

                attachRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");


                JObject parameterJson = new JObject
                {
                    ["id"] = pdfUploadedID,
                    ["data_file_id"] = dataUploadedID,

                };

                attachRequest.Content = new StringContent(parameterJson.ToString(), Encoding.UTF8, "application/json"); ;
                var attachResponse = await httpClient.SendAsync(attachRequest);

                var attachResult = await attachResponse.Content.ReadAsStringAsync();

                Console.WriteLine("Processing response received.");
                Console.WriteLine(attachResult);
            }
        }
    }
}

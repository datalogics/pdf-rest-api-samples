
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
        pdfUploadByteAryContent.Headers.TryAddWithoutValidation("Content-Filename", "pdf_filename.pdf");


        pdfUploadRequest.Content = pdfUploadByteAryContent;
        var pdfUploadResponse = await httpClient.SendAsync(pdfUploadRequest);

        var pdfUploadResult = await pdfUploadResponse.Content.ReadAsStringAsync();

        Console.WriteLine("PDF upload response received.");
        Console.WriteLine(pdfUploadResult);

        JObject pdfUploadResultJson = JObject.Parse(pdfUploadResult);
        var pdfUploadedID = pdfUploadResultJson["files"][0]["id"];

        using (var imageUploadRequest = new HttpRequestMessage(HttpMethod.Post, "upload"))
        {
            imageUploadRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
            imageUploadRequest.Headers.Accept.Add(new("application/json"));

            var imageUploadByteArray = File.ReadAllBytes("/path/to/image_file");
            var imageUploadByteAryContent = new ByteArrayContent(imageUploadByteArray);
            imageUploadByteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
            imageUploadByteAryContent.Headers.TryAddWithoutValidation("Content-Filename", "image_filename.png");


            imageUploadRequest.Content = imageUploadByteAryContent;
            var imageUploadResponse = await httpClient.SendAsync(imageUploadRequest);

            var imageUploadResult = await imageUploadResponse.Content.ReadAsStringAsync();

            Console.WriteLine("Image upload response received.");
            Console.WriteLine(imageUploadResult);

            JObject imageUploadResultJson = JObject.Parse(imageUploadResult);
            var imageUploadedID = imageUploadResultJson["files"][0]["id"];
            using (var attachRequest = new HttpRequestMessage(HttpMethod.Post, "pdf-with-added-image"))
            {
                attachRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
                attachRequest.Headers.Accept.Add(new("application/json"));

                attachRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");


                JObject parameterJson = new JObject
                {
                    ["id"] = pdfUploadedID,
                    ["image_id"] = imageUploadedID,
                    ["page"] = 1,
                    ["x"] = 0,
                    ["y"] = 0

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


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

        using (var attachmentUploadRequest = new HttpRequestMessage(HttpMethod.Post, "upload"))
        {
            attachmentUploadRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
            attachmentUploadRequest.Headers.Accept.Add(new("application/json"));

            var attachmentUploadByteArray = File.ReadAllBytes("/path/to/attachment_file");
            var attachmentUploadByteAryContent = new ByteArrayContent(attachmentUploadByteArray);
            attachmentUploadByteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
            attachmentUploadByteAryContent.Headers.TryAddWithoutValidation("Content-Filename", "attachment_filename.pdf");


            attachmentUploadRequest.Content = attachmentUploadByteAryContent;
            var attachmentUploadResponse = await httpClient.SendAsync(attachmentUploadRequest);

            var attachmentUploadResult = await attachmentUploadResponse.Content.ReadAsStringAsync();

            Console.WriteLine("Attachment upload response received.");
            Console.WriteLine(attachmentUploadResult);

            JObject attachmentUploadResultJson = JObject.Parse(attachmentUploadResult);
            var attachmentUploadedID = attachmentUploadResultJson["files"][0]["id"];
            using (var attachRequest = new HttpRequestMessage(HttpMethod.Post, "pdf-with-added-attachment"))
            {
                attachRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
                attachRequest.Headers.Accept.Add(new("application/json"));

                attachRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");


                JObject parameterJson = new JObject
                {
                    ["id"] = pdfUploadedID,
                    ["id_to_attach"] = attachmentUploadedID,

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

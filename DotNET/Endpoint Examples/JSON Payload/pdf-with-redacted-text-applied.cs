
using Newtonsoft.Json.Linq;
using System.Text;

// Toggle deletion of sensitive files (default: false)
var deleteSensitiveFiles = false;

using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com") })
{
    using (var uploadRequest = new HttpRequestMessage(HttpMethod.Post, "upload"))
    {
        uploadRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
        uploadRequest.Headers.Accept.Add(new("application/json"));

        var uploadByteArray = File.ReadAllBytes("/path/to/file");
        var uploadByteAryContent = new ByteArrayContent(uploadByteArray);
        uploadByteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
        uploadByteAryContent.Headers.TryAddWithoutValidation("Content-Filename", "filename.pdf");


        uploadRequest.Content = uploadByteAryContent;
        var uploadResponse = await httpClient.SendAsync(uploadRequest);

        var uploadResult = await uploadResponse.Content.ReadAsStringAsync();

        Console.WriteLine("Upload response received.");
        Console.WriteLine(uploadResult);

        JObject uploadResultJson = JObject.Parse(uploadResult);
        var uploadedID = uploadResultJson["files"][0]["id"];
        using (var redactedTextRequest = new HttpRequestMessage(HttpMethod.Post, "pdf-with-redacted-text-applied"))
        {
            redactedTextRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
            redactedTextRequest.Headers.Accept.Add(new("application/json"));

            redactedTextRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");


            JObject parameterJson = new JObject
            {
                ["id"] = uploadedID,
            };

            redactedTextRequest.Content = new StringContent(parameterJson.ToString(), Encoding.UTF8, "application/json"); ;
            var redactedTextResponse = await httpClient.SendAsync(redactedTextRequest);

            var redactedTextResult = await redactedTextResponse.Content.ReadAsStringAsync();

            Console.WriteLine("Processing response received.");
            Console.WriteLine(redactedTextResult);

            // All files uploaded or generated are automatically deleted based on the 
            // File Retention Period as shown on https://pdfrest.com/pricing. 
            // For immediate deletion of files, particularly when sensitive data 
            // is involved, an explicit delete call can be made to the API.
            //
            // The following code is an optional step to delete sensitive files 
            // (unredacted, unencrypted, unrestricted, or unwatermarked) from pdfRest servers.

            if (deleteSensitiveFiles)
            {
                using (var deleteRequest = new HttpRequestMessage(HttpMethod.Post, "delete"))
                {
                    deleteRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
                    deleteRequest.Headers.Accept.Add(new("application/json"));
                    deleteRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");

                    JObject deleteJson = new JObject
                    {
                        ["ids"] = uploadedID
                    };
                    deleteRequest.Content = new StringContent(deleteJson.ToString(), Encoding.UTF8, "application/json");
                    var deleteResponse = await httpClient.SendAsync(deleteRequest);
                    var deleteResult = await deleteResponse.Content.ReadAsStringAsync();
                    Console.WriteLine(deleteResult);
                }
            }
        }
    }
}

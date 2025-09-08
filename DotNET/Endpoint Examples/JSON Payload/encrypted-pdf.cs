
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
        using (var encryptRequest = new HttpRequestMessage(HttpMethod.Post, "encrypted-pdf"))
        {
            encryptRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
            encryptRequest.Headers.Accept.Add(new("application/json"));

            encryptRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");


            JObject parameterJson = new JObject
            {
                ["id"] = uploadedID,
                ["new_open_password"] = "password"
            };

            encryptRequest.Content = new StringContent(parameterJson.ToString(), Encoding.UTF8, "application/json"); ;
            var encryptResponse = await httpClient.SendAsync(encryptRequest);

            var encryptResult = await encryptResponse.Content.ReadAsStringAsync();

            Console.WriteLine("Processing response received.");
            Console.WriteLine(encryptResult);
            JObject encryptResultJson = JObject.Parse(encryptResult);
            var outputID = encryptResultJson["outputId"];

            // All files uploaded or generated are automatically deleted based on the 
            // File Retention Period as shown on https://pdfrest.com/pricing. 
            // For immediate deletion of files, particularly when sensitive data 
            // is involved, an explicit delete call can be made to the API.
            //
            // Deletes all files in the workflow, including outputs. Save all desired files before enabling this step.

            if (deleteSensitiveFiles)
            {
                using (var deleteRequest = new HttpRequestMessage(HttpMethod.Post, "delete"))
                {
                    deleteRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
                    deleteRequest.Headers.Accept.Add(new("application/json"));
                    deleteRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");

                    JObject deleteJson = new JObject
                    {
                        ["ids"] = $"{uploadedID}, {outputID}"
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

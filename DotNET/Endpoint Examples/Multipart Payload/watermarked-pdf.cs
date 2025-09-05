using System.Text;

// Toggle deletion of sensitive files (default: false)
var deleteSensitiveFiles = false;

using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com") })
{
    using (var request = new HttpRequestMessage(HttpMethod.Post, "watermarked-pdf"))
    {
        request.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
        request.Headers.Accept.Add(new("application/json"));
        var multipartContent = new MultipartFormDataContent();

        var byteArray = File.ReadAllBytes("/path/to/file");
        var byteAryContent = new ByteArrayContent(byteArray);
        multipartContent.Add(byteAryContent, "file", "file_name");
        byteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/pdf");

        var byteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes("Watermarked"));
        multipartContent.Add(byteArrayOption, "watermark_text");


        request.Content = multipartContent;
        var response = await httpClient.SendAsync(request);

        var apiResult = await response.Content.ReadAsStringAsync();

        Console.WriteLine("API response received.");
        Console.WriteLine(apiResult);

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
            deleteRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
            deleteRequest.Headers.Accept.Add(new("application/json"));
            deleteRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");

            string inId = (string)Newtonsoft.Json.Linq.JObject.Parse(apiResult)["inputId"][0];
            var deleteJson = new Newtonsoft.Json.Linq.JObject { ["ids"] = inId };
            deleteRequest.Content = new StringContent(deleteJson.ToString(), Encoding.UTF8, "application/json");
                var deleteResponse = await httpClient.SendAsync(deleteRequest);
                var deleteResult = await deleteResponse.Content.ReadAsStringAsync();
                Console.WriteLine(deleteResult);
            }
        }
    }
}

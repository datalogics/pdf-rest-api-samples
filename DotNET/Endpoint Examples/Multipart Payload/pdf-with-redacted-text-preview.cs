using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com") })
{
    using (var request = new HttpRequestMessage(HttpMethod.Post, "pdf-with-redacted-text-preview"))
    {
        request.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
        request.Headers.Accept.Add(new("application/json"));
        var multipartContent = new MultipartFormDataContent();

        var byteArray = File.ReadAllBytes("/path/to/file");
        var byteAryContent = new ByteArrayContent(byteArray);
        multipartContent.Add(byteAryContent, "file", "file_name.pdf");
        byteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/pdf");

        var redaction_option_array = new JArray();
        var redaction_option1 = new JObject
        {
            ["type"] = "regex",
            ["value"] = "(?:\\(\\d{3}\\)\\s?|\\d{3}[-.\\s]?)?\\d{3}[-.\\s]?\\d{4}"
        };
        var redaction_option2 = new JObject
        {
            ["type"] = "literal",
            ["value"] = "word"
        };
        redaction_option_array.Add(redaction_option1);
        redaction_option_array.Add(redaction_option2);
        var byteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(redaction_option_array)));
        multipartContent.Add(byteArrayOption, "redactions");


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

        using (var deleteRequest = new HttpRequestMessage(HttpMethod.Post, "delete"))
        {
            deleteRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
            deleteRequest.Headers.Accept.Add(new("application/json"));
            deleteRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");

            var parsed = Newtonsoft.Json.Linq.JObject.Parse(apiResult);
            var inId = parsed["inputId"].ToString();
            var outId = parsed["outputId"].ToString();
            // IMPORTANT: Do not delete the outId (the preview PDF) file until after the redaction is applied
            // with the /pdf-with-redacted-text-applied endpoint.
            var deleteJson = new Newtonsoft.Json.Linq.JObject { ["ids"] = inId + ", " + outId };
            deleteRequest.Content = new StringContent(deleteJson.ToString(), Encoding.UTF8, "application/json");
            var deleteResponse = await httpClient.SendAsync(deleteRequest);
            var deleteResult = await deleteResponse.Content.ReadAsStringAsync();
            Console.WriteLine(deleteResult);
        }
    }
}

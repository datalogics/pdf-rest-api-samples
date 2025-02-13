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
        multipartContent.Add(byteAryContent, "file", "file_name");
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
    }
}

using Newtonsoft.Json.Linq;
using System.Text;

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
        using (var markdownRequest = new HttpRequestMessage(HttpMethod.Post, "markdown"))
        {
            markdownRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
            markdownRequest.Headers.Accept.Add(new("application/json"));
            markdownRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");

            JObject parameterJson = new JObject
            {
                ["id"] = uploadedID,
            };

            markdownRequest.Content = new StringContent(parameterJson.ToString(), Encoding.UTF8, "application/json");
            var markdownResponse = await httpClient.SendAsync(markdownRequest);

            var markdownResult = await markdownResponse.Content.ReadAsStringAsync();

            Console.WriteLine("Markdown response received.");
            Console.WriteLine(markdownResult);
        }
    }
}
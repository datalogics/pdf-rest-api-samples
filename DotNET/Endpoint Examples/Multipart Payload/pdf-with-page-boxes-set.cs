using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com") })
{
    using (var request = new HttpRequestMessage(HttpMethod.Post, "pdf-with-page-boxes-set"))
    {
        request.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
        request.Headers.Accept.Add(new("application/json"));
        var multipartContent = new MultipartFormDataContent();

        var byteArray = File.ReadAllBytes("/path/to/file");
        var byteAryContent = new ByteArrayContent(byteArray);
        multipartContent.Add(byteAryContent, "file", "file_name.pdf");
        byteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/pdf");

        var boxes_option_array = new JArray();
        var boxes_option1 = new JObject
        {
            ["box"] = "media",
            ["pages"] = new JArray();
        };
        var pages_option1 = new JObject
        {
            ["range"] = "1",
            ["left"] = "100",
            ["right"] = "100",
            ["top"] = "100",
            ["bottom"] = "100",
        };
        ((JArray)boxes_option1["pages"]).Add(pages_option1);
        boxes_option_array.Add(boxes_option1);
        var byteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(boxes_option_array)));
        multipartContent.Add(byteArrayOption, "boxes");


        request.Content = multipartContent;
        var response = await httpClient.SendAsync(request);

        var apiResult = await response.Content.ReadAsStringAsync();

        Console.WriteLine("API response received.");
        Console.WriteLine(apiResult);
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com") })
{
    using (var request = new HttpRequestMessage(HttpMethod.Post, "pdf-with-added-text"))
    {
        request.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
        request.Headers.Accept.Add(new("application/json"));
        var multipartContent = new MultipartFormDataContent();

        var byteArray = File.ReadAllBytes("/path/to/file");
        var byteAryContent = new ByteArrayContent(byteArray);
        multipartContent.Add(byteAryContent, "file", "file_name");
        byteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/pdf");

        var text_option_array = new JArray();
        var text_options = new JObject
        {
            ["font"] = "Times New Roman",
            ["max_width"] = "175",
            ["opacity"] = "1",
            ["page"] = "1",
            ["rotation"] = "0",
            ["text"] = "sample text in PDF",
            ["text_color_rgb"] = "0,0,0",
            ["text_size"] = "30",
            ["x"] = "72",
            ["y"] = "144"
        };
        text_option_array.Add(text_options);
        var byteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(text_option_array)));
        multipartContent.Add(byteArrayOption, "text_objects");


        request.Content = multipartContent;
        var response = await httpClient.SendAsync(request);

        var apiResult = await response.Content.ReadAsStringAsync();

        Console.WriteLine("API response received.");
        Console.WriteLine(apiResult);
    }
}

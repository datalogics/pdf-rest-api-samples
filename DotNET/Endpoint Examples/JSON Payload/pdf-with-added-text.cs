
using Newtonsoft.Json;
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
        using (var addedTextRequest = new HttpRequestMessage(HttpMethod.Post, "pdf-with-added-text"))
        {
            addedTextRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
            addedTextRequest.Headers.Accept.Add(new("application/json"));

            addedTextRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");

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

            JObject parameterJson = new JObject
                {
                    ["id"] = uploadedID,
                    ["text_objects"] = JsonConvert.SerializeObject(text_option_array),
                };

            addedTextRequest.Content = new StringContent(parameterJson.ToString(), Encoding.UTF8, "application/json"); ;
            var addedTextResponse = await httpClient.SendAsync(addedTextRequest);

            var addedTextResult = await addedTextResponse.Content.ReadAsStringAsync();

            Console.WriteLine("Processing response received.");
            Console.WriteLine(addedTextResult);
        }
    }
}

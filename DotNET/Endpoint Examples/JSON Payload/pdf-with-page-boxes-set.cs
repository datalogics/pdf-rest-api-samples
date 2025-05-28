
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
        using (var SetBoxesRequest = new HttpRequestMessage(HttpMethod.Post, "pdf-with-page-boxes-set"))
        {
            SetBoxesRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
            SetBoxesRequest.Headers.Accept.Add(new("application/json"));

            SetBoxesRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");

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

            JObject parameterJson = new JObject
                {
                    ["id"] = uploadedID,
                    ["boxes"] = JsonConvert.SerializeObject(boxes_option_array),
                };

            SetBoxesRequest.Content = new StringContent(parameterJson.ToString(), Encoding.UTF8, "application/json"); ;
            var SetBoxesResponse = await httpClient.SendAsync(SetBoxesRequest);

            var SetBoxesResult = await SetBoxesResponse.Content.ReadAsStringAsync();

            Console.WriteLine("Processing response received.");
            Console.WriteLine(SetBoxesResult);
        }
    }
}

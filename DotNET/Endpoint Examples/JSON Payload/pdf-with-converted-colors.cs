
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
        using (var convertColorsRequest = new HttpRequestMessage(HttpMethod.Post, "pdf-with-converted-colors"))
        {
            convertColorsRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
            convertColorsRequest.Headers.Accept.Add(new("application/json"));

            convertColorsRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");


            JObject parameterJson = new JObject
            {
                ["id"] = uploadedID,
                ["color_profile"] = "srgb"
            };

            convertColorsRequest.Content = new StringContent(parameterJson.ToString(), Encoding.UTF8, "application/json"); ;
            var convertColorsResponse = await httpClient.SendAsync(convertColorsRequest);

            var convertColorsResult = await convertColorsResponse.Content.ReadAsStringAsync();

            Console.WriteLine("Processing response received.");
            Console.WriteLine(convertColorsResult);
        }
    }
}

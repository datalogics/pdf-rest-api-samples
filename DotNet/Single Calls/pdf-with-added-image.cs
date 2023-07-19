using System.Text;

using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com") })
{
    using (var request = new HttpRequestMessage(HttpMethod.Post, "pdf-with-added-image"))
    {
        request.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
        request.Headers.Accept.Add(new("application/json"));
        var multipartContent = new MultipartFormDataContent();

        var byteArray = File.ReadAllBytes("/path/to/file");
        var byteAryContent = new ByteArrayContent(byteArray);
        multipartContent.Add(byteAryContent, "file", "file_name");
        byteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/pdf");

        var byteArray2 = File.ReadAllBytes("/path/to/file");
        var byteAryContent2 = new ByteArrayContent(byteArray2);
        multipartContent.Add(byteAryContent2, "image_file", "file_name");
        byteAryContent2.Headers.TryAddWithoutValidation("Content-Type", "image/png");

        var byteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes("1"));
        multipartContent.Add(byteArrayOption, "page");

        var byteArrayOption2 = new ByteArrayContent(Encoding.UTF8.GetBytes("0"));
        multipartContent.Add(byteArrayOption2, "x");
        var byteArrayOption3 = new ByteArrayContent(Encoding.UTF8.GetBytes("0"));
        multipartContent.Add(byteArrayOption3, "y");

        request.Content = multipartContent;
        var response = await httpClient.SendAsync(request);

        var apiResult = await response.Content.ReadAsStringAsync();

        Console.WriteLine("API response received.");
        Console.WriteLine(apiResult);
    }
}

using System.Text;

using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com") })
{
    using (var request = new HttpRequestMessage(HttpMethod.Post, "merged-pdf"))
    {
        request.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
        request.Headers.Accept.Add(new("application/json"));
        var multipartContent = new MultipartFormDataContent();

        var byteArray = File.ReadAllBytes("/path/to/file");
        var byteAryContent = new ByteArrayContent(byteArray);
        multipartContent.Add(byteAryContent, "file", "file_name");
        byteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/pdf");

        var byteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes("file"));
        multipartContent.Add(byteArrayOption, "type[]");
        var byteArrayOption2 = new ByteArrayContent(Encoding.UTF8.GetBytes("all"));
        multipartContent.Add(byteArrayOption2, "pages[]");


        var byteArray2 = File.ReadAllBytes("/path/to/file");
        var byteAryContent2 = new ByteArrayContent(byteArray2);
        multipartContent.Add(byteAryContent2, "file", "file_name");
        byteAryContent2.Headers.TryAddWithoutValidation("Content-Type", "application/pdf");

        var byteArrayOption3 = new ByteArrayContent(Encoding.UTF8.GetBytes("file"));
        multipartContent.Add(byteArrayOption3, "type[]");
        var byteArrayOption4 = new ByteArrayContent(Encoding.UTF8.GetBytes("all"));
        multipartContent.Add(byteArrayOption4, "pages[]");

        var byteArrayOption5 = new ByteArrayContent(Encoding.UTF8.GetBytes("merged"));
        multipartContent.Add(byteArrayOption5, "output");

        request.Content = multipartContent;
        var response = await httpClient.SendAsync(request);

        var apiResult = await response.Content.ReadAsStringAsync();

        Console.WriteLine("API response received.");
        Console.WriteLine(apiResult);
    }
}

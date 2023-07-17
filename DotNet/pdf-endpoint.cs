using System.Text;

using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com") })
{
    using (var request = new HttpRequestMessage(HttpMethod.Post, "pdf"))
    {
        request.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
        request.Headers.Accept.Add(new("application/json"));
        var multipartContent = new MultipartFormDataContent();

        var byteArray = File.ReadAllBytes("/path/to/input.html");
        var byteAryContent = new ByteArrayContent(byteArray);
        multipartContent.Add(byteAryContent, "file", "input.html");
        byteAryContent.Headers.TryAddWithoutValidation("Content-Type", "text/html");

        var byteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes("A4"));
        multipartContent.Add(byteArrayOption, "page_size");

        var byteArrayOption2 = new ByteArrayContent(Encoding.UTF8.GetBytes("output.pdf"));
        multipartContent.Add(byteArrayOption2, "output");
        request.Content = multipartContent;
        var response = await httpClient.SendAsync(request);

        var apiResult = await response.Content.ReadAsStringAsync();

        Console.WriteLine("API response received.");
        Console.WriteLine(apiResult);
    }
}

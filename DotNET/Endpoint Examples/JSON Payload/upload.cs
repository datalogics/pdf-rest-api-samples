
using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com") })
{
    using (var request = new HttpRequestMessage(HttpMethod.Post, "upload"))
    {
        request.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
        request.Headers.Accept.Add(new("application/json"));

        var byteArray = File.ReadAllBytes("/path/to/file");
        var byteAryContent = new ByteArrayContent(byteArray);
        byteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
        byteAryContent.Headers.TryAddWithoutValidation("Content-Filename", "filename.pdf");


        request.Content = byteAryContent;
        var response = await httpClient.SendAsync(request);

        var apiResult = await response.Content.ReadAsStringAsync();

        Console.WriteLine("Upload response received.");
        Console.WriteLine(apiResult);
    }
}

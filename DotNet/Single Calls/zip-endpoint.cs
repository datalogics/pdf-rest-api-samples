using System.Text;

using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com") })
{
    using (var request = new HttpRequestMessage(HttpMethod.Post, "zip"))
    {
        request.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
        request.Headers.Accept.Add(new("application/json"));
        var multipartContent = new MultipartFormDataContent();

        var byteArray = File.ReadAllBytes("/path/to/file1.pdf");
        var byteAryContent = new ByteArrayContent(byteArray);
        multipartContent.Add(byteAryContent, "file", "file1.pdf");
        byteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/pdf");





        var byteArray2 = File.ReadAllBytes("/path/to/file2.pdf");
        var byteAryContent2 = new ByteArrayContent(byteArray2);
        multipartContent.Add(byteAryContent2, "file", "file2.pdf");
        byteAryContent2.Headers.TryAddWithoutValidation("Content-Type", "application/pdf");


        request.Content = multipartContent;
        var response = await httpClient.SendAsync(request);

        var apiResult = await response.Content.ReadAsStringAsync();

        Console.WriteLine("API response received.");
        Console.WriteLine(apiResult);
    }
}
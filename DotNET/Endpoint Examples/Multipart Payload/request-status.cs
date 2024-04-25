using System.Text;

using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com") })
{
    string requestId = "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";  // requestId to poll
    using (var request = new HttpRequestMessage(HttpMethod.Get, "request-status" + requestId))
    {
        request.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");

        var response = await httpClient.SendAsync(request);

        var apiResult = await response.Content.ReadAsStringAsync();

        Console.WriteLine("API response received.");
        Console.WriteLine(apiResult);
    }
}

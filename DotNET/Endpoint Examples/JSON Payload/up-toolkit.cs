using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com") })
{
    // up-forms and up-office can be used to query the other tools
    using (var request = new HttpRequestMessage(HttpMethod.Get, "up-toolkit"))
    {

        var response = await httpClient.SendAsync(request);

        var apiResult = await response.Content.ReadAsStringAsync();

        Console.WriteLine("API response received.");
        Console.WriteLine(apiResult);
    }
}

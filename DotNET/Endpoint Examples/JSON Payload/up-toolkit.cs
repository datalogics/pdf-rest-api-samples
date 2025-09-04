namespace Samples.EndpointExamples.JsonPayload
{
    public static class UpToolkit
    {
        public static async Task Execute(string[] args)
        {
            var baseUrl = Environment.GetEnvironmentVariable("PDFREST_URL") ?? "https://api.pdfrest.com";
            using (var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) })
            using (var request = new HttpRequestMessage(HttpMethod.Get, "up-toolkit"))
            {
                var response = await httpClient.SendAsync(request);
                var apiResult = await response.Content.ReadAsStringAsync();
                Console.WriteLine("API response received.");
                Console.WriteLine(apiResult);
            }
        }
    }
}

namespace Samples.EndpointExamples.JsonPayload
{
    public static class DeleteResource
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Console.Error.WriteLine("delete-resource requires <resourceId>");
                Environment.Exit(1);
                return;
            }

            var id = args[0];
            var apiKey = Environment.GetEnvironmentVariable("PDFREST_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Console.Error.WriteLine("Missing required environment variable: PDFREST_API_KEY");
                Environment.Exit(1);
                return;
            }
            var baseUrl = Environment.GetEnvironmentVariable("PDFREST_URL") ?? "https://api.pdfrest.com";
            var url = baseUrl.TrimEnd('/') + "/resource/" + id;

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.TryAddWithoutValidation("Api-Key", apiKey);
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}

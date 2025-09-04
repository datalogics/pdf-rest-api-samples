/*
 * What this sample does:
 * - Called from Program.cs to delete a resource by id and print the JSON response.
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   dotnet run -- delete-resource <id>
 *
 * Output:
 * - Prints JSON responses; non-2xx results exit non-zero.
 */
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

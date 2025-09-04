/*
 * What this sample does:
 * - Deletes a resource by id.
 * - Routed from Program.cs as: `dotnet run -- delete-resource-multipart <id>`.
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 */

namespace Samples.EndpointExamples.MultipartPayload
{
    public static class DeleteResource
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Console.Error.WriteLine("delete-resource-multipart requires <id>");
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

            using var client = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.TryAddWithoutValidation("Api-Key", apiKey);
            var response = await client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);
        }
    }
}

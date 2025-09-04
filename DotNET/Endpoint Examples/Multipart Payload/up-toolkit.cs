/*
 * What this sample does:
 * - Queries toolkit status (GET) using multipart folder style for parity.
 * - Routed from Program.cs as: `dotnet run -- up-toolkit-multipart`.
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   dotnet run -- up-toolkit-multipart
 *
 * Output:
 * - Prints JSON toolkit status.
 */

namespace Samples.EndpointExamples.MultipartPayload
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

/*
 * What this sample does:
 * - Requests a new blank PDF using the JSON payload route.
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   dotnet run -- blank-pdf
 *
 * Output:
 * - Prints JSON response for the generated blank document.
 */
using Newtonsoft.Json.Linq;
using System.Text;

namespace Samples.EndpointExamples.JsonPayload
{
    public static class BlankPdf
    {
        public static async Task Execute(string[] args)
        {
            var apiKey = Environment.GetEnvironmentVariable("PDFREST_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Console.Error.WriteLine("Missing required environment variable: PDFREST_API_KEY");
                Environment.Exit(1);
                return;
            }

            var baseUrl = Environment.GetEnvironmentVariable("PDFREST_URL") ?? "https://api.pdfrest.com";

            using (var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) })
            using (var request = new HttpRequestMessage(HttpMethod.Post, "blank-pdf"))
            {
                request.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                request.Headers.Accept.Add(new("application/json"));

                var payload = new JObject
                {
                    ["page_size"] = "letter",
                    ["page_count"] = 3,
                    ["page_orientation"] = "portrait"
                };

                request.Content = new StringContent(payload.ToString(), Encoding.UTF8, "application/json");

                var response = await httpClient.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();

                Console.WriteLine("Blank PDF response received.");
                Console.WriteLine(result);

                if (!response.IsSuccessStatusCode)
                {
                    Environment.ExitCode = 1;
                }
            }
        }
    }
}

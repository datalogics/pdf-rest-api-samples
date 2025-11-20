/*
 * What this sample does:
 * - Calls /blank-pdf via multipart/form-data to create a three-page blank PDF.
 * - Routed from Program.cs as: `dotnet run -- blank-pdf-multipart`.
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   dotnet run -- blank-pdf-multipart
 *
 * Output:
 * - Prints the JSON response. Validation errors (args/env) exit non-zero.
 */

namespace Samples.EndpointExamples.MultipartPayload
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

                var multipartContent = new MultipartFormDataContent
                {
                    { new StringContent("letter"), "page_size" },
                    { new StringContent("3"), "page_count" },
                    { new StringContent("portrait"), "page_orientation" }
                };

                request.Content = multipartContent;
                var response = await httpClient.SendAsync(request);
                var apiResult = await response.Content.ReadAsStringAsync();

                Console.WriteLine("blank-pdf response received.");
                Console.WriteLine(apiResult);

                if (!response.IsSuccessStatusCode)
                {
                    Environment.ExitCode = 1;
                }
            }
        }
    }
}

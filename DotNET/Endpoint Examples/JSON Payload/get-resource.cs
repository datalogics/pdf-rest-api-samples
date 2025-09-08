/*
 * What this sample does:
 * - Called from Program.cs to download a resource by id (optional output path).
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   dotnet run -- get-resource <id> [outputFile]
 *
 * Output:
 * - Writes file to disk; non-2xx results exit non-zero.
 */
namespace Samples.EndpointExamples.JsonPayload
{
    public static class GetResource
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Console.Error.WriteLine("get-resource requires <resourceId> [outputFile]");
                Environment.Exit(1);
                return;
            }

            var id = args[0];
            var outputPath = args.Length > 1 ? args[1] : "download.bin";

            var baseUrl = Environment.GetEnvironmentVariable("PDFREST_URL") ?? "https://api.pdfrest.com";
            var resourceBase = baseUrl.TrimEnd('/') + "/resource/";

            using (var httpClient = new HttpClient { BaseAddress = new Uri(resourceBase) })
            {
                try
                {
                    using (var stream = await httpClient.GetStreamAsync(id + "?format=file"))
                    using (var fs = new FileStream(outputPath, FileMode.Create))
                    {
                        await stream.CopyToAsync(fs);
                    }
                    Console.WriteLine($"Saved to {outputPath}");
                }
                catch (HttpRequestException e)
                {
                    Console.Error.WriteLine($"HTTP error: {e.Message}");
                    Environment.Exit(1);
                }
            }
        }
    }
}

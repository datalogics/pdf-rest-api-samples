/*
 * What this sample does:
 * - Adds an attachment to a PDF via multipart/form-data.
 * - Routed from Program.cs as: `dotnet run -- pdf-with-added-attachment-multipart <pdf> <file>`.
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   dotnet run -- pdf-with-added-attachment-multipart input.pdf attachment.bin
 *
 * Output:
 * - Prints the JSON response. Validation errors (args/env) exit non-zero.
 */

using System.Text;

namespace Samples.EndpointExamples.MultipartPayload
{
    public static class PdfWithAddedAttachment
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                Console.Error.WriteLine("pdf-with-added-attachment-multipart requires <pdf> <file>");
                Environment.Exit(1);
                return;
            }
            var pdfPath = args[0];
            var attachPath = args[1];
            if (!File.Exists(pdfPath) || !File.Exists(attachPath))
            {
                Console.Error.WriteLine("One or more input files not found.");
                Environment.Exit(1);
                return;
            }
            var apiKey = Environment.GetEnvironmentVariable("PDFREST_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Console.Error.WriteLine("Missing required environment variable: PDFREST_API_KEY");
                Environment.Exit(1);
                return;
            }
            var baseUrl = Environment.GetEnvironmentVariable("PDFREST_URL") ?? "https://api.pdfrest.com";

            using (var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) })
            using (var request = new HttpRequestMessage(HttpMethod.Post, "pdf-with-added-attachment"))
            {
                request.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                request.Headers.Accept.Add(new("application/json"));
                var multipartContent = new MultipartFormDataContent();

                var byteArray = File.ReadAllBytes(pdfPath);
                var byteAryContent = new ByteArrayContent(byteArray);
                multipartContent.Add(byteAryContent, "file", Path.GetFileName(pdfPath));
                byteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");

                var byteArray2 = File.ReadAllBytes(attachPath);
                var byteAryContent2 = new ByteArrayContent(byteArray2);
                multipartContent.Add(byteAryContent2, "file_to_attach", Path.GetFileName(attachPath));
                byteAryContent2.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");

                request.Content = multipartContent;
                var response = await httpClient.SendAsync(request);
                var apiResult = await response.Content.ReadAsStringAsync();

                Console.WriteLine("API response received.");
                Console.WriteLine(apiResult);
            }
        }
    }
}

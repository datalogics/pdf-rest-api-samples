/*
 * What this sample does:
 * - Previews redactions and then applies them using the preview output.
 * - Routed from Program.cs as: `dotnet run -- redact-preview-and-finalize <pdf>`.
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   dotnet run -- redact-preview-and-finalize /path/to/input.pdf
 *
 * Output:
 * - Prints JSON responses for preview and finalize steps.
 */

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Samples.ComplexFlowExamples
{
    public static class RedactPreviewAndFinalize
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Console.Error.WriteLine("redact-preview-and-finalize requires <pdf>");
                Environment.Exit(1);
                return;
            }
            var inputPath = args[0];
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"File not found: {inputPath}");
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
            {
                // Preview redaction
                using var previewRequest = new HttpRequestMessage(HttpMethod.Post, "pdf-with-redacted-text-preview");
                previewRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                previewRequest.Headers.Accept.Add(new("application/json"));
                var previewMultipartContent = new MultipartFormDataContent();
                var byteArray = File.ReadAllBytes(inputPath);
                var byteAryContent = new ByteArrayContent(byteArray);
                previewMultipartContent.Add(byteAryContent, "file", Path.GetFileName(inputPath));
                byteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
                var redactionArray = new JArray
                {
                    new JObject { ["type"] = "regex", ["value"] = "[Tt]he" }
                };
                var byteArrayRedOption = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(redactionArray)));
                previewMultipartContent.Add(byteArrayRedOption, "redactions");
                previewRequest.Content = previewMultipartContent;
                var pdfResponse = await httpClient.SendAsync(previewRequest);
                var pdfResult = await pdfResponse.Content.ReadAsStringAsync();
                Console.WriteLine("Redaction preview response received.");
                Console.WriteLine(pdfResult);
                dynamic responseData = JObject.Parse(pdfResult);
                string pdfID = responseData.outputId;

                // Apply finalization
                using var finalizeRequest = new HttpRequestMessage(HttpMethod.Post, "pdf-with-redacted-text-applied");
                finalizeRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                finalizeRequest.Headers.Accept.Add(new("application/json"));
                var finalMultipartContent = new MultipartFormDataContent();
                var byteArrayIdOption = new ByteArrayContent(Encoding.UTF8.GetBytes(pdfID));
                finalMultipartContent.Add(byteArrayIdOption, "id");
                finalizeRequest.Content = finalMultipartContent;
                var response = await httpClient.SendAsync(finalizeRequest);
                var apiResult = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Finalized redaction response received.");
                Console.WriteLine(apiResult);
            }
        }
    }
}

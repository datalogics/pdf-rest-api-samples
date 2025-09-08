
/*
 * What this sample does:
 * - Called from Program.cs to attach a file to a PDF via JSON flow (uploads PDF + attachment).
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   dotnet run -- pdf-with-added-attachment doc.pdf appendix.pdf
 *
 * Output:
 * - Prints JSON responses; non-2xx results exit non-zero.
 */
using Newtonsoft.Json.Linq;
using System.Text;

namespace Samples.EndpointExamples.JsonPayload
{
    public static class PdfWithAddedAttachment
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                Console.Error.WriteLine("pdf-with-added-attachment requires <pdfFile> <attachmentFile>");
                Environment.Exit(1);
                return;
            }
            var pdfFile = args[0];
            var attachmentFile = args[1];
            if (!File.Exists(pdfFile) || !File.Exists(attachmentFile)) { Console.Error.WriteLine("One or more files not found."); Environment.Exit(1); return; }
            var apiKey = Environment.GetEnvironmentVariable("PDFREST_API_KEY"); if (string.IsNullOrWhiteSpace(apiKey)) { Console.Error.WriteLine("Missing required environment variable: PDFREST_API_KEY"); Environment.Exit(1); return; }
            var baseUrl = Environment.GetEnvironmentVariable("PDFREST_URL") ?? "https://api.pdfrest.com";
            using (var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) })
            {
                // Upload PDF
                var pdfUploadRequest = new HttpRequestMessage(HttpMethod.Post, "upload");
                pdfUploadRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                pdfUploadRequest.Headers.Accept.Add(new("application/json"));
                var pdfBytes = File.ReadAllBytes(pdfFile);
                var pdfContent = new ByteArrayContent(pdfBytes);
                pdfContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
                pdfContent.Headers.TryAddWithoutValidation("Content-Filename", Path.GetFileName(pdfFile));
                pdfUploadRequest.Content = pdfContent;
                var pdfUploadResponse = await httpClient.SendAsync(pdfUploadRequest);
                var pdfUploadResult = await pdfUploadResponse.Content.ReadAsStringAsync();
                JObject pdfJson = JObject.Parse(pdfUploadResult);
                var pdfId = pdfJson["files"][0]["id"];

                // Upload attachment
                var attUploadRequest = new HttpRequestMessage(HttpMethod.Post, "upload");
                attUploadRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                attUploadRequest.Headers.Accept.Add(new("application/json"));
                var attBytes = File.ReadAllBytes(attachmentFile);
                var attContent = new ByteArrayContent(attBytes);
                attContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
                attContent.Headers.TryAddWithoutValidation("Content-Filename", Path.GetFileName(attachmentFile));
                attUploadRequest.Content = attContent;
                var attUploadResponse = await httpClient.SendAsync(attUploadRequest);
                var attUploadResult = await attUploadResponse.Content.ReadAsStringAsync();
                JObject attJson = JObject.Parse(attUploadResult);
                var attId = attJson["files"][0]["id"];

                using (var attachRequest = new HttpRequestMessage(HttpMethod.Post, "pdf-with-added-attachment"))
                {
                    attachRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                    attachRequest.Headers.Accept.Add(new("application/json"));
                    attachRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");
                    JObject parameterJson = new JObject { ["id"] = pdfId, ["id_to_attach"] = attId };
                    attachRequest.Content = new StringContent(parameterJson.ToString(), Encoding.UTF8, "application/json");
                    var attachResponse = await httpClient.SendAsync(attachRequest);
                    var attachResult = await attachResponse.Content.ReadAsStringAsync();
                    Console.WriteLine("Processing response received.");
                    Console.WriteLine(attachResult);
                }
            }
        }
    }
}

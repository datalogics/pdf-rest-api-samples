/*
 * What this sample does:
 * - Converts an Office file to PDF, then to PDF/A-3b for preservation.
 * - Routed from Program.cs as: `dotnet run -- preserve-word-document <officeFile>`.
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   dotnet run -- preserve-word-document /path/to/input.docx
 *
 * Output:
 * - Prints JSON responses for PDF and PDF/A conversions.
 */

using Newtonsoft.Json.Linq;
using System.Text;

namespace Samples.ComplexFlowExamples
{
    public static class PreserveWordDocument
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Console.Error.WriteLine("preserve-word-document requires <officeFile>");
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
                using var pdfRequest = new HttpRequestMessage(HttpMethod.Post, "pdf");
                pdfRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                pdfRequest.Headers.Accept.Add(new("application/json"));
                var pdfMultipartContent = new MultipartFormDataContent();
                var byteArray = File.ReadAllBytes(inputPath);
                var byteAryContent = new ByteArrayContent(byteArray);
                pdfMultipartContent.Add(byteAryContent, "file", Path.GetFileName(inputPath));
                byteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
                pdfRequest.Content = pdfMultipartContent;
                var pdfResponse = await httpClient.SendAsync(pdfRequest);
                var pdfResult = await pdfResponse.Content.ReadAsStringAsync();
                Console.WriteLine("PDF response received.");
                Console.WriteLine(pdfResult);
                dynamic responseData = JObject.Parse(pdfResult);
                string pdfID = responseData.outputId;

                using var pdfaRequest = new HttpRequestMessage(HttpMethod.Post, "pdfa");
                pdfaRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                pdfaRequest.Headers.Accept.Add(new("application/json"));
                var multipartContent = new MultipartFormDataContent();
                var byteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes(pdfID));
                multipartContent.Add(byteArrayOption, "id");
                var byteArrayOption2 = new ByteArrayContent(Encoding.UTF8.GetBytes("PDF/A-3b"));
                multipartContent.Add(byteArrayOption2, "output_type");
                pdfaRequest.Content = multipartContent;
                var response = await httpClient.SendAsync(pdfaRequest);
                var apiResult = await response.Content.ReadAsStringAsync();
                Console.WriteLine("PDF/A response received.");
                Console.WriteLine(apiResult);
            }
        }
    }
}

/*
 * What this sample does:
 * - Adds a watermark and locks the PDF with restrictions to protect it.
 * - Routed from Program.cs as: `dotnet run -- protected-watermark <pdf>`.
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   dotnet run -- protected-watermark /path/to/input.pdf
 *
 * Output:
 * - Prints JSON responses for watermark and restriction steps.
 */

using Newtonsoft.Json.Linq;
using System.Text;

namespace Samples.ComplexFlowExamples
{
    public static class ProtectedWatermark
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Console.Error.WriteLine("protected-watermark requires <pdf>");
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
                using var watermarkRequest = new HttpRequestMessage(HttpMethod.Post, "watermarked-pdf");
                watermarkRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                watermarkRequest.Headers.Accept.Add(new("application/json"));
                var watermarkMultipartContent = new MultipartFormDataContent();
                var watermarkByteArray = File.ReadAllBytes(inputPath);
                var watermarkByteAryContent = new ByteArrayContent(watermarkByteArray);
                watermarkMultipartContent.Add(watermarkByteAryContent, "file", Path.GetFileName(inputPath));
                watermarkByteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
                var watermarkByteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes("Watermarked"));
                watermarkMultipartContent.Add(watermarkByteArrayOption, "watermark_text");
                watermarkRequest.Content = watermarkMultipartContent;
                var watermarkResponse = await httpClient.SendAsync(watermarkRequest);
                var watermarkResult = await watermarkResponse.Content.ReadAsStringAsync();
                Console.WriteLine("Watermark to PDF response received.");
                Console.WriteLine(watermarkResult);
                dynamic watermarkResponseData = JObject.Parse(watermarkResult);
                string watermarkID = watermarkResponseData.outputId;

                using var restrictRequest = new HttpRequestMessage(HttpMethod.Post, "restricted-pdf");
                restrictRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                restrictRequest.Headers.Accept.Add(new("application/json"));
                var restrictMultipartContent = new MultipartFormDataContent();
                var watermarkIDByteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes(watermarkID));
                restrictMultipartContent.Add(watermarkIDByteArrayOption, "id");
                var byteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes("password"));
                restrictMultipartContent.Add(byteArrayOption, "new_permissions_password");
                restrictMultipartContent.Add(new ByteArrayContent(Encoding.UTF8.GetBytes("copy_content")), "restrictions[]");
                restrictMultipartContent.Add(new ByteArrayContent(Encoding.UTF8.GetBytes("edit_content")), "restrictions[]");
                restrictMultipartContent.Add(new ByteArrayContent(Encoding.UTF8.GetBytes("edit_annotations")), "restrictions[]");
                restrictRequest.Content = restrictMultipartContent;
                var restrictResponse = await httpClient.SendAsync(restrictRequest);
                var apiResult = await restrictResponse.Content.ReadAsStringAsync();
                Console.WriteLine("Restrict response received.");
                Console.WriteLine(apiResult);
            }
        }
    }
}

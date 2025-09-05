/*
 * What this sample does:
 * - Adds text to a PDF via multipart/form-data.
 * - Routed from Program.cs as: `dotnet run -- pdf-with-added-text-multipart <inputFile>`.
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   dotnet run -- pdf-with-added-text-multipart /path/to/input.pdf
 *
 * Output:
 * - Prints the JSON response. Validation errors (args/env) exit non-zero.
 */

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Samples.EndpointExamples.MultipartPayload
{
    public static class PdfWithAddedText
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Console.Error.WriteLine("pdf-with-added-text-multipart requires <inputFile>");
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
            using (var request = new HttpRequestMessage(HttpMethod.Post, "pdf-with-added-text"))
            {
                request.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                request.Headers.Accept.Add(new("application/json"));
                var multipartContent = new MultipartFormDataContent();

                var byteArray = File.ReadAllBytes(inputPath);
                var byteAryContent = new ByteArrayContent(byteArray);
                multipartContent.Add(byteAryContent, "file", Path.GetFileName(inputPath));
                byteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");

                var text_option_array = new JArray();
                var text_options = new JObject
                {
                    ["font"] = "Times New Roman",
                    ["max_width"] = "175",
                    ["opacity"] = "1",
                    ["page"] = "1",
                    ["rotation"] = "0",
                    ["text"] = "sample text in PDF",
                    ["text_color_rgb"] = "0,0,0",
                    ["text_size"] = "30",
                    ["x"] = "72",
                    ["y"] = "144"
                };
                text_option_array.Add(text_options);
                var byteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(text_option_array)));
                multipartContent.Add(byteArrayOption, "text_objects");

                request.Content = multipartContent;
                var response = await httpClient.SendAsync(request);
                var apiResult = await response.Content.ReadAsStringAsync();

                Console.WriteLine("API response received.");
                Console.WriteLine(apiResult);
            }
        }
    }
}

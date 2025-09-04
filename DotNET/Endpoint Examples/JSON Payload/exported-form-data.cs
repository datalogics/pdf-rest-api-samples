/*
 * What this sample does:
 * - Uploads a PDF, then exports form data via the JSON two-step flow.
 * - Routed from Program.cs as: `dotnet run -- exported-form-data <inputFile>`.
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   dotnet run -- exported-form-data /path/to/input.pdf
 *
 * Output:
 * - Prints the JSON response from the export operation; non-2xx results print the body and exit non-zero.
 */
using Newtonsoft.Json.Linq;
using System.Text;

namespace Samples.EndpointExamples.JsonPayload
{
    public static class ExportedFormData
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Console.Error.WriteLine("exported-form-data requires <inputFile>");
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
                // Upload first
                using (var uploadRequest = new HttpRequestMessage(HttpMethod.Post, "upload"))
                {
                    uploadRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                    uploadRequest.Headers.Accept.Add(new("application/json"));

                    var uploadByteArray = File.ReadAllBytes(inputPath);
                    var uploadByteAryContent = new ByteArrayContent(uploadByteArray);
                    uploadByteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
                    uploadByteAryContent.Headers.TryAddWithoutValidation("Content-Filename", Path.GetFileName(inputPath));

                    uploadRequest.Content = uploadByteAryContent;
                    var uploadResponse = await httpClient.SendAsync(uploadRequest);
                    var uploadResult = await uploadResponse.Content.ReadAsStringAsync();
                    Console.WriteLine("Upload response received.");
                    Console.WriteLine(uploadResult);

                    JObject uploadResultJson = JObject.Parse(uploadResult);
                    var uploadedID = uploadResultJson["files"]?[0]?["id"];
                    if (uploadedID == null)
                    {
                        Console.Error.WriteLine("Upload did not return an id.");
                        Environment.Exit(1);
                        return;
                    }

                    // Export form data
                    using (var exportRequest = new HttpRequestMessage(HttpMethod.Post, "exported-form-data"))
                    {
                        exportRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                        exportRequest.Headers.Accept.Add(new("application/json"));
                        exportRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");

                        JObject parameterJson = new JObject
                        {
                            ["id"] = uploadedID,
                            ["data_format"] = "xml",
                        };

                        exportRequest.Content = new StringContent(parameterJson.ToString(), Encoding.UTF8, "application/json");
                        var exportResponse = await httpClient.SendAsync(exportRequest);
                        var exportResult = await exportResponse.Content.ReadAsStringAsync();
                        Console.WriteLine("Processing response received.");
                        Console.WriteLine(exportResult);
                    }
                }
            }
        }
    }
}

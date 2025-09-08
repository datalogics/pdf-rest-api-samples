
/*
 * What this sample does:
 * - Called from Program.cs to import external form data into a PDF via JSON flow.
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   dotnet run -- pdf-with-imported-form-data form.pdf data.xml
 *
 * Output:
 * - Prints JSON responses; non-2xx results exit non-zero.
 */
using Newtonsoft.Json.Linq;
using System.Text;

namespace Samples.EndpointExamples.JsonPayload
{
    public static class PdfWithImportedFormData
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                Console.Error.WriteLine("pdf-with-imported-form-data requires <pdfFile> <dataFile>");
                Environment.Exit(1);
                return;
            }

            var pdfFile = args[0];
            var dataFile = args[1];
            if (!File.Exists(pdfFile) || !File.Exists(dataFile))
            {
                Console.Error.WriteLine("One or more files not found.");
                Environment.Exit(1);
                return;
            }

            var apiKey = Environment.GetEnvironmentVariable("PDFREST_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey)) { Console.Error.WriteLine("Missing required environment variable: PDFREST_API_KEY"); Environment.Exit(1); return; }
            var baseUrl = Environment.GetEnvironmentVariable("PDFREST_URL") ?? "https://api.pdfrest.com";

            using (var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) })
            {
                using (var pdfUploadRequest = new HttpRequestMessage(HttpMethod.Post, "upload"))
                {
                    pdfUploadRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                    pdfUploadRequest.Headers.Accept.Add(new("application/json"));

                    var pdfUploadByteArray = File.ReadAllBytes(pdfFile);
                    var pdfUploadByteAryContent = new ByteArrayContent(pdfUploadByteArray);
                    pdfUploadByteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
                    pdfUploadByteAryContent.Headers.TryAddWithoutValidation("Content-Filename", Path.GetFileName(pdfFile));

                    pdfUploadRequest.Content = pdfUploadByteAryContent;
                    var pdfUploadResponse = await httpClient.SendAsync(pdfUploadRequest);
                    var pdfUploadResult = await pdfUploadResponse.Content.ReadAsStringAsync();
                    Console.WriteLine("PDF upload response received.");
                    Console.WriteLine(pdfUploadResult);
                    JObject pdfUploadResultJson = JObject.Parse(pdfUploadResult);
                    var pdfUploadedID = pdfUploadResultJson["files"][0]["id"];

                    using (var dataUploadRequest = new HttpRequestMessage(HttpMethod.Post, "upload"))
                    {
                        dataUploadRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                        dataUploadRequest.Headers.Accept.Add(new("application/json"));

                        var dataUploadByteArray = File.ReadAllBytes(dataFile);
                        var dataUploadByteAryContent = new ByteArrayContent(dataUploadByteArray);
                        dataUploadByteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
                        dataUploadByteAryContent.Headers.TryAddWithoutValidation("Content-Filename", Path.GetFileName(dataFile));

                        dataUploadRequest.Content = dataUploadByteAryContent;
                        var dataUploadResponse = await httpClient.SendAsync(dataUploadRequest);
                        var dataUploadResult = await dataUploadResponse.Content.ReadAsStringAsync();
                        Console.WriteLine("Data upload response received.");
                        Console.WriteLine(dataUploadResult);

                        JObject dataUploadResultJson = JObject.Parse(dataUploadResult);
                        var dataUploadedID = dataUploadResultJson["files"][0]["id"];
                        using (var attachRequest = new HttpRequestMessage(HttpMethod.Post, "pdf-with-imported-form-data"))
                        {
                            attachRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                            attachRequest.Headers.Accept.Add(new("application/json"));
                            attachRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");

                            JObject parameterJson = new JObject { ["id"] = pdfUploadedID, ["data_file_id"] = dataUploadedID };
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
    }
}

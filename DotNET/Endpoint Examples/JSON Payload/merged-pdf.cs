
/*
 * What this sample does:
 * - Called from Program.cs to upload two files, then request merged-pdf.
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   dotnet run -- merged-pdf /path/to/file1.pdf /path/to/file2.pdf
 *
 * Output:
 * - Prints JSON responses; non-2xx results exit non-zero.
 */
using Newtonsoft.Json.Linq;
using System.Text;

namespace Samples.EndpointExamples.JsonPayload
{
    public static class MergedPdf
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                Console.Error.WriteLine("merged-pdf requires <firstFile> <secondFile>");
                Environment.Exit(1);
                return;
            }

            var firstPath = args[0];
            var secondPath = args[1];
            if (!File.Exists(firstPath) || !File.Exists(secondPath))
            {
                Console.Error.WriteLine("One or more files not found.");
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
                using (var firstUploadRequest = new HttpRequestMessage(HttpMethod.Post, "upload"))
                {
                    firstUploadRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                    firstUploadRequest.Headers.Accept.Add(new("application/json"));

                    var firstUploadByteArray = File.ReadAllBytes(firstPath);
                    var firstUploadByteAryContent = new ByteArrayContent(firstUploadByteArray);
                    firstUploadByteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
                    firstUploadByteAryContent.Headers.TryAddWithoutValidation("Content-Filename", Path.GetFileName(firstPath));


                    firstUploadRequest.Content = firstUploadByteAryContent;
                    var firstUploadResponse = await httpClient.SendAsync(firstUploadRequest);

                    var firstUploadResult = await firstUploadResponse.Content.ReadAsStringAsync();

                    Console.WriteLine("First upload response received.");
                    Console.WriteLine(firstUploadResult);

                    JObject firstUploadResultJson = JObject.Parse(firstUploadResult);
                    var firstUploadedID = firstUploadResultJson["files"][0]["id"];

                    using (var secondUploadRequest = new HttpRequestMessage(HttpMethod.Post, "upload"))
                    {
                        secondUploadRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                        secondUploadRequest.Headers.Accept.Add(new("application/json"));

                        var secondUploadByteArray = File.ReadAllBytes(secondPath);
                        var secondUploadByteAryContent = new ByteArrayContent(secondUploadByteArray);
                        secondUploadByteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
                        secondUploadByteAryContent.Headers.TryAddWithoutValidation("Content-Filename", Path.GetFileName(secondPath));


                        secondUploadRequest.Content = secondUploadByteAryContent;
                        var secondUploadResponse = await httpClient.SendAsync(secondUploadRequest);

                        var secondUploadResult = await secondUploadResponse.Content.ReadAsStringAsync();

                        Console.WriteLine("Second upload response received.");
                        Console.WriteLine(secondUploadResult);

                        JObject secondUploadResultJson = JObject.Parse(secondUploadResult);
                        var secondUploadedID = secondUploadResultJson["files"][0]["id"];
                        using (var mergeRequest = new HttpRequestMessage(HttpMethod.Post, "merged-pdf"))
                        {
                            mergeRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                            mergeRequest.Headers.Accept.Add(new("application/json"));

                            mergeRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");


                            JObject parameterJson = new JObject
                            {
                                ["id"] = new JArray(firstUploadedID, secondUploadedID),
                                ["pages"] = new JArray(1, 1),
                                ["type"] = new JArray("id", "id"),

                            };

                            mergeRequest.Content = new StringContent(parameterJson.ToString(), Encoding.UTF8, "application/json"); ;
                            var mergeResponse = await httpClient.SendAsync(mergeRequest);

                            var mergeResult = await mergeResponse.Content.ReadAsStringAsync();

                            Console.WriteLine("Processing response received.");
                            Console.WriteLine(mergeResult);
                        }
                    }
                }
            }
        }
    }
}


/*
 * What this sample does:
 * - Called from Program.cs to zip multiple uploaded resources via JSON flow.
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   dotnet run -- zip file1.pdf file2.pdf
 *
 * Output:
 * - Prints JSON responses; non-2xx results exit non-zero.
 */
using Newtonsoft.Json.Linq;
using System.Text;

namespace Samples.EndpointExamples.JsonPayload
{
    public static class Zip
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 2) { Console.Error.WriteLine("zip requires <file1> <file2>"); Environment.Exit(1); return; }
            var file1 = args[0]; var file2 = args[1]; if (!File.Exists(file1) || !File.Exists(file2)) { Console.Error.WriteLine("One or more files not found."); Environment.Exit(1); return; }
            var apiKey = Environment.GetEnvironmentVariable("PDFREST_API_KEY"); if (string.IsNullOrWhiteSpace(apiKey)) { Console.Error.WriteLine("Missing required environment variable: PDFREST_API_KEY"); Environment.Exit(1); return; }
            var baseUrl = Environment.GetEnvironmentVariable("PDFREST_URL") ?? "https://api.pdfrest.com";
            using (var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) })
            {
                // upload first
                var firstUploadRequest = new HttpRequestMessage(HttpMethod.Post, "upload");
                firstUploadRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                firstUploadRequest.Headers.Accept.Add(new("application/json"));
                var b1 = File.ReadAllBytes(file1);
                var c1 = new ByteArrayContent(b1);
                c1.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
                c1.Headers.TryAddWithoutValidation("Content-Filename", Path.GetFileName(file1));
                firstUploadRequest.Content = c1;
                var firstUploadResponse = await httpClient.SendAsync(firstUploadRequest);
                var firstUploadResult = await firstUploadResponse.Content.ReadAsStringAsync();
                JObject j1 = JObject.Parse(firstUploadResult);
                var id1 = j1["files"][0]["id"];

                // upload second
                var secondUploadRequest = new HttpRequestMessage(HttpMethod.Post, "upload");
                secondUploadRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                secondUploadRequest.Headers.Accept.Add(new("application/json"));
                var b2 = File.ReadAllBytes(file2);
                var c2 = new ByteArrayContent(b2);
                c2.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
                c2.Headers.TryAddWithoutValidation("Content-Filename", Path.GetFileName(file2));
                secondUploadRequest.Content = c2;
                var secondUploadResponse = await httpClient.SendAsync(secondUploadRequest);
                var secondUploadResult = await secondUploadResponse.Content.ReadAsStringAsync();
                JObject j2 = JObject.Parse(secondUploadResult);
                var id2 = j2["files"][0]["id"];

                using (var zipRequest = new HttpRequestMessage(HttpMethod.Post, "zip"))
                {
                    zipRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                    zipRequest.Headers.Accept.Add(new("application/json"));
                    zipRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");
                    JObject parameterJson = new JObject { ["id"] = new JArray(id1, id2) };
                    zipRequest.Content = new StringContent(parameterJson.ToString(), Encoding.UTF8, "application/json");
                    var zipResponse = await httpClient.SendAsync(zipRequest);
                    var zipResult = await zipResponse.Content.ReadAsStringAsync();
                    Console.WriteLine("Processing response received.");
                    Console.WriteLine(zipResult);
                }
            }
        }
    }
}

/*
 * What this sample does:
 * - Demonstrates polling with response-type header and request-status.
 * - Routed from Program.cs as: `dotnet run -- request-status-multipart <inputFile>`.
 */

using System.Text;
using Newtonsoft.Json.Linq;

namespace Samples.EndpointExamples.MultipartPayload
{
    public static class RequestStatus
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Console.Error.WriteLine("request-status-multipart requires <inputFile>");
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

            // Send initial request to an arbitrary route (bmp) with response-type header to get requestId
            string bmpResponse = await GetBmpResponseAsync(baseUrl, apiKey, inputPath, Path.GetFileName(inputPath));
            dynamic bmpJson = JObject.Parse(bmpResponse);
            if (bmpJson.ContainsKey("error"))
            {
                Console.Error.WriteLine($"Error from initial request: {bmpJson.error}");
                Environment.Exit(1);
                return;
            }
            string requestId = bmpJson.requestId;
            Console.WriteLine($"Received request ID: {requestId}");

            // Poll request-status until not pending
            string statusResponse = await GetRequestStatusAsync(baseUrl, apiKey, requestId);
            dynamic statusJson = JObject.Parse(statusResponse);
            while (statusJson.status == "pending")
            {
                const int delay = 5;
                Console.WriteLine($"Response from /request-status for request {requestId}: {statusJson}");
                Console.WriteLine($"Request status was \"pending\". Checking again in {delay} seconds...");
                await Task.Delay(TimeSpan.FromSeconds(delay));
                statusResponse = await GetRequestStatusAsync(baseUrl, apiKey, requestId);
                statusJson = JObject.Parse(statusResponse);
            }
            Console.WriteLine($"Response from /request-status: {statusJson}");
            Console.WriteLine("Done!");
        }

        private static async Task<string> GetBmpResponseAsync(string baseUrl, string apiKey, string pathToFile, string fileName)
        {
            using var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
            using var bmpRequest = new HttpRequestMessage(HttpMethod.Post, "bmp");
            bmpRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
            bmpRequest.Headers.Accept.Add(new("application/json"));
            var multipartContent = new MultipartFormDataContent();
            var byteArray = File.ReadAllBytes(pathToFile);
            var byteAryContent = new ByteArrayContent(byteArray);
            multipartContent.Add(byteAryContent, "file", fileName);
            byteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
            bmpRequest.Headers.Add("response-type", "requestId");
            bmpRequest.Content = multipartContent;
            var bmpResponse = await httpClient.SendAsync(bmpRequest);
            return await bmpResponse.Content.ReadAsStringAsync();
        }

        private static async Task<string> GetRequestStatusAsync(string baseUrl, string apiKey, string requestId)
        {
            using var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
            using var request = new HttpRequestMessage(HttpMethod.Get, $"request-status/{requestId}");
            request.Headers.TryAddWithoutValidation("Api-Key", apiKey);
            var response = await httpClient.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
    }
}

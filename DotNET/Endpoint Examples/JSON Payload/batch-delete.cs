using Newtonsoft.Json.Linq;
using System.Text;

namespace Samples.EndpointExamples.JsonPayload
{
    public static class BatchDelete
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Console.Error.WriteLine("batch-delete requires <id1> [id2] [id3] ... OR a single comma-separated list");
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
            var url = baseUrl.TrimEnd('/') + "/delete";

            string ids = args.Length == 1 ? args[0] : string.Join(", ", args);

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.TryAddWithoutValidation("Api-Key", apiKey);
            request.Headers.TryAddWithoutValidation("Content-Type", "application/json");

            JObject parameterJson = new JObject { ["ids"] = ids };
            request.Content = new StringContent(parameterJson.ToString(), Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}


namespace Samples.EndpointExamples.JsonPayload
{
    public static class Upload
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Console.Error.WriteLine("upload requires <inputFile>");
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
                using (var request = new HttpRequestMessage(HttpMethod.Post, "upload"))
                {
                    request.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                    request.Headers.Accept.Add(new("application/json"));

                    var byteArray = File.ReadAllBytes(inputPath);
                    var byteAryContent = new ByteArrayContent(byteArray);
                    byteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
                    byteAryContent.Headers.TryAddWithoutValidation("Content-Filename", Path.GetFileName(inputPath));

                    request.Content = byteAryContent;
                    var response = await httpClient.SendAsync(request);

                    var apiResult = await response.Content.ReadAsStringAsync();

                    Console.WriteLine("Upload response received.");
                    Console.WriteLine(apiResult);
                }
            }
        }
    }
}

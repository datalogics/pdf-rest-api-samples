namespace Samples.EndpointExamples.JsonPayload
{
    public static class GetResource
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Console.Error.WriteLine("get-resource requires <resourceId> [outputFile]");
                Environment.Exit(1);
                return;
            }

            var id = args[0];
            var outputPath = args.Length > 1 ? args[1] : "download.bin";

            var baseUrl = Environment.GetEnvironmentVariable("PDFREST_URL") ?? "https://api.pdfrest.com";
            var resourceBase = baseUrl.TrimEnd('/') + "/resource/";

            using (var httpClient = new HttpClient { BaseAddress = new Uri(resourceBase) })
            {
                try
                {
                    using (var stream = await httpClient.GetStreamAsync(id + "?format=file"))
                    using (var fs = new FileStream(outputPath, FileMode.Create))
                    {
                        await stream.CopyToAsync(fs);
                    }
                    Console.WriteLine($"Saved to {outputPath}");
                }
                catch (HttpRequestException e)
                {
                    Console.Error.WriteLine($"HTTP error: {e.Message}");
                    Environment.Exit(1);
                }
            }
        }
    }
}

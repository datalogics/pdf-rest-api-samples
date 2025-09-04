/*
 * What this sample does:
 * - Merges two input files into one PDF via multipart/form-data.
 * - Routed from Program.cs as: `dotnet run -- merged-pdf-multipart <file1> <file2>`.
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 */

using System.Text;

namespace Samples.EndpointExamples.MultipartPayload
{
    public static class MergedPdf
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                Console.Error.WriteLine("merged-pdf-multipart requires <file1> <file2>");
                Environment.Exit(1);
                return;
            }
            var path1 = args[0];
            var path2 = args[1];
            if (!File.Exists(path1) || !File.Exists(path2))
            {
                Console.Error.WriteLine("One or more input files not found.");
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
            using (var request = new HttpRequestMessage(HttpMethod.Post, "merged-pdf"))
            {
                request.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                request.Headers.Accept.Add(new("application/json"));
                var multipartContent = new MultipartFormDataContent();

                var byteArray = File.ReadAllBytes(path1);
                var byteAryContent = new ByteArrayContent(byteArray);
                multipartContent.Add(byteAryContent, "file", Path.GetFileName(path1));
                byteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");

                var byteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes("file"));
                multipartContent.Add(byteArrayOption, "type[]");
                var byteArrayOption2 = new ByteArrayContent(Encoding.UTF8.GetBytes("all"));
                multipartContent.Add(byteArrayOption2, "pages[]");


                var byteArray2 = File.ReadAllBytes(path2);
                var byteAryContent2 = new ByteArrayContent(byteArray2);
                multipartContent.Add(byteAryContent2, "file", Path.GetFileName(path2));
                byteAryContent2.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");

                var byteArrayOption3 = new ByteArrayContent(Encoding.UTF8.GetBytes("file"));
                multipartContent.Add(byteArrayOption3, "type[]");
                var byteArrayOption4 = new ByteArrayContent(Encoding.UTF8.GetBytes("all"));
                multipartContent.Add(byteArrayOption4, "pages[]");

                var byteArrayOption5 = new ByteArrayContent(Encoding.UTF8.GetBytes("merged"));
                multipartContent.Add(byteArrayOption5, "output");

                request.Content = multipartContent;
                var response = await httpClient.SendAsync(request);
                var apiResult = await response.Content.ReadAsStringAsync();

                Console.WriteLine("API response received.");
                Console.WriteLine(apiResult);
            }
        }
    }
}

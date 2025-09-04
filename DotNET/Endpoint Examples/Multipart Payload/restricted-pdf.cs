/*
 * What this sample does:
 * - Applies permission restrictions with a new permissions password.
 * - Routed from Program.cs as: `dotnet run -- restricted-pdf-multipart <inputFile> <permissionsPassword>`.
 */

using System.Text;

namespace Samples.EndpointExamples.MultipartPayload
{
    public static class RestrictedPdf
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                Console.Error.WriteLine("restricted-pdf-multipart requires <inputFile> <permissionsPassword>");
                Environment.Exit(1);
                return;
            }
            var inputPath = args[0];
            var perm = args[1];
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
            using (var request = new HttpRequestMessage(HttpMethod.Post, "restricted-pdf"))
            {
                request.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                request.Headers.Accept.Add(new("application/json"));
                var multipartContent = new MultipartFormDataContent();

                var byteArray = File.ReadAllBytes(inputPath);
                var byteAryContent = new ByteArrayContent(byteArray);
                multipartContent.Add(byteAryContent, "file", Path.GetFileName(inputPath));
                byteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");

                var byteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes(perm));
                multipartContent.Add(byteArrayOption, "new_permissions_password");

                var byteArrayOption2 = new ByteArrayContent(Encoding.UTF8.GetBytes("copy_content"));
                multipartContent.Add(byteArrayOption2, "restrictions[]");
                var byteArrayOption3 = new ByteArrayContent(Encoding.UTF8.GetBytes("edit_content"));
                multipartContent.Add(byteArrayOption3, "restrictions[]");

                request.Content = multipartContent;
                var response = await httpClient.SendAsync(request);
                var apiResult = await response.Content.ReadAsStringAsync();

                Console.WriteLine("API response received.");
                Console.WriteLine(apiResult);
            }
        }
    }
}

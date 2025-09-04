/*
 * What this sample does:
 * - Adds an image to a PDF via multipart/form-data.
 * - Routed from Program.cs as: `dotnet run -- pdf-with-added-image-multipart <pdf> <image>`.
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   dotnet run -- pdf-with-added-image-multipart input.pdf image.png
 *
 * Output:
 * - Prints the JSON response. Validation errors (args/env) exit non-zero.
 */

using System.Text;

namespace Samples.EndpointExamples.MultipartPayload
{
    public static class PdfWithAddedImage
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                Console.Error.WriteLine("pdf-with-added-image-multipart requires <pdf> <image>");
                Environment.Exit(1);
                return;
            }
            var pdfPath = args[0];
            var imgPath = args[1];
            if (!File.Exists(pdfPath) || !File.Exists(imgPath))
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
            using (var request = new HttpRequestMessage(HttpMethod.Post, "pdf-with-added-image"))
            {
                request.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                request.Headers.Accept.Add(new("application/json"));
                var multipartContent = new MultipartFormDataContent();

                var byteArray = File.ReadAllBytes(pdfPath);
                var byteAryContent = new ByteArrayContent(byteArray);
                multipartContent.Add(byteAryContent, "file", Path.GetFileName(pdfPath));
                byteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");

                var byteArray2 = File.ReadAllBytes(imgPath);
                var byteAryContent2 = new ByteArrayContent(byteArray2);
                multipartContent.Add(byteAryContent2, "image_file", Path.GetFileName(imgPath));
                byteAryContent2.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");

                var byteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes("1"));
                multipartContent.Add(byteArrayOption, "page");
                var byteArrayOption2 = new ByteArrayContent(Encoding.UTF8.GetBytes("0"));
                multipartContent.Add(byteArrayOption2, "x");
                var byteArrayOption3 = new ByteArrayContent(Encoding.UTF8.GetBytes("0"));
                multipartContent.Add(byteArrayOption3, "y");

                request.Content = multipartContent;
                var response = await httpClient.SendAsync(request);
                var apiResult = await response.Content.ReadAsStringAsync();

                Console.WriteLine("API response received.");
                Console.WriteLine(apiResult);
            }
        }
    }
}

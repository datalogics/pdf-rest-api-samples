/*
 * What this sample does:
 * - Converts two different file types to PDF via multipart, then merges them.
 * - Routed from Program.cs as: `dotnet run -- merge-different-file-types <imageFile> <pptFile>`.
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   dotnet run -- merge-different-file-types image.png slides.pptx
 *
 * Output:
 * - Prints JSON responses for the two pdf conversions and the final merge.
 */

using Newtonsoft.Json.Linq;
using System.Text;

namespace Samples.ComplexFlowExamples
{
    public static class MergeDifferentFileTypes
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                Console.Error.WriteLine("merge-different-file-types requires <imageFile> <pptFile>");
                Environment.Exit(1);
                return;
            }

            var imagePath = args[0];
            var pptPath = args[1];
            if (!File.Exists(imagePath) || !File.Exists(pptPath))
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
            {
                // Begin first PDF conversion
                using var imageRequest = new HttpRequestMessage(HttpMethod.Post, "pdf");
                imageRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                imageRequest.Headers.Accept.Add(new("application/json"));
                var imageMultipartContent = new MultipartFormDataContent();

                var imageByteArray = File.ReadAllBytes(imagePath);
                var imageByteAryContent = new ByteArrayContent(imageByteArray);
                imageMultipartContent.Add(imageByteAryContent, "file", Path.GetFileName(imagePath));
                imageByteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");

                imageRequest.Content = imageMultipartContent;
                var imageResponse = await httpClient.SendAsync(imageRequest);

                var imageResult = await imageResponse.Content.ReadAsStringAsync();
                Console.WriteLine("Image to PDF response received.");
                Console.WriteLine(imageResult);

                dynamic imageResponseData = JObject.Parse(imageResult);
                string imageID = imageResponseData.outputId;

                // Begin second PDF conversion
                using var powerpointRequest = new HttpRequestMessage(HttpMethod.Post, "pdf");
                powerpointRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                powerpointRequest.Headers.Accept.Add(new("application/json"));
                var powerpointMultipartContent = new MultipartFormDataContent();

                var powerpointByteArray = File.ReadAllBytes(pptPath);
                var powerpointByteAryContent = new ByteArrayContent(powerpointByteArray);
                powerpointMultipartContent.Add(powerpointByteAryContent, "file", Path.GetFileName(pptPath));
                powerpointByteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");

                powerpointRequest.Content = powerpointMultipartContent;
                var powerpointResponse = await httpClient.SendAsync(powerpointRequest);

                var powerpointResult = await powerpointResponse.Content.ReadAsStringAsync();
                Console.WriteLine("powerpoint to PDF response received.");
                Console.WriteLine(powerpointResult);

                dynamic powerpointResponseData = JObject.Parse(powerpointResult);
                string powerpointID = powerpointResponseData.outputId;

                // Begin file merge
                using var request = new HttpRequestMessage(HttpMethod.Post, "merged-pdf");
                request.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                request.Headers.Accept.Add(new("application/json"));
                var multipartContent = new MultipartFormDataContent();

                var imageByteArrayID = new ByteArrayContent(Encoding.UTF8.GetBytes(imageID));
                multipartContent.Add(imageByteArrayID, "id[]");

                var byteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes("id"));
                multipartContent.Add(byteArrayOption, "type[]");
                var byteArrayOption2 = new ByteArrayContent(Encoding.UTF8.GetBytes("all"));
                multipartContent.Add(byteArrayOption2, "pages[]");

                var powerpointByteArrayID = new ByteArrayContent(Encoding.UTF8.GetBytes(powerpointID));
                multipartContent.Add(powerpointByteArrayID, "id[]");

                var byteArrayOption3 = new ByteArrayContent(Encoding.UTF8.GetBytes("id"));
                multipartContent.Add(byteArrayOption3, "type[]");
                var byteArrayOption4 = new ByteArrayContent(Encoding.UTF8.GetBytes("all"));
                multipartContent.Add(byteArrayOption4, "pages[]");

                request.Content = multipartContent;
                var response = await httpClient.SendAsync(request);
                var apiResult = await response.Content.ReadAsStringAsync();

                Console.WriteLine("Merge response received.");
                Console.WriteLine(apiResult);
            }
        }
    }
}
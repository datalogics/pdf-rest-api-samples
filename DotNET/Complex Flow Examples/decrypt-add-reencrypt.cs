/*
 * What this sample does:
 * - Decrypts a PDF, adds an image, then re-encrypts it.
 * - Routed from Program.cs as: `dotnet run -- decrypt-add-reencrypt <pdf> <image> [password]`.
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   dotnet run -- decrypt-add-reencrypt input.pdf image.png secret
 *
 * Output:
 * - Prints JSON responses for decrypt, add-image, and re-encrypt stages.
 */

using Newtonsoft.Json.Linq;
using System.Text;

namespace Samples.ComplexFlowExamples
{
    public static class DecryptAddReencrypt
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                Console.Error.WriteLine("decrypt-add-reencrypt requires <pdf> <image> [password]");
                Environment.Exit(1);
                return;
            }
            var pdfPath = args[0];
            var imgPath = args[1];
            var pwd = args.Length > 2 ? args[2] : "password";
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
            {
                // Decrypt
                using var decryptRequest = new HttpRequestMessage(HttpMethod.Post, "decrypted-pdf");
                decryptRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                decryptRequest.Headers.Accept.Add(new("application/json"));
                var decryptMultipartContent = new MultipartFormDataContent();
                var byteArray = File.ReadAllBytes(pdfPath);
                var byteAryContent = new ByteArrayContent(byteArray);
                decryptMultipartContent.Add(byteAryContent, "file", Path.GetFileName(pdfPath));
                byteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
                var byteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes(pwd));
                decryptMultipartContent.Add(byteArrayOption, "current_open_password");
                decryptRequest.Content = decryptMultipartContent;
                var decryptResponse = await httpClient.SendAsync(decryptRequest);
                var decryptResult = await decryptResponse.Content.ReadAsStringAsync();
                Console.WriteLine("Decrypt response received.");
                Console.WriteLine(decryptResult);
                dynamic decryptJson = JObject.Parse(decryptResult);
                string decryptID = decryptJson.outputId;

                // Add image
                using var addImageRequest = new HttpRequestMessage(HttpMethod.Post, "pdf-with-added-image");
                addImageRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                addImageRequest.Headers.Accept.Add(new("application/json"));
                var addImageMultipartContent = new MultipartFormDataContent();
                var addImageId = new ByteArrayContent(Encoding.UTF8.GetBytes(decryptID));
                addImageMultipartContent.Add(addImageId, "id");
                var addImageImage = File.ReadAllBytes(imgPath);
                var imageContent = new ByteArrayContent(addImageImage);
                addImageMultipartContent.Add(imageContent, "image_file", Path.GetFileName(imgPath));
                imageContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
                var addImagePage = new ByteArrayContent(Encoding.UTF8.GetBytes("1"));
                addImageMultipartContent.Add(addImagePage, "page");
                var addImageX = new ByteArrayContent(Encoding.UTF8.GetBytes("0"));
                addImageMultipartContent.Add(addImageX, "x");
                var addImageY = new ByteArrayContent(Encoding.UTF8.GetBytes("0"));
                addImageMultipartContent.Add(addImageY, "y");
                addImageRequest.Content = addImageMultipartContent;
                var addImageResponse = await httpClient.SendAsync(addImageRequest);
                var addImageResult = await addImageResponse.Content.ReadAsStringAsync();
                Console.WriteLine("Add image response received.");
                Console.WriteLine(addImageResult);
                dynamic addImageJson = JObject.Parse(addImageResult);
                string addImageID = addImageJson.outputId;

                // Re-encrypt
                using var encryptRequest = new HttpRequestMessage(HttpMethod.Post, "encrypted-pdf");
                encryptRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                encryptRequest.Headers.Accept.Add(new("application/json"));
                var multipartContent = new MultipartFormDataContent();
                var encryptID = new ByteArrayContent(Encoding.UTF8.GetBytes(addImageID));
                multipartContent.Add(encryptID, "id");
                var encryptPassword = new ByteArrayContent(Encoding.UTF8.GetBytes(pwd));
                multipartContent.Add(encryptPassword, "new_open_password");
                encryptRequest.Content = multipartContent;
                var response = await httpClient.SendAsync(encryptRequest);
                var apiResult = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Encrypt response received.");
                Console.WriteLine(apiResult);
            }
        }
    }
}


using Newtonsoft.Json.Linq;
using System.Text;

namespace Samples.EndpointExamples.JsonPayload
{
    public static class PdfWithAddedImage
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                Console.Error.WriteLine("pdf-with-added-image requires <pdfFile> <imageFile>");
                Environment.Exit(1);
                return;
            }

            var pdfFile = args[0];
            var imageFile = args[1];
            if (!File.Exists(pdfFile) || !File.Exists(imageFile)) { Console.Error.WriteLine("One or more files not found."); Environment.Exit(1); return; }
            var apiKey = Environment.GetEnvironmentVariable("PDFREST_API_KEY"); if (string.IsNullOrWhiteSpace(apiKey)) { Console.Error.WriteLine("Missing required environment variable: PDFREST_API_KEY"); Environment.Exit(1); return; }
            var baseUrl = Environment.GetEnvironmentVariable("PDFREST_URL") ?? "https://api.pdfrest.com";

            using (var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) })
            {
                // Upload PDF
                var pdfUploadRequest = new HttpRequestMessage(HttpMethod.Post, "upload");
                pdfUploadRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                pdfUploadRequest.Headers.Accept.Add(new("application/json"));
                var pdfBytes = File.ReadAllBytes(pdfFile);
                var pdfContent = new ByteArrayContent(pdfBytes);
                pdfContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
                pdfContent.Headers.TryAddWithoutValidation("Content-Filename", Path.GetFileName(pdfFile));
                pdfUploadRequest.Content = pdfContent;
                var pdfUploadResponse = await httpClient.SendAsync(pdfUploadRequest);
                var pdfUploadResult = await pdfUploadResponse.Content.ReadAsStringAsync();
                JObject pdfJson = JObject.Parse(pdfUploadResult);
                var pdfId = pdfJson["files"][0]["id"];

                // Upload Image
                var imgUploadRequest = new HttpRequestMessage(HttpMethod.Post, "upload");
                imgUploadRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                imgUploadRequest.Headers.Accept.Add(new("application/json"));
                var imgBytes = File.ReadAllBytes(imageFile);
                var imgContent = new ByteArrayContent(imgBytes);
                imgContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
                imgContent.Headers.TryAddWithoutValidation("Content-Filename", Path.GetFileName(imageFile));
                imgUploadRequest.Content = imgContent;
                var imgUploadResponse = await httpClient.SendAsync(imgUploadRequest);
                var imgUploadResult = await imgUploadResponse.Content.ReadAsStringAsync();
                JObject imgJson = JObject.Parse(imgUploadResult);
                var imgId = imgJson["files"][0]["id"];

                using (var attachRequest = new HttpRequestMessage(HttpMethod.Post, "pdf-with-added-image"))
                {
                    attachRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                    attachRequest.Headers.Accept.Add(new("application/json"));
                    attachRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");
                    JObject parameterJson = new JObject { ["id"] = pdfId, ["image_id"] = imgId, ["page"] = 1, ["x"] = 0, ["y"] = 0 };
                    attachRequest.Content = new StringContent(parameterJson.ToString(), Encoding.UTF8, "application/json");
                    var attachResponse = await httpClient.SendAsync(attachRequest);
                    var attachResult = await attachResponse.Content.ReadAsStringAsync();
                    Console.WriteLine("Processing response received.");
                    Console.WriteLine(attachResult);
                }
            }
        }
    }
}

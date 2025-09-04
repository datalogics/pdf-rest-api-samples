
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Samples.EndpointExamples.JsonPayload
{
    public static class PdfWithAddedText
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 1) { Console.Error.WriteLine("pdf-with-added-text requires <inputFile>"); Environment.Exit(1); return; }
            var inputPath = args[0]; if (!File.Exists(inputPath)) { Console.Error.WriteLine($"File not found: {inputPath}"); Environment.Exit(1); return; }
            var apiKey = Environment.GetEnvironmentVariable("PDFREST_API_KEY"); if (string.IsNullOrWhiteSpace(apiKey)) { Console.Error.WriteLine("Missing required environment variable: PDFREST_API_KEY"); Environment.Exit(1); return; }
            var baseUrl = Environment.GetEnvironmentVariable("PDFREST_URL") ?? "https://api.pdfrest.com";
            using (var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) })
            using (var uploadRequest = new HttpRequestMessage(HttpMethod.Post, "upload"))
            {
                uploadRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                uploadRequest.Headers.Accept.Add(new("application/json"));
                var uploadByteArray = File.ReadAllBytes(inputPath);
                var uploadByteAryContent = new ByteArrayContent(uploadByteArray);
                uploadByteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
                uploadByteAryContent.Headers.TryAddWithoutValidation("Content-Filename", Path.GetFileName(inputPath));
                uploadRequest.Content = uploadByteAryContent;
                var uploadResponse = await httpClient.SendAsync(uploadRequest);
                var uploadResult = await uploadResponse.Content.ReadAsStringAsync();
                Console.WriteLine("Upload response received.");
                Console.WriteLine(uploadResult);

                JObject uploadResultJson = JObject.Parse(uploadResult);
                var uploadedID = uploadResultJson["files"][0]["id"];
                using (var addedTextRequest = new HttpRequestMessage(HttpMethod.Post, "pdf-with-added-text"))
                {
                    addedTextRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                    addedTextRequest.Headers.Accept.Add(new("application/json"));
                    addedTextRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");

                    var text_option_array = new JArray();
                    var text_options = new JObject
                    {
                        ["font"] = "Times New Roman",
                        ["max_width"] = "175",
                        ["opacity"] = "1",
                        ["page"] = "1",
                        ["rotation"] = "0",
                        ["text"] = "sample text in PDF",
                        ["text_color_rgb"] = "0,0,0",
                        ["text_size"] = "30",
                        ["x"] = "72",
                        ["y"] = "144"
                    };
                    text_option_array.Add(text_options);

                    JObject parameterJson = new JObject { ["id"] = uploadedID, ["text_objects"] = JsonConvert.SerializeObject(text_option_array) };
                    addedTextRequest.Content = new StringContent(parameterJson.ToString(), Encoding.UTF8, "application/json");
                    var addedTextResponse = await httpClient.SendAsync(addedTextRequest);
                    var addedTextResult = await addedTextResponse.Content.ReadAsStringAsync();
                    Console.WriteLine("Processing response received.");
                    Console.WriteLine(addedTextResult);
                }
            }
        }
    }
}

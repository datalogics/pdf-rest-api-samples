
using Newtonsoft.Json.Linq;
using System.Text;

namespace Samples.EndpointExamples.JsonPayload
{
    public static class Pdf
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Console.Error.WriteLine("pdf requires <inputFile>");
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
                    using (var pdfRequest = new HttpRequestMessage(HttpMethod.Post, "pdf"))
                    {
                        pdfRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                        pdfRequest.Headers.Accept.Add(new("application/json"));

                        pdfRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");


                        JObject parameterJson = new JObject
                        {
                            ["id"] = uploadedID,
                        };

                        pdfRequest.Content = new StringContent(parameterJson.ToString(), Encoding.UTF8, "application/json"); ;
                        var pdfResponse = await httpClient.SendAsync(pdfRequest);

                        var pdfResult = await pdfResponse.Content.ReadAsStringAsync();

                        Console.WriteLine("Processing response received.");
                        Console.WriteLine(pdfResult);
                    }
                }
            }
        }
    }
}

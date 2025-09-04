
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Samples.EndpointExamples.JsonPayload
{
    public static class SignedPdfNonPfx
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 3)
            {
                Console.Error.WriteLine("signed-pdf-non-pfx requires <input.pdf> <certificate.pem> <private_key.pem>");
                Environment.Exit(1);
                return;
            }

            var apiKey = Environment.GetEnvironmentVariable("PDFREST_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey)) { Console.Error.WriteLine("Missing required environment variable: PDFREST_API_KEY"); Environment.Exit(1); return; }
            var baseUrl = Environment.GetEnvironmentVariable("PDFREST_URL") ?? "https://api.pdfrest.com";
            var files = args.Take(3).ToArray();
            foreach (var f in files) { if (!File.Exists(f)) { Console.Error.WriteLine($"File not found: {f}"); Environment.Exit(1); return; } }

            using var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
            var ids = new string[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                var uploadRequest = new HttpRequestMessage(HttpMethod.Post, "upload");
                uploadRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                uploadRequest.Headers.Accept.Add(new("application/json"));
                var bytes = File.ReadAllBytes(files[i]);
                var content = new ByteArrayContent(bytes);
                content.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
                content.Headers.TryAddWithoutValidation("Content-Filename", Path.GetFileName(files[i]));
                uploadRequest.Content = content;
                var uploadResponse = await httpClient.SendAsync(uploadRequest);
                var uploadResult = await uploadResponse.Content.ReadAsStringAsync();
                JObject uploadJson = JObject.Parse(uploadResult);
                ids[i] = uploadJson["files"][0]["id"].ToString();
            }

            var signedPdfRequest = new HttpRequestMessage(HttpMethod.Post, "signed-pdf");
            signedPdfRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
            signedPdfRequest.Headers.Accept.Add(new("application/json"));
            signedPdfRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");

            var signatureConfiguration = new JObject
            {
                ["type"] = "new",
                ["name"] = "esignature",
                ["location"] = new JObject
                {
                    ["bottom_left"] = new JObject { ["x"] = "0", ["y"] = "0" },
                    ["top_right"] = new JObject { ["x"] = "216", ["y"] = "72" },
                    ["page"] = "1"
                },
                ["display"] = new JObject { ["include_datetime"] = "true" }
            };

            JObject parameterJson = new JObject
            {
                ["id"] = ids[0],
                ["certificate_id"] = ids[1],
                ["private_key_id"] = ids[2],
                ["signature_configuration"] = signatureConfiguration.ToString(Formatting.None),
            };
            signedPdfRequest.Content = new StringContent(parameterJson.ToString(), Encoding.UTF8, "application/json");
            var signedPdfResponse = await httpClient.SendAsync(signedPdfRequest);
            var signedPdfResult = await signedPdfResponse.Content.ReadAsStringAsync();
            Console.WriteLine("Processing response received.");
            Console.WriteLine(signedPdfResult);
        }
    }
}

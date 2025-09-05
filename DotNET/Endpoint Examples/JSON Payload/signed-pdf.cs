
/*
 * What this sample does:
 * - Called from Program.cs to digitally sign a PDF using a PFX, passphrase, and optional logo via JSON flow (uploads all inputs).
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   dotnet run -- signed-pdf input.pdf creds.pfx passphrase.txt logo.png
 *
 * Output:
 * - Prints JSON responses; non-2xx results exit non-zero.
 */
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Samples.EndpointExamples.JsonPayload
{
    public static class SignedPdf
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 4)
            {
                Console.Error.WriteLine("signed-pdf requires <input.pdf> <credentials.pfx> <passphrase.txt> <logo.png>");
                Environment.Exit(1);
                return;
            }

            var apiKey = Environment.GetEnvironmentVariable("PDFREST_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey)) { Console.Error.WriteLine("Missing required environment variable: PDFREST_API_KEY"); Environment.Exit(1); return; }
            var baseUrl = Environment.GetEnvironmentVariable("PDFREST_URL") ?? "https://api.pdfrest.com";
            var files = args.Take(4).ToArray();
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
                ["logo_opacity"] = "0.5",
                ["location"] = new JObject
                {
                    ["bottom_left"] = new JObject { ["x"] = "0", ["y"] = "0" },
                    ["top_right"] = new JObject { ["x"] = "216", ["y"] = "72" },
                    ["page"] = "1"
                },
                ["display"] = new JObject
                {
                    ["include_distinguished_name"] = "true",
                    ["include_datetime"] = "true",
                    ["contact"] = "My contact information",
                    ["location"] = "My signing location",
                    ["name"] = "John Doe",
                    ["reason"] = "My reason for signing"
                }
            };

            JObject parameterJson = new JObject
            {
                ["id"] = ids[0],
                ["pfx_credential_id"] = ids[1],
                ["pfx_passphrase_id"] = ids[2],
                ["logo_id"] = ids[3],
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

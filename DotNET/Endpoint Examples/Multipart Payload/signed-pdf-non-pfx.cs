/*
 * What this sample does:
 * - Signs a PDF via multipart/form-data using PEM certificate and key.
 * - Routed from Program.cs as: `dotnet run -- signed-pdf-non-pfx-multipart <pdf> <cert> <key>`.
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   dotnet run -- signed-pdf-non-pfx-multipart input.pdf cert.pem key.pem
 *
 * Output:
 * - Prints the JSON response. Validation errors (args/env) exit non-zero.
 */

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Samples.EndpointExamples.MultipartPayload
{
    public static class SignedPdfNonPfx
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 3)
            {
                Console.Error.WriteLine("signed-pdf-non-pfx-multipart requires <pdf> <cert> <key>");
                Environment.Exit(1);
                return;
            }
            var pdfPath = args[0];
            var certPath = args[1];
            var keyPath = args[2];
            if (!File.Exists(pdfPath) || !File.Exists(certPath) || !File.Exists(keyPath))
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
            using (var request = new HttpRequestMessage(HttpMethod.Post, "signed-pdf"))
            {
                request.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                request.Headers.Accept.Add(new("application/json"));
                var multipartContent = new MultipartFormDataContent();

                var inputByteArray = File.ReadAllBytes(pdfPath);
                var inputByteArrayContent = new ByteArrayContent(inputByteArray);
                multipartContent.Add(inputByteArrayContent, "file", Path.GetFileName(pdfPath));
                inputByteArrayContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");

                var certByteArray = File.ReadAllBytes(certPath);
                var certByteArrayContent = new ByteArrayContent(certByteArray);
                multipartContent.Add(certByteArrayContent, "certificate_file", Path.GetFileName(certPath));
                certByteArrayContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");

                var privateKeyByteArray = File.ReadAllBytes(keyPath);
                var privateKeyByteArrayContent = new ByteArrayContent(privateKeyByteArray);
                multipartContent.Add(privateKeyByteArrayContent, "private_key_file", Path.GetFileName(keyPath));
                privateKeyByteArrayContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");

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

                var byteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes(signatureConfiguration.ToString(Formatting.None)));
                multipartContent.Add(byteArrayOption, "signature_configuration");

                request.Content = multipartContent;
                var response = await httpClient.SendAsync(request);
                var apiResult = await response.Content.ReadAsStringAsync();

                Console.WriteLine("API response received.");
                Console.WriteLine(apiResult);
            }
        }
    }
}

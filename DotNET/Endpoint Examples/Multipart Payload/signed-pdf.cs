/*
 * What this sample does:
 * - Signs a PDF via multipart/form-data using a PFX credential and optional logo.
 * - Routed from Program.cs as: `dotnet run -- signed-pdf-multipart <pdf> <pfx> <passfile> <logo>`.
 */

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Samples.EndpointExamples.MultipartPayload
{
    public static class SignedPdf
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 4)
            {
                Console.Error.WriteLine("signed-pdf-multipart requires <pdf> <pfx> <passfile> <logo>");
                Environment.Exit(1);
                return;
            }
            var pdfPath = args[0];
            var pfxPath = args[1];
            var passPath = args[2];
            var logoPath = args[3];
            if (!File.Exists(pdfPath) || !File.Exists(pfxPath) || !File.Exists(passPath) || !File.Exists(logoPath))
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

                var credByteArray = File.ReadAllBytes(pfxPath);
                var credByteArrayContent = new ByteArrayContent(credByteArray);
                multipartContent.Add(credByteArrayContent, "pfx_credential_file", Path.GetFileName(pfxPath));
                credByteArrayContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");

                var passphraseByteArray = File.ReadAllBytes(passPath);
                var passphraseByteArrayContent = new ByteArrayContent(passphraseByteArray);
                multipartContent.Add(passphraseByteArrayContent, "pfx_passphrase_file", Path.GetFileName(passPath));
                passphraseByteArrayContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");

                var logoByteArray = File.ReadAllBytes(logoPath);
                var logoByteArrayContent = new ByteArrayContent(logoByteArray);
                multipartContent.Add(logoByteArrayContent, "logo_file", Path.GetFileName(logoPath));
                logoByteArrayContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");

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

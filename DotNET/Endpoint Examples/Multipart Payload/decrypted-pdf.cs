/*
 * What this sample does:
 * - Removes an open password from a PDF via multipart/form-data.
 * - Routed from Program.cs as: `dotnet run -- decrypted-pdf-multipart <inputFile> <password>`.
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   dotnet run -- decrypted-pdf-multipart /path/to/input.pdf secret
 *
 * Output:
 * - Prints the JSON response. Validation errors (args/env) exit non-zero.
 */

using System.Text;

namespace Samples.EndpointExamples.MultipartPayload
{
    public static class DecryptedPdf
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                Console.Error.WriteLine("decrypted-pdf-multipart requires <inputFile> <password>");
                Environment.Exit(1);
                return;
            }
            var inputPath = args[0];
            var password = args[1];
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
            // Optional immediate deletion of sensitive files
            // Default: false; override with PDFREST_DELETE_SENSITIVE_FILES=true
            var deleteSensitiveFiles = string.Equals(
                Environment.GetEnvironmentVariable("PDFREST_DELETE_SENSITIVE_FILES"),
                "true",
                StringComparison.OrdinalIgnoreCase);

            using (var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) })
            using (var request = new HttpRequestMessage(HttpMethod.Post, "decrypted-pdf"))
            {
                request.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                request.Headers.Accept.Add(new("application/json"));
                var multipartContent = new MultipartFormDataContent();

                var byteArray = File.ReadAllBytes(inputPath);
                var byteAryContent = new ByteArrayContent(byteArray);
                multipartContent.Add(byteAryContent, "file", Path.GetFileName(inputPath));
                byteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");

                var byteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes(password));
                multipartContent.Add(byteArrayOption, "current_open_password");

                request.Content = multipartContent;
                var response = await httpClient.SendAsync(request);
                var apiResult = await response.Content.ReadAsStringAsync();

                Console.WriteLine("API response received.");
                Console.WriteLine(apiResult);

                // All files uploaded or generated are automatically deleted based on the
                // File Retention Period as shown on https://pdfrest.com/pricing.
                // For immediate deletion of files, particularly when sensitive data
                // is involved, an explicit delete call can be made to the API.
                //
                // The following code is an optional step to delete sensitive files
                // (unredacted, unencrypted, unrestricted, or unwatermarked) from pdfRest servers.
                if (deleteSensitiveFiles)
                {
                    using (var deleteRequest = new HttpRequestMessage(HttpMethod.Post, "delete"))
                    {
                        deleteRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                        deleteRequest.Headers.Accept.Add(new("application/json"));
                        deleteRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");

                        var outId = Newtonsoft.Json.Linq.JObject.Parse(apiResult)["outputId"]!.ToString();
                        var deleteJson = new Newtonsoft.Json.Linq.JObject { ["ids"] = outId };
                        deleteRequest.Content = new StringContent(deleteJson.ToString(), Encoding.UTF8, "application/json");
                        var deleteResponse = await httpClient.SendAsync(deleteRequest);
                        var deleteResult = await deleteResponse.Content.ReadAsStringAsync();
                        Console.WriteLine(deleteResult);
                    }
                }
            }
        }
    }
}

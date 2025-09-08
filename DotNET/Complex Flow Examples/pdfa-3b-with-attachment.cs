/*
 * What this sample does:
 * - Attaches an XML to a PDF, then converts to PDF/A-3b.
 * - Routed from Program.cs as: `dotnet run -- pdfa-3b-with-attachment <pdf> <xml>`.
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   dotnet run -- pdfa-3b-with-attachment input.pdf data.xml
 *
 * Output:
 * - Prints JSON responses for attachment and PDF/A conversion.
 */

using Newtonsoft.Json.Linq;
using System.Text;

namespace Samples.ComplexFlowExamples
{
    public static class Pdfa3bWithAttachment
    {
        public static async Task Execute(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                Console.Error.WriteLine("pdfa-3b-with-attachment requires <pdf> <xml>");
                Environment.Exit(1);
                return;
            }
            var pdfPath = args[0];
            var xmlPath = args[1];
            if (!File.Exists(pdfPath) || !File.Exists(xmlPath))
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
                using var attachRequest = new HttpRequestMessage(HttpMethod.Post, "pdf-with-added-attachment");
                attachRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                attachRequest.Headers.Accept.Add(new("application/json"));
                var attachMultipartContent = new MultipartFormDataContent();
                var byteArray = File.ReadAllBytes(pdfPath);
                var byteAryContent = new ByteArrayContent(byteArray);
                attachMultipartContent.Add(byteAryContent, "file", Path.GetFileName(pdfPath));
                byteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
                var byteArray2 = File.ReadAllBytes(xmlPath);
                var byteAryContent2 = new ByteArrayContent(byteArray2);
                attachMultipartContent.Add(byteAryContent2, "file_to_attach", Path.GetFileName(xmlPath));
                byteAryContent2.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
                attachRequest.Content = attachMultipartContent;
                var attachResponse = await httpClient.SendAsync(attachRequest);
                var attachResult = await attachResponse.Content.ReadAsStringAsync();
                Console.WriteLine("Attachement response received.");
                Console.WriteLine(attachResult);
                dynamic responseData = JObject.Parse(attachResult);
                string attachementID = responseData.outputId;

                using var pdfaRequest = new HttpRequestMessage(HttpMethod.Post, "pdfa");
                pdfaRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                pdfaRequest.Headers.Accept.Add(new("application/json"));
                var multipartContent = new MultipartFormDataContent();
                var byteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes(attachementID));
                multipartContent.Add(byteArrayOption, "id");
                var byteArrayOption2 = new ByteArrayContent(Encoding.UTF8.GetBytes("PDF/A-3b"));
                multipartContent.Add(byteArrayOption2, "output_type");
                pdfaRequest.Content = multipartContent;
                var response = await httpClient.SendAsync(pdfaRequest);
                var apiResult = await response.Content.ReadAsStringAsync();
                Console.WriteLine("PDF/A response received.");
                Console.WriteLine(apiResult);
            }
        }
    }
}

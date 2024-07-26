using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

class Program
{
    private static readonly string apiKey = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"; // Your API key here

    static async Task Main(string[] args)
    {
        using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com") })
        {
            // Upload PDF for OCR
            using var ocrRequest = new HttpRequestMessage(HttpMethod.Post, "pdf-with-ocr-text");

            ocrRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
            ocrRequest.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var ocrMultipartContent = new MultipartFormDataContent();

            var pdfByteArray = File.ReadAllBytes("/path/to/file.pdf");
            var pdfByteArrayContent = new ByteArrayContent(pdfByteArray);
            ocrMultipartContent.Add(pdfByteArrayContent, "file", "file.pdf");
            pdfByteArrayContent.Headers.TryAddWithoutValidation("Content-Type", "application/pdf");
            ocrMultipartContent.Add(new StringContent("example_pdf-with-ocr-text_out"), "output");

            ocrRequest.Content = ocrMultipartContent;
            var ocrResponse = await httpClient.SendAsync(ocrRequest);

            var ocrResult = await ocrResponse.Content.ReadAsStringAsync();
            Console.WriteLine("OCR response received.");
            Console.WriteLine(ocrResult);

            dynamic ocrResponseData = JObject.Parse(ocrResult);
            string ocrPDFID = ocrResponseData.outputId;

            // Extract text from OCR'd PDF
            using var extractTextRequest = new HttpRequestMessage(HttpMethod.Post, "extracted-text");

            extractTextRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
            extractTextRequest.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var extractTextMultipartContent = new MultipartFormDataContent();

            extractTextMultipartContent.Add(new StringContent(ocrPDFID), "id");

            extractTextRequest.Content = extractTextMultipartContent;
            var extractTextResponse = await httpClient.SendAsync(extractTextRequest);

            var extractTextResult = await extractTextResponse.Content.ReadAsStringAsync();
            Console.WriteLine("Extract text response received.");
            Console.WriteLine(extractTextResult);

            dynamic extractTextResponseData = JObject.Parse(extractTextResult);
            string fullText = extractTextResponseData.fullText;

            Console.WriteLine("Extracted text:");
            Console.WriteLine(fullText);
        }
    }
}
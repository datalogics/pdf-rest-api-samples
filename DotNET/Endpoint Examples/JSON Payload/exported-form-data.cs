
/*
 * What this sample does:
 * - Top-level example: upload a PDF, then export form data via JSON flow.
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   See Program.cs-wrapped command in this DotNET folder; this file is not compiled by default.
 *
 * Output:
 * - Prints JSON responses if executed; non-2xx results exit non-zero.
 */
using Newtonsoft.Json.Linq;
using System.Text;

using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com") })
{
    using (var uploadRequest = new HttpRequestMessage(HttpMethod.Post, "upload"))
    {
        uploadRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
        uploadRequest.Headers.Accept.Add(new("application/json"));

        var uploadByteArray = File.ReadAllBytes("/path/to/file");
        var uploadByteAryContent = new ByteArrayContent(uploadByteArray);
        uploadByteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
        uploadByteAryContent.Headers.TryAddWithoutValidation("Content-Filename", "filename.pdf");


        uploadRequest.Content = uploadByteAryContent;
        var uploadResponse = await httpClient.SendAsync(uploadRequest);

        var uploadResult = await uploadResponse.Content.ReadAsStringAsync();

        Console.WriteLine("Upload response received.");
        Console.WriteLine(uploadResult);

        JObject uploadResultJson = JObject.Parse(uploadResult);
        var uploadedID = uploadResultJson["files"][0]["id"];
        using (var exportRequest = new HttpRequestMessage(HttpMethod.Post, "exported-form-data"))
        {
            exportRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
            exportRequest.Headers.Accept.Add(new("application/json"));

            exportRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");


            JObject parameterJson = new JObject
            {
                ["id"] = uploadedID,
                ["data_format"] = "xml",
            };

            exportRequest.Content = new StringContent(parameterJson.ToString(), Encoding.UTF8, "application/json"); ;
            var exportResponse = await httpClient.SendAsync(exportRequest);

            var exportResult = await exportResponse.Content.ReadAsStringAsync();

            Console.WriteLine("Processing response received.");
            Console.WriteLine(exportResult);
        }
    }
}

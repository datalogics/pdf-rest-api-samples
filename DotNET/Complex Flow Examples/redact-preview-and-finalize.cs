using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

/*
* This sample demonstrates the workflow from unredacted document to fully
* redacted document. The output file from the preview tool is immediately
* forwarded to the finalization stage. We recommend inspecting the output from
* the preview stage before utilizing this workflow to ensure that content is
* redacted as intended.
*/

var apiKey = "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"; // Your API key here

using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com") })
{

    // Begin redaction preview
    using var previewRequest = new HttpRequestMessage(HttpMethod.Post, "pdf-with-redacted-text-preview");

    previewRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
    previewRequest.Headers.Accept.Add(new("application/json"));
    var previewMultipartContent = new MultipartFormDataContent();

    var byteArray = File.ReadAllBytes("/path/to/file.pdf");
    var byteAryContent = new ByteArrayContent(byteArray);
    previewMultipartContent.Add(byteAryContent, "file", "file_name.pdf");
    var redactionArray = new JArray();
    var redaction = new JObject
    {
        ["type"] = "regex",
        ["value"] = "[Tt]he"
    };
    redactionArray.Add(redaction);
    var byteArrayRedOption = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(redactionArray)));
    previewMultipartContent.Add(byteArrayRedOption, "redactions");
    byteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/pdf");

    previewRequest.Content = previewMultipartContent;
    var pdfResponse = await httpClient.SendAsync(previewRequest);

    var pdfResult = await pdfResponse.Content.ReadAsStringAsync();
    Console.WriteLine("Redaction preview response received.");
    Console.WriteLine(pdfResult);

    dynamic responseData = JObject.Parse(pdfResult);
    string pdfID = responseData.outputId;

    // Apply the previewed redactions
    using var finalizeRequest = new HttpRequestMessage(HttpMethod.Post, "pdf-with-redacted-text-applied");

    finalizeRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
    finalizeRequest.Headers.Accept.Add(new("application/json"));
    var finalMultipartContent = new MultipartFormDataContent();


    var byteArrayIdOption = new ByteArrayContent(Encoding.UTF8.GetBytes(pdfID));
    finalMultipartContent.Add(byteArrayIdOption, "id");

    finalizeRequest.Content = finalMultipartContent;
    var response = await httpClient.SendAsync(finalizeRequest);

    var apiResult = await response.Content.ReadAsStringAsync();

    Console.WriteLine("Finalized redaction response received.");
    Console.WriteLine(apiResult);

}

using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;

/* In this sample, we will show how to watermark a PDF document and then restrict
* editing on the document so that the watermark cannot be removed, as discussed in
* https://pdfrest.com/solutions/add-pdf-watermarks-that-cannot-be-removed/.
* We will be running the input file through /watermarked-pdf to apply the watermark
* and then /restricted-pdf to lock the watermark in.
*/

var apiKey = "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"; // Your API key here

using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com") })
{
    // Begin watermarking
    using var watermarkRequest = new HttpRequestMessage(HttpMethod.Post, "watermarked-pdf");

    watermarkRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
    watermarkRequest.Headers.Accept.Add(new("application/json"));
    var watermarkMultipartContent = new MultipartFormDataContent();

    var watermarkByteArray = File.ReadAllBytes("/path/to/file.pdf");
    var watermarkByteAryContent = new ByteArrayContent(watermarkByteArray);
    watermarkMultipartContent.Add(watermarkByteAryContent, "file", "file.pdf");
    watermarkByteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/pdf");

    var watermarkByteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes("Watermarked"));
    watermarkMultipartContent.Add(watermarkByteArrayOption, "watermark_text");

    watermarkRequest.Content = watermarkMultipartContent;
    var watermarkResponse = await httpClient.SendAsync(watermarkRequest);

    var watermarkResult = await watermarkResponse.Content.ReadAsStringAsync();
    Console.WriteLine("Watermark to PDF response received.");
    Console.WriteLine(watermarkResult);

    dynamic watermarkResponseData = JObject.Parse(watermarkResult);
    string watermarkID = watermarkResponseData.outputId;

    // Begin restricting
    using var restrictRequest = new HttpRequestMessage(HttpMethod.Post, "restricted-pdf");

    restrictRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
    restrictRequest.Headers.Accept.Add(new("application/json"));
    var restrictMultipartContent = new MultipartFormDataContent();

    var watermarkIDByteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes(watermarkID));
    restrictMultipartContent.Add(watermarkIDByteArrayOption, "id");

    var byteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes("password"));
    restrictMultipartContent.Add(byteArrayOption, "new_permissions_password");

    var byteArrayOption2 = new ByteArrayContent(Encoding.UTF8.GetBytes("copy_content"));
    restrictMultipartContent.Add(byteArrayOption2, "restrictions[]");
    var byteArrayOption3 = new ByteArrayContent(Encoding.UTF8.GetBytes("edit_content"));
    restrictMultipartContent.Add(byteArrayOption3, "restrictions[]");
    var byteArrayOption4 = new ByteArrayContent(Encoding.UTF8.GetBytes("edit_annotations"));
    restrictMultipartContent.Add(byteArrayOption3, "restrictions[]");


    restrictRequest.Content = restrictMultipartContent;
    var restrictResponse = await httpClient.SendAsync(restrictRequest);

    var apiResult = await restrictResponse.Content.ReadAsStringAsync();

    Console.WriteLine("Restrict response received.");
    Console.WriteLine(apiResult);
}

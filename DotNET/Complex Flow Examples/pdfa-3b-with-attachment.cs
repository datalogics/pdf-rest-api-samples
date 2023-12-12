using Newtonsoft.Json.Linq;
using System.Text;

/* In this sample, we will show how to attach an xml document to a PDF file and then
* convert the file with the attachment to conform to the PDF/A standard, which
* can be useful for invoicing and standards compliance. We will be running the
* input document through /pdf-with-added-attachment to add the attachment and
* then /pdfa to do the PDF/A conversion.

* Note that there is nothing special about attaching an xml file, and any appropriate
* file may be attached and wrapped into the PDF/A conversion.
*/

var apiKey = "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"; // Your API key here

using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com") })
{
    using (var attachRequest = new HttpRequestMessage(HttpMethod.Post, "pdf-with-added-attachment"))
    {
        attachRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
        attachRequest.Headers.Accept.Add(new("application/json"));
        var attachMultipartContent = new MultipartFormDataContent();

        var byteArray = File.ReadAllBytes("/path/to/file.pdf");
        var byteAryContent = new ByteArrayContent(byteArray);
        attachMultipartContent.Add(byteAryContent, "file", "file_name.pdf");
        byteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/pdf");

        var byteArray2 = File.ReadAllBytes("/path/to/file.xml");
        var byteAryContent2 = new ByteArrayContent(byteArray2);
        attachMultipartContent.Add(byteAryContent2, "file_to_attach", "file_name.xml");
        byteAryContent2.Headers.TryAddWithoutValidation("Content-Type", "application/xml");

        attachRequest.Content = attachMultipartContent;
        var attachResponse = await httpClient.SendAsync(attachRequest);

        var attachResult = await attachResponse.Content.ReadAsStringAsync();
        Console.WriteLine("Attachement response received.");
        Console.WriteLine(attachResult);

        dynamic responseData = JObject.Parse(attachResult);
        string attachementID = responseData.outputId;

        using (var request = new HttpRequestMessage(HttpMethod.Post, "pdfa"))
        {
            request.Headers.TryAddWithoutValidation("Api-Key", apiKey);
            request.Headers.Accept.Add(new("application/json"));
            var multipartContent = new MultipartFormDataContent();


            var byteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes(attachementID));
            multipartContent.Add(byteArrayOption, "id");

            var byteArrayOption2 = new ByteArrayContent(Encoding.UTF8.GetBytes("PDF/A-3b"));
            multipartContent.Add(byteArrayOption2, "output_type");


            request.Content = multipartContent;
            var response = await httpClient.SendAsync(request);

            var apiResult = await response.Content.ReadAsStringAsync();

            Console.WriteLine("PDF/A response received.");
            Console.WriteLine(apiResult);
        }
    }
}

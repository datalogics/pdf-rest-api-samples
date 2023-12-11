using Newtonsoft.Json.Linq;
using System.Text;

/* In this sample, we will show how to optimize a Word file for long-term preservation
* as discussed in https://pdfrest.com/solutions/optimize-word-excel-and-powerpoint-files-for-long-term-preservation/
* We will take our Word (or Excel or PowerPoint) document and first convert it to
* a PDF with a call to the /pdf route. Then, we will take that converted PDF
* and convert it to the PDF/A format for long-term storage.
*/

using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com") })
{
    using (var pdfRequest = new HttpRequestMessage(HttpMethod.Post, "pdf"))
    {
        pdfRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"); // Your API key here
        pdfRequest.Headers.Accept.Add(new("application/json"));
        var pdfMultipartContent = new MultipartFormDataContent();

        var byteArray = File.ReadAllBytes("/path/to/file.doc");
        var byteAryContent = new ByteArrayContent(byteArray);
        pdfMultipartContent.Add(byteAryContent, "file", "file_name.doc");
        byteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/msword");

        pdfRequest.Content = pdfMultipartContent;
        var pdfResponse = await httpClient.SendAsync(pdfRequest);

        var pdfResult = await pdfResponse.Content.ReadAsStringAsync();
        Console.WriteLine("PDF response received.");
        Console.WriteLine(pdfResult);

        dynamic responseData = JObject.Parse(pdfResult);
        string pdfID = responseData.outputId;

        using (var request = new HttpRequestMessage(HttpMethod.Post, "pdfa"))
        {
            request.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"); // Your API key here
            request.Headers.Accept.Add(new("application/json"));
            var multipartContent = new MultipartFormDataContent();


            var byteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes(pdfID));
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

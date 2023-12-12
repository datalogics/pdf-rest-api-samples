using Newtonsoft.Json.Linq;
using System.Text;

/* In this sample, we will show how to merge different file types together as
* discussed in https://pdfrest.com/solutions/merge-multiple-types-of-files-together/.
First, we will upload an image file to the /pdf route and capture the output ID.
* Next, we will upload a PowerPoint file to the /pdf route and capture its output
* ID. Finally, we will pass both IDs to the /merged-pdf route to combine both inputs
* into a single PDF.
*
* Note that there is nothing special about an image and a PowerPoint file, and
* this sample could be easily used to convert and combine any two file types
* that the /pdf route takes as inputs.
*/

var apiKey = "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"; // Your API key here

using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com") })
{
    using (var imageRequest = new HttpRequestMessage(HttpMethod.Post, "pdf"))
    {
        imageRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
        imageRequest.Headers.Accept.Add(new("application/json"));
        var imageMultipartContent = new MultipartFormDataContent();

        var imageByteArray = File.ReadAllBytes("/path/to/file.png");
        var imageByteAryContent = new ByteArrayContent(imageByteArray);
        imageMultipartContent.Add(imageByteAryContent, "file", "file.png");
        imageByteAryContent.Headers.TryAddWithoutValidation("Content-Type", "image/x-png");

        imageRequest.Content = imageMultipartContent;
        var imageResponse = await httpClient.SendAsync(imageRequest);

        var imageResult = await imageResponse.Content.ReadAsStringAsync();
        Console.WriteLine("Image to PDF response received.");
        Console.WriteLine(imageResult);

        dynamic imageResponseData = JObject.Parse(imageResult);
        string imageID = imageResponseData.outputId;

        using (var powerpointRequest = new HttpRequestMessage(HttpMethod.Post, "pdf"))
        {
            powerpointRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey);
            powerpointRequest.Headers.Accept.Add(new("application/json"));
            var powerpointMultipartContent = new MultipartFormDataContent();

            var powerpointByteArray = File.ReadAllBytes("/path/to/file.ppt");
            var powerpointByteAryContent = new ByteArrayContent(powerpointByteArray);
            powerpointMultipartContent.Add(powerpointByteAryContent, "file", "file.ppt");
            powerpointByteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/vnd.ms-powerpoint");

            powerpointRequest.Content = powerpointMultipartContent;
            var powerpointResponse = await httpClient.SendAsync(powerpointRequest);

            var powerpointResult = await powerpointResponse.Content.ReadAsStringAsync();
            Console.WriteLine("powerpoint to PDF response received.");
            Console.WriteLine(powerpointResult);

            dynamic powerpointResponseData = JObject.Parse(powerpointResult);
            string powerpointID = powerpointResponseData.outputId;

            using (var request = new HttpRequestMessage(HttpMethod.Post, "merged-pdf"))
            {
                request.Headers.TryAddWithoutValidation("Api-Key", apiKey);
                request.Headers.Accept.Add(new("application/json"));
                var multipartContent = new MultipartFormDataContent();


                var imageByteArrayID = new ByteArrayContent(Encoding.UTF8.GetBytes(imageID));
                multipartContent.Add(imageByteArrayID, "id[]");

                var byteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes("id"));
                multipartContent.Add(byteArrayOption, "type[]");
                var byteArrayOption2 = new ByteArrayContent(Encoding.UTF8.GetBytes("all"));
                multipartContent.Add(byteArrayOption2, "pages[]");


                var powerpointByteArrayID = new ByteArrayContent(Encoding.UTF8.GetBytes(powerpointID));
                multipartContent.Add(powerpointByteArrayID, "id[]");

                var byteArrayOption3 = new ByteArrayContent(Encoding.UTF8.GetBytes("id"));
                multipartContent.Add(byteArrayOption3, "type[]");
                var byteArrayOption4 = new ByteArrayContent(Encoding.UTF8.GetBytes("all"));
                multipartContent.Add(byteArrayOption4, "pages[]");

                request.Content = multipartContent;
                var response = await httpClient.SendAsync(request);

                var apiResult = await response.Content.ReadAsStringAsync();

                Console.WriteLine("Merge response received.");
                Console.WriteLine(apiResult);
            }


        }
    }
}

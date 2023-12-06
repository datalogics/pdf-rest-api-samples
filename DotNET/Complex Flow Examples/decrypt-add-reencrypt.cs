using Newtonsoft.Json.Linq;
using System.Text;

/* In this sample we will show how to take an encrypted file and decrypt, modify
* and re-encrypt it to create an encryption-at-rest solution as discussed in
* https://pdfrest.com/solutions/create-secure-document-workflows-with-pdf-password-protection/
* We will be running the document through /decrypted-pdf to open the document
* to modification, running the decrypted result through /pdf-with-added-image,
* and then sending the output with the new image through /encrypted-pdf to
* lock it up again.
*/

using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com") })
{
    using (var decryptRequest = new HttpRequestMessage(HttpMethod.Post, "decrypted-pdf"))
    {
        decryptRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
        decryptRequest.Headers.Accept.Add(new("application/json"));
        var decryptMultipartContent = new MultipartFormDataContent();

        var byteArray = File.ReadAllBytes("/path/to/file.pdf");
        var byteAryContent = new ByteArrayContent(byteArray);
        decryptMultipartContent.Add(byteAryContent, "file", "file.pdf");
        byteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/pdf");

        var byteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes("password"));
        decryptMultipartContent.Add(byteArrayOption, "current_open_password");


        decryptRequest.Content = decryptMultipartContent;
        var decryptResponse = await httpClient.SendAsync(decryptRequest);

        var decryptResult = await decryptResponse.Content.ReadAsStringAsync();

        Console.WriteLine("Decrypt response received.");
        Console.WriteLine(decryptResult);

        dynamic decryptJson = JObject.Parse(decryptResult);
        string decryptID = decryptJson.outputId;


        using (var addImageRequest = new HttpRequestMessage(HttpMethod.Post, "pdf-with-added-image"))
        {
            addImageRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
            addImageRequest.Headers.Accept.Add(new("application/json"));
            var addImageMultipartContent = new MultipartFormDataContent();

            var addImageId = new ByteArrayContent(Encoding.UTF8.GetBytes(decryptID));
            addImageMultipartContent.Add(addImageId, "id");

            var addImageImage = File.ReadAllBytes("/path/to/file.png");
            var imageContent = new ByteArrayContent(addImageImage);
            addImageMultipartContent.Add(imageContent, "image_file", "file_name.png");
            imageContent.Headers.TryAddWithoutValidation("Content-Type", "image/png");

            var addImagePage = new ByteArrayContent(Encoding.UTF8.GetBytes("1"));
            addImageMultipartContent.Add(addImagePage, "page");

            var addImageX = new ByteArrayContent(Encoding.UTF8.GetBytes("0"));
            addImageMultipartContent.Add(addImageX, "x");
            var addImageY = new ByteArrayContent(Encoding.UTF8.GetBytes("0"));
            addImageMultipartContent.Add(addImageY, "y");

            addImageRequest.Content = addImageMultipartContent;
            var addImageResponse = await httpClient.SendAsync(addImageRequest);

            var addImageResult = await addImageResponse.Content.ReadAsStringAsync();

            Console.WriteLine("Add image response received.");
            Console.WriteLine(addImageResult);

            dynamic addImageJson = JObject.Parse(addImageResult);
            string addImageID = addImageJson.outputId;

            using (var encryptRequest = new HttpRequestMessage(HttpMethod.Post, "encrypted-pdf"))
            {
                encryptRequest.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
                encryptRequest.Headers.Accept.Add(new("application/json"));
                var multipartContent = new MultipartFormDataContent();

                var encryptID = new ByteArrayContent(Encoding.UTF8.GetBytes(addImageID));
                multipartContent.Add(encryptID, "id");

                var encryptPassword = new ByteArrayContent(Encoding.UTF8.GetBytes("password"));
                multipartContent.Add(encryptPassword, "new_open_password");


                encryptRequest.Content = multipartContent;
                var response = await httpClient.SendAsync(encryptRequest);

                var apiResult = await response.Content.ReadAsStringAsync();

                Console.WriteLine("Encrypt response received.");
                Console.WriteLine(apiResult);
            }
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com") })
{
    using (var request = new HttpRequestMessage(HttpMethod.Post, "signed-pdf"))
    {
        request.Headers.TryAddWithoutValidation("Api-Key", "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
        request.Headers.Accept.Add(new("application/json"));
        var multipartContent = new MultipartFormDataContent();

        var inputByteArray = File.ReadAllBytes("/path/to/input.pdf");
        var inputByteArrayContent = new ByteArrayContent(inputByteArray);
        multipartContent.Add(inputByteArrayContent, "file", "input.pdf");
        inputByteArrayContent.Headers.TryAddWithoutValidation("Content-Type", "application/pdf");

        var certByteArray = File.ReadAllBytes("/path/to/certificate.pem");
        var certByteArrayContent = new ByteArrayContent(certByteArray);
        multipartContent.Add(certByteArrayContent, "certificate_file", "certificate.pem");
        certByteArrayContent.Headers.TryAddWithoutValidation("Content-Type", "application/x-pem-file");

        var privateKeyByteArray = File.ReadAllBytes("/path/to/private_key.pem");
        var privateKeyByteArrayContent = new ByteArrayContent(privateKeyByteArray);
        multipartContent.Add(privateKeyByteArrayContent, "private_key_file", "private_key.pem");
        privateKeyByteArrayContent.Headers.TryAddWithoutValidation("Content-Type", "application/x-pem-file");

        var signatureConfiguration = new JObject
        {
            ["type"] = "new",
            ["name"] = "esignature",
            ["location"] = new JObject
            {
                ["bottom_left"] = new JObject
                {
                    ["x"] = "0",
                    ["y"] = "0"
                },
                ["top_right"] = new JObject
                {
                    ["x"] = "216",
                    ["y"] = "72"
                },
                ["page"] = "1"
            },
            ["display"] = new JObject
            {
                ["include_datetime"] = "true"
            }
        };

        var byteArrayOption = new ByteArrayContent(Encoding.UTF8.GetBytes(signatureConfiguration.ToString(Formatting.None)));
        multipartContent.Add(byteArrayOption, "signature_configuration");

        request.Content = multipartContent;
        var response = await httpClient.SendAsync(request);

        var apiResult = await response.Content.ReadAsStringAsync();

        Console.WriteLine("API response received.");
        Console.WriteLine(apiResult);
    }
}

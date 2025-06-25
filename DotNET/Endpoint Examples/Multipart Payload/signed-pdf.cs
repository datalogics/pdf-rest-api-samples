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

        var credByteArray = File.ReadAllBytes("/path/to/credentials.pfx");
        var credByteArrayContent = new ByteArrayContent(credByteArray);
        multipartContent.Add(credByteArrayContent, "pfx_credential_file", "credentials.pfx");
        credByteArrayContent.Headers.TryAddWithoutValidation("Content-Type", "application/x-pkcs12");

        var passphraseByteArray = File.ReadAllBytes("/path/to/passphrase.txt");
        var passphraseByteArrayContent = new ByteArrayContent(passphraseByteArray);
        multipartContent.Add(passphraseByteArrayContent, "pfx_passphrase_file", "passphrase.txt");
        passphraseByteArrayContent.Headers.TryAddWithoutValidation("Content-Type", "text/plain");

        var logoByteArray = File.ReadAllBytes("/path/to/logo.jpg");
        var logoByteArrayContent = new ByteArrayContent(logoByteArray);
        multipartContent.Add(logoByteArrayContent, "logo_file", "logo.jpg");
        logoByteArrayContent.Headers.TryAddWithoutValidation("Content-Type", "image/jpeg");

        var signatureConfiguration = new JObject
        {
            ["type"] = "new",
            ["name"] = "esignature",
            ["logo_opacity"] = "0.5",
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
                ["include_distinguished_name"] = "true",
                ["include_datetime"] = "true",
                ["contact"] = "My contact information",
                ["location"] = "My signing location",
                ["name"] = "John Doe",
                ["reason"] = "My reason for signing"
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

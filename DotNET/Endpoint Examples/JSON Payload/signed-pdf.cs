
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

const string ApiKey = "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
using var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com") };
string[] filePaths =
{
    "/path/to/input.pdf",
    "/path/to/credentials.pfx",
    "/path/to/passphrase.txt",
    "/path/to/logo.png"
};
string[] ids = new string[filePaths.Length];
for (int fileIdx = 0; fileIdx < filePaths.Length; fileIdx++)
{
    var uploadRequest = new HttpRequestMessage(HttpMethod.Post, "upload");
    uploadRequest.Headers.TryAddWithoutValidation("Api-Key", ApiKey);
    uploadRequest.Headers.Accept.Add(new("application/json"));

    var uploadByteArray = File.ReadAllBytes(filePaths[fileIdx]);
    var uploadByteAryContent = new ByteArrayContent(uploadByteArray);
    uploadByteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
    uploadByteAryContent.Headers.TryAddWithoutValidation("Content-Filename", Path.GetFileName(filePaths[fileIdx]));


    uploadRequest.Content = uploadByteAryContent;
    var uploadResponse = await httpClient.SendAsync(uploadRequest);

    var uploadResult = await uploadResponse.Content.ReadAsStringAsync();

    Console.WriteLine("Upload response received.");
    Console.WriteLine(uploadResult);

    JObject uploadResultJson = JObject.Parse(uploadResult);
    ids[fileIdx] = uploadResultJson["files"][0]["id"].ToString();
}


using var signedPdfRequest = new HttpRequestMessage(HttpMethod.Post, "signed-pdf");
signedPdfRequest.Headers.TryAddWithoutValidation("Api-Key", ApiKey);
signedPdfRequest.Headers.Accept.Add(new("application/json"));

signedPdfRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");

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

JObject parameterJson = new JObject
{
    ["id"] = ids[0],
    ["pfx_credential_id"] = ids[1],
    ["pfx_passphrase_id"] = ids[2],
    ["logo_id"] = ids[3],
    ["signature_configuration"] = signatureConfiguration.ToString(Formatting.None),
};
signedPdfRequest.Content = new StringContent(parameterJson.ToString(), Encoding.UTF8, "application/json");
var signedPdfResponse = await httpClient.SendAsync(signedPdfRequest);

var signedPdfResult = await signedPdfResponse.Content.ReadAsStringAsync();

Console.WriteLine("Processing response received.");
Console.WriteLine(signedPdfResult);
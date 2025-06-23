
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

const string ApiKey = "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
using var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com") };
string[] filePaths =
{
    "/path/to/input.pdf",
    "/path/to/certificate.pem",
    "/path/to/private_key.pem"
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

JObject parameterJson = new JObject
{
    ["id"] = ids[0],
    ["certificate_id"] = ids[1],
    ["private_key_id"] = ids[2],
    ["signature_configuration"] = signatureConfiguration.ToString(Formatting.None),
};
signedPdfRequest.Content = new StringContent(parameterJson.ToString(), Encoding.UTF8, "application/json");
var signedPdfResponse = await httpClient.SendAsync(signedPdfRequest);

var signedPdfResult = await signedPdfResponse.Content.ReadAsStringAsync();

Console.WriteLine("Processing response received.");
Console.WriteLine(signedPdfResult);
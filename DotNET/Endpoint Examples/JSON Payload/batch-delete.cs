using Newtonsoft.Json.Linq;
using System.Text;

var client = new HttpClient();
var request = new HttpRequestMessage(HttpMethod.Post, "https://api.pdfrest.com/delete");
request.Headers.Add("api-key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
request.Headers.TryAddWithoutValidation("Content-Type", "application/json");


JObject parameterJson = new JObject
{
    ["ids"] = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx, xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
};

request.Content = new StringContent(parameterJson.ToString(), Encoding.UTF8, "application/json"); ;
var response = await client.SendAsync(request);
response.EnsureSuccessStatusCode();
Console.WriteLine(await response.Content.ReadAsStringAsync());

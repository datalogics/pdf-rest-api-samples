var client = new HttpClient();
var request = new HttpRequestMessage(HttpMethod.Delete, "https://api.pdfrest.com/resource/xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
request.Headers.Add("api-key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
var response = await client.SendAsync(request);
response.EnsureSuccessStatusCode();
Console.WriteLine(await response.Content.ReadAsStringAsync());

var client = new HttpClient();
var request = new HttpRequestMessage(HttpMethod.Post, "https://api.pdfrest.com/delete");
request.Headers.Add("api-key", "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
var content = new MultipartFormDataContent();
content.Add(new StringContent("xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx,xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"), "ids");
request.Content = content;
var response = await client.SendAsync(request);
response.EnsureSuccessStatusCode();
Console.WriteLine(await response.Content.ReadAsStringAsync());

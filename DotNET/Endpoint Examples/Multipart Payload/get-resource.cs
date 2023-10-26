using (var httpClient = new HttpClient { BaseAddress = new Uri("https://api.pdfrest.com/resource/") } )
{
    try
    {
        string id = "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";  // ID to retrieve

        using (var stream = await httpClient.GetStreamAsync(id + "?format=file"))
        {
            using (var fs = new FileStream("/path/to/save/file", FileMode.CreateNew))
            {
                await stream.CopyToAsync(fs);
            }
        }
    }
    catch (HttpRequestException e)
    {
        Console.WriteLine("Message :{0} ", e.Message);
    }
}

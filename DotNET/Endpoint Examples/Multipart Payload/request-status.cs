using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;

/*
 * This sample demonstrates how to send a polling API request in order to obtain a request
 * status.
 */
public class PollableApiExample
{
    private const string ApiKey = "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"; // Your API key here
    private const string BaseUri = "https://api.pdfrest.com";

    public static async Task Main(String[] args)
    {
        const string pathToFile = "/path/to/file.pdf"; // Path to the input file here
        const string fileName = "file.pdf";            // Name of the file here

        // Send a request with the 'response-type' header.
        string bmpResponse = await GetBmpResponseAsync(pathToFile, fileName);
        dynamic bmpResponseJson = JObject.Parse(bmpResponse);
        if (bmpResponseJson.ContainsKey("error"))
        {
            Console.WriteLine($"Error from initial request: {bmpResponseJson.error}");
        } else
        {
            // Get the request ID from the response.
            string requestId = bmpResponseJson.requestId;
            Console.WriteLine($"Received request ID: {requestId}");

            // Use the request ID to send a polling request.
            string requestStatusResponse = await GetRequestStatusAsync(requestId);
            dynamic requestStatusResponseJson = JObject.Parse(requestStatusResponse);

            // If the request is still pending, send another call momentarily.
            while (requestStatusResponseJson.status == "pending")
            {
                const int delay = 5; // (seconds)
                Console.WriteLine($"Response from /request-status for request {requestId}: {requestStatusResponseJson}");
                Console.WriteLine($"Request status was \"pending\". Checking again in {delay} seconds...");
                await Task.Delay(TimeSpan.FromSeconds(delay));
                requestStatusResponse = await GetRequestStatusAsync(requestId);
                requestStatusResponseJson = JObject.Parse(requestStatusResponse);
            }
            // The request status is no longer "pending".
            Console.WriteLine($"Response from /request-status: {requestStatusResponseJson}");
            Console.WriteLine("Done!");
        }
    }

    /*
     * Send a request with the 'response-type' header to get a request ID. /bmp is an arbitrary example.
     */
    public static async Task<string> GetBmpResponseAsync(string pathToFile, string fileName)
    {
        using var httpClient = new HttpClient { BaseAddress = new Uri(BaseUri) };

        using var bmpRequest = new HttpRequestMessage(HttpMethod.Post, "bmp");

        bmpRequest.Headers.TryAddWithoutValidation("Api-Key", ApiKey);

        bmpRequest.Headers.Accept.Add(new("application/json"));
        var multipartContent = new MultipartFormDataContent();

        var byteArray = File.ReadAllBytes(pathToFile);
        var byteAryContent = new ByteArrayContent(byteArray);
        multipartContent.Add(byteAryContent, "file", fileName);
        byteAryContent.Headers.TryAddWithoutValidation("Content-Type", "application/pdf");

        // By adding the 'response-type' header to the request, the response will include a 'status' key.
        bmpRequest.Headers.Add("response-type", "requestId");

        bmpRequest.Content = multipartContent;
        Console.WriteLine("Sending request to /bmp...");
        var bmpResponse = await httpClient.SendAsync(bmpRequest);

        return await bmpResponse.Content.ReadAsStringAsync();
    }

    /*
     * Get the request status from /request-status using the given request ID.
     */
    public static async Task<string> GetRequestStatusAsync(string requestId)
    {
        using var httpClient = new HttpClient { BaseAddress = new Uri(BaseUri) };
        using var request = new HttpRequestMessage(HttpMethod.Get, $"request-status/{requestId}");
        request.Headers.TryAddWithoutValidation("Api-Key", ApiKey);

        var response = await httpClient.SendAsync(request);

        return await response.Content.ReadAsStringAsync();
    }
}

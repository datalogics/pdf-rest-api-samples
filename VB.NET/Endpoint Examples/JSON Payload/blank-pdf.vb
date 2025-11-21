'''
' What this sample does:
' - Calls /blank-pdf with a JSON payload to create a three-page blank PDF.
'
' Setup (environment):
' - Copy .env.example to .env
' - Set PDFREST_API_KEY=your_api_key_here
' - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
'     PDFREST_URL=https://eu-api.pdfrest.com
'   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
'
' Usage (via dispatcher):
'   dotnet run -- blank-pdf
'
' Output:
' - Prints the API JSON response to stdout. Non-2xx responses write a concise message to stderr and exit non-zero.
'''

Option Strict On
Option Explicit On

Imports System
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Text
Imports System.Text.Json
Imports System.Threading.Tasks

Namespace VBNetSamples.Endpoint_Examples.JSON_Payload
    Module BlankPdf
        Public Async Function Execute(args As String()) As Task
            Dim apiKey As String = Environment.GetEnvironmentVariable("PDFREST_API_KEY")
            If String.IsNullOrWhiteSpace(apiKey) Then
                Console.Error.WriteLine("Missing environment variable PDFREST_API_KEY.")
                Environment.Exit(1)
            End If

            Dim baseUrl As String = Environment.GetEnvironmentVariable("PDFREST_URL")
            If String.IsNullOrWhiteSpace(baseUrl) Then baseUrl = "https://api.pdfrest.com"

            Dim baseUri As Uri
            Try
                baseUri = New Uri(baseUrl)
            Catch ex As Exception
                Console.Error.WriteLine($"Invalid PDFREST_URL: {baseUrl}")
                Environment.Exit(1)
                Return
            End Try

            Using httpClient As New HttpClient()
                httpClient.BaseAddress = baseUri

                Dim request As New HttpRequestMessage(HttpMethod.Post, "blank-pdf")
                request.Headers.TryAddWithoutValidation("Api-Key", apiKey)
                request.Headers.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))

                Dim payload = New With {
                    .page_size = "letter",
                    .page_count = 3,
                    .page_orientation = "portrait"
                }
                Dim payloadJson As String = JsonSerializer.Serialize(payload)
                request.Content = New StringContent(payloadJson, Encoding.UTF8, "application/json")

                Dim response As HttpResponseMessage = Await httpClient.SendAsync(request)
                Dim responseBody As String = Await response.Content.ReadAsStringAsync()

                If Not response.IsSuccessStatusCode Then
                    Console.Error.WriteLine($"blank-pdf request failed: {CInt(response.StatusCode)} {response.ReasonPhrase}")
                    Console.Error.WriteLine(responseBody)
                    Environment.Exit(1)
                End If

                Console.WriteLine(responseBody)
            End Using
        End Function
    End Module
End Namespace

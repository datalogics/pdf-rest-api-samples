'''
' What this sample does:
' - Calls /blank-pdf with multipart/form-data to create a three-page blank PDF.
'
' Setup (environment):
' - Copy .env.example to .env
' - Set PDFREST_API_KEY=your_api_key_here
' - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
'     PDFREST_URL=https://eu-api.pdfrest.com
'   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
'
' Usage (via dispatcher):
'   dotnet run -- blank-pdf-multipart
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
Imports System.Threading.Tasks

Namespace VBNetSamples.Endpoint_Examples.Multipart_Payload
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

                Dim multipart As New MultipartFormDataContent()
                multipart.Add(New StringContent("letter", Encoding.UTF8), "page_size")
                multipart.Add(New StringContent("3", Encoding.UTF8), "page_count")
                multipart.Add(New StringContent("portrait", Encoding.UTF8), "page_orientation")

                Dim req As New HttpRequestMessage(HttpMethod.Post, "blank-pdf")
                req.Headers.TryAddWithoutValidation("Api-Key", apiKey)
                req.Headers.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))
                req.Content = multipart

                Dim resp As HttpResponseMessage = Await httpClient.SendAsync(req)
                Dim body As String = Await resp.Content.ReadAsStringAsync()
                If Not resp.IsSuccessStatusCode Then
                    Console.Error.WriteLine($"blank-pdf (multipart) failed: {CInt(resp.StatusCode)} {resp.ReasonPhrase}")
                    Console.Error.WriteLine(body)
                    Environment.Exit(1)
                End If

                Console.WriteLine(body)
            End Using
        End Function
    End Module
End Namespace

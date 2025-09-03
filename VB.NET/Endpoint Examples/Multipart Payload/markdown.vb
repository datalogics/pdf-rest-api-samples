'''
' What this sample does:
' - Converts a PDF to Markdown using pdfRest.
' - Sends a single multipart/form-data request directly to /markdown with the file.
'
' Setup (environment):
' - Copy .env.example to .env
' - Set PDFREST_API_KEY=your_api_key_here
' - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
'     PDFREST_URL=https://eu-api.pdfrest.com
'   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
'
' Usage (via dispatcher):
'   dotnet run --project VBNetSamples.vbproj -- markdown-multipart /path/to/input.pdf
'
' Output:
' - Prints the API JSON response to stdout. Non-2xx responses write a concise message to stderr and exit non-zero.
' - Tip: pipe output to a file: dotnet run --project VBNetSamples.vbproj -- markdown-multipart /path/to/input.pdf > response.json
'''

Option Strict On
Option Explicit On

Imports System
Imports System.IO
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Text
Imports System.Threading.Tasks

Namespace VBNetSamples.Endpoint_Examples.Multipart_Payload
    Module Markdown
        Public Async Function Execute(args As String()) As Task
            If args Is Nothing OrElse args.Length < 1 Then
                Console.Error.WriteLine("Usage: dotnet run -- markdown-multipart /path/to/input.pdf")
                Environment.Exit(1)
            End If

            Dim inputPath As String = args(0)
            If Not File.Exists(inputPath) Then
                Console.Error.WriteLine($"Input file not found: {inputPath}")
                Environment.Exit(1)
            End If

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
                Dim fileBytes As Byte() = File.ReadAllBytes(inputPath)
                Dim fileContent As New ByteArrayContent(fileBytes)
                fileContent.Headers.ContentType = New MediaTypeHeaderValue("application/pdf")
                multipart.Add(fileContent, "file", Path.GetFileName(inputPath))

                ' Optional parameters
                multipart.Add(New StringContent("pdfrest_markdown", Encoding.UTF8), "output")
                multipart.Add(New StringContent("on", Encoding.UTF8), "page_break_comments")
                ' e.g., page range: multipart.Add(New StringContent("1-3", Encoding.UTF8), "page_range")

                Dim req As New HttpRequestMessage(HttpMethod.Post, "markdown")
                req.Headers.TryAddWithoutValidation("Api-Key", apiKey)
                req.Headers.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))
                req.Content = multipart

                Dim resp As HttpResponseMessage = Await httpClient.SendAsync(req)
                Dim body As String = Await resp.Content.ReadAsStringAsync()
                If Not resp.IsSuccessStatusCode Then
                    Console.Error.WriteLine($"Markdown (multipart) failed: {CInt(resp.StatusCode)} {resp.ReasonPhrase}")
                    Console.Error.WriteLine(body)
                    Environment.Exit(1)
                End If

                Console.WriteLine(body)
            End Using
        End Function
    End Module
End Namespace

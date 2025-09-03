'''
' What this sample does:
' - Converts a PDF to Markdown using pdfRest.
' - Uses a JSON payload in two steps: first uploads, then calls /markdown with the uploaded id.
'
' Setup (environment):
' - Copy .env.example to .env
' - Set PDFREST_API_KEY=your_api_key_here
' - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
'     PDFREST_URL=https://eu-api.pdfrest.com
'   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
'
' Usage (via dispatcher):
'   dotnet run -- markdown /path/to/input.pdf
'
' Output:
' - Prints the API JSON response to stdout. Non-2xx responses write a concise message to stderr and exit non-zero.
' - Tip: pipe output to a file: dotnet run -- markdown /path/to/input.pdf > response.json
'''

Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Text
Imports System.Text.Json
Imports System.Threading.Tasks

Namespace VBNetSamples.Endpoint_Examples.JSON_Payload
    Module Markdown
        Public Async Function Execute(args As String()) As Task
            If args Is Nothing OrElse args.Length < 1 Then
                Console.Error.WriteLine("Usage: dotnet run -- markdown /path/to/input.pdf")
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

                ' 1) Upload
                Dim uploadRequest As New HttpRequestMessage(HttpMethod.Post, "upload")
                uploadRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey)
                uploadRequest.Headers.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))

                Dim fileBytes As Byte() = File.ReadAllBytes(inputPath)
                Dim uploadContent As New ByteArrayContent(fileBytes)
                uploadContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream")
                uploadContent.Headers.TryAddWithoutValidation("Content-Filename", Path.GetFileName(inputPath))
                uploadRequest.Content = uploadContent

                Dim uploadResponse As HttpResponseMessage = Await httpClient.SendAsync(uploadRequest)
                Dim uploadBody As String = Await uploadResponse.Content.ReadAsStringAsync()

                If Not uploadResponse.IsSuccessStatusCode Then
                    Console.Error.WriteLine($"Upload failed: {CInt(uploadResponse.StatusCode)} {uploadResponse.ReasonPhrase}")
                    Console.Error.WriteLine(uploadBody)
                    Environment.Exit(1)
                End If

                ' Optional: print upload JSON
                Console.WriteLine(uploadBody)

                Dim uploadedId As String = Nothing
                Try
                    Using doc As JsonDocument = JsonDocument.Parse(uploadBody)
                        uploadedId = doc.RootElement.GetProperty("files")(0).GetProperty("id").GetString()
                    End Using
                Catch ex As Exception
                    Console.Error.WriteLine("Failed to parse upload response JSON for file id.")
                    Console.Error.WriteLine(ex.Message)
                    Environment.Exit(1)
                End Try

                ' 2) Markdown via JSON payload
                Dim markdownRequest As New HttpRequestMessage(HttpMethod.Post, "markdown")
                markdownRequest.Headers.TryAddWithoutValidation("Api-Key", apiKey)
                markdownRequest.Headers.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))

                Dim payload As New Dictionary(Of String, String) From {
                    {"id", uploadedId}
                }
                Dim payloadJson As String = JsonSerializer.Serialize(payload)
                markdownRequest.Content = New StringContent(payloadJson, Encoding.UTF8, "application/json")

                Dim markdownResponse As HttpResponseMessage = Await httpClient.SendAsync(markdownRequest)
                Dim markdownBody As String = Await markdownResponse.Content.ReadAsStringAsync()

                If Not markdownResponse.IsSuccessStatusCode Then
                    Console.Error.WriteLine($"Markdown request failed: {CInt(markdownResponse.StatusCode)} {markdownResponse.ReasonPhrase}")
                    Console.Error.WriteLine(markdownBody)
                    Environment.Exit(1)
                End If

                Console.WriteLine(markdownBody)
            End Using
        End Function
    End Module
End Namespace

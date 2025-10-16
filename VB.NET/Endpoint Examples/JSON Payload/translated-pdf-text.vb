'''
' Translate PDF text using pdfRest.
' Two-step JSON flow: upload then call /translated-pdf-text with the uploaded id.
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
    Module TranslatedPdfText
        Public Async Function Execute(args As String()) As Task
            If args Is Nothing OrElse args.Length < 1 Then
                Console.Error.WriteLine("Usage: dotnet run -- translated-pdf-text /path/to/input.pdf")
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

                ' 2) Translate via JSON payload
                Dim req As New HttpRequestMessage(HttpMethod.Post, "translated-pdf-text")
                req.Headers.TryAddWithoutValidation("Api-Key", apiKey)
                req.Headers.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))

                Dim payload As New Dictionary(Of String, Object) From {
                    {"id", uploadedId},
                    {"output_language", "en-US"} ' Translates text to American English. Format the output_language as a 2-3 character ISO 639 code, optionally with a region/script (e.g., 'en', 'es', 'zh-Hant', 'eng-US').
                }
                Dim payloadJson As String = JsonSerializer.Serialize(payload)
                req.Content = New StringContent(payloadJson, Encoding.UTF8, "application/json")

                Dim resp As HttpResponseMessage = Await httpClient.SendAsync(req)
                Dim body As String = Await resp.Content.ReadAsStringAsync()
                If Not resp.IsSuccessStatusCode Then
                    Console.Error.WriteLine($"Translate request failed: {CInt(resp.StatusCode)} {resp.ReasonPhrase}")
                    Console.Error.WriteLine(body)
                    Environment.Exit(1)
                End If
                Console.WriteLine(body)
            End Using
        End Function
    End Module
End Namespace


'''
' Translate PDF text using pdfRest.
' Single multipart/form-data request to /translated-pdf-text with the file.
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
    Module TranslatedPdfText
        Public Async Function Execute(args As String()) As Task
            If args Is Nothing OrElse args.Length < 1 Then
                Console.Error.WriteLine("Usage: dotnet run -- translated-pdf-text-multipart /path/to/input.pdf")
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

                multipart.Add(New StringContent("en-US", Encoding.UTF8), "output_language") ' Translates text to American English. Format the output_language as a 2-3 character ISO 639 code, optionally with a region/script (e.g., 'en', 'es', 'zh-Hant', 'eng-US').

                Dim req As New HttpRequestMessage(HttpMethod.Post, "translated-pdf-text")
                req.Headers.TryAddWithoutValidation("Api-Key", apiKey)
                req.Headers.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))
                req.Content = multipart

                Dim resp As HttpResponseMessage = Await httpClient.SendAsync(req)
                Dim body As String = Await resp.Content.ReadAsStringAsync()
                If Not resp.IsSuccessStatusCode Then
                    Console.Error.WriteLine($"Translate (multipart) failed: {CInt(resp.StatusCode)} {resp.ReasonPhrase}")
                    Console.Error.WriteLine(body)
                    Environment.Exit(1)
                End If
                Console.WriteLine(body)
            End Using
        End Function
    End Module
End Namespace


'''
' What this sample does:
' - Merges multiple inputs (PDFs and non-PDFs) into a single PDF.
' - Non-PDF files are first converted to PDF; PDFs are uploaded as-is. All resulting IDs are then merged.
'
' Setup (environment):
' - Copy .env.example to .env
' - Set PDFREST_API_KEY=your_api_key_here
' - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
'     PDFREST_URL=https://eu-api.pdfrest.com
'   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
'
' Usage (via dispatcher):
'   dotnet run -- merge-different-file-types /path/to/file1 /path/to/file2 [...]
'
' Output:
' - Prints the API JSON response to stdout. Non-2xx responses write a concise message to stderr and exit non-zero.
' - Tip: pipe output to a file: dotnet run -- merge-different-file-types ... > response.json
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

Namespace VBNetSamples.Complex_Flow_Examples
    Module MergeDifferentFileTypes
        Public Async Function Execute(args As String()) As Task
            If args Is Nothing OrElse args.Length < 2 Then
                Console.Error.WriteLine("Usage: dotnet run -- merge-different-file-types /path/to/file1 /path/to/file2 [/path/to/file3 ...]")
                Environment.Exit(1)
            End If

            Dim allExist = Array.TrueForAll(args, Function(p) File.Exists(p))
            If Not allExist Then
                Console.Error.WriteLine("One or more input files do not exist.")
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

                Dim collectedIds As New List(Of String)()

                For i = 0 To args.Length - 1
                    Dim p As String = args(i)
                    Dim ext As String = Path.GetExtension(p).ToLowerInvariant()
                    If ext = ".pdf" Then
                        ' Upload PDFs to get an id
                        Dim uploadReq As New HttpRequestMessage(HttpMethod.Post, "upload")
                        uploadReq.Headers.TryAddWithoutValidation("Api-Key", apiKey)
                        uploadReq.Headers.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))

                        Dim pdfBytes As Byte() = File.ReadAllBytes(p)
                        Dim uploadContent As New ByteArrayContent(pdfBytes)
                        uploadContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream")
                        uploadContent.Headers.TryAddWithoutValidation("Content-Filename", Path.GetFileName(p))
                        uploadReq.Content = uploadContent

                        Dim uploadResp = Await httpClient.SendAsync(uploadReq)
                        Dim uploadBody = Await uploadResp.Content.ReadAsStringAsync()
                        If Not uploadResp.IsSuccessStatusCode Then
                            Console.Error.WriteLine($"Upload failed for input #{i + 1}: {CInt(uploadResp.StatusCode)} {uploadResp.ReasonPhrase}")
                            Console.Error.WriteLine(uploadBody)
                            Environment.Exit(1)
                        End If

                        Dim id As String = Nothing
                        Using doc = JsonDocument.Parse(uploadBody)
                            id = doc.RootElement.GetProperty("files")(0).GetProperty("id").GetString()
                        End Using
                        collectedIds.Add(id)
                    Else
                        ' Convert non-PDF to PDF to get an outputId
                        Dim multipart As New MultipartFormDataContent()
                        Dim fileBytes As Byte() = File.ReadAllBytes(p)
                        Dim fileContent As New ByteArrayContent(fileBytes)
                        fileContent.Headers.ContentType = New MediaTypeHeaderValue(ContentTypeFor(p))
                        multipart.Add(fileContent, "file", Path.GetFileName(p))

                        Dim convReq As New HttpRequestMessage(HttpMethod.Post, "pdf")
                        convReq.Headers.TryAddWithoutValidation("Api-Key", apiKey)
                        convReq.Headers.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))
                        convReq.Content = multipart

                        Dim convResp = Await httpClient.SendAsync(convReq)
                        Dim convBody = Await convResp.Content.ReadAsStringAsync()
                        If Not convResp.IsSuccessStatusCode Then
                            Console.Error.WriteLine($"Conversion failed for input #{i + 1}: {CInt(convResp.StatusCode)} {convResp.ReasonPhrase}")
                            Console.Error.WriteLine(convBody)
                            Environment.Exit(1)
                        End If

                        Using doc = JsonDocument.Parse(convBody)
                            Dim outputId = doc.RootElement.GetProperty("outputId").GetString()
                            collectedIds.Add(outputId)
                        End Using
                    End If
                Next

                ' Build x-www-form-urlencoded body with arrays
                Dim pairs As New List(Of KeyValuePair(Of String, String))()
                For Each id In collectedIds
                    pairs.Add(New KeyValuePair(Of String, String)("id[]", id))
                    pairs.Add(New KeyValuePair(Of String, String)("pages[]", "1-last"))
                    pairs.Add(New KeyValuePair(Of String, String)("type[]", "id"))
                Next

                Dim mergeReq As New HttpRequestMessage(HttpMethod.Post, "merged-pdf")
                mergeReq.Headers.TryAddWithoutValidation("Api-Key", apiKey)
                mergeReq.Headers.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))
                mergeReq.Content = New FormUrlEncodedContent(pairs)

                Dim mergeResp = Await httpClient.SendAsync(mergeReq)
                Dim mergeBody = Await mergeResp.Content.ReadAsStringAsync()
                If Not mergeResp.IsSuccessStatusCode Then
                    Console.Error.WriteLine($"Merge failed: {CInt(mergeResp.StatusCode)} {mergeResp.ReasonPhrase}")
                    Console.Error.WriteLine(mergeBody)
                    Environment.Exit(1)
                End If

                Console.WriteLine(mergeBody)
            End Using
        End Function

        Private Function ContentTypeFor(filePath As String) As String
            Dim ext = System.IO.Path.GetExtension(filePath).ToLowerInvariant()
            Select Case ext
                Case ".pdf" : Return "application/pdf"
                Case ".png" : Return "image/png"
                Case ".jpg", ".jpeg" : Return "image/jpeg"
                Case ".gif" : Return "image/gif"
                Case ".tif", ".tiff" : Return "image/tiff"
                Case ".bmp" : Return "image/bmp"
                Case ".webp" : Return "image/webp"
                Case ".doc" : Return "application/msword"
                Case ".docx" : Return "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                Case ".ppt" : Return "application/vnd.ms-powerpoint"
                Case ".pptx" : Return "application/vnd.openxmlformats-officedocument.presentationml.presentation"
                Case ".xls" : Return "application/vnd.ms-excel"
                Case ".xlsx" : Return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Case ".txt" : Return "text/plain"
                Case ".rtf" : Return "application/rtf"
                Case ".html", ".htm" : Return "text/html"
                Case Else : Return "application/octet-stream"
            End Select
        End Function
    End Module
End Namespace

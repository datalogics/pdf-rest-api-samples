'''
' What this sample does:
' - Provides a command dispatcher for VB.NET samples in this folder.
' - Routes `dotnet run -- <command>` to the corresponding sample module.
'
' Setup (environment):
' - Copy .env.example to .env
' - Set PDFREST_API_KEY=your_api_key_here
' - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
'     PDFREST_URL=https://eu-api.pdfrest.com
'   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
'
' Usage:
'   dotnet run -- <command> [args]
'   e.g., dotnet run -- markdown /path/to/input.pdf
'
' Output:
' - Each command prints the API JSON response to stdout. Non-2xx responses write a concise message to stderr and exit non-zero.
'''

Option Strict On
Option Explicit On

Imports System
Imports System.Linq
Imports System.Threading.Tasks
Imports DotNetEnv

Module Program
    Public Sub Main(args As String())
        MainAsync(args).GetAwaiter().GetResult()
    End Sub

    Private Async Function MainAsync(args As String()) As Task
        ' Load environment variables from .env if present (via DotNetEnv)
        Env.Load()
        If args Is Nothing OrElse args.Length = 0 Then
            PrintUsage()
            Environment.Exit(1)
        End If

        Dim cmd As String = args(0).Trim().ToLowerInvariant()
        Dim rest As String()
        If args.Length > 1 Then
            rest = args.Skip(1).ToArray()
        Else
            rest = Array.Empty(Of String)()
        End If

        Select Case cmd
            Case "markdown", "markdown-json"
                Await VBNetSamples.Endpoint_Examples.JSON_Payload.Markdown.Execute(rest)
            Case "rasterized-pdf", "rasterize-json"
                Await VBNetSamples.Endpoint_Examples.JSON_Payload.RasterizedPdf.Execute(rest)
            Case "blank-pdf"
                Await VBNetSamples.Endpoint_Examples.JSON_Payload.BlankPdf.Execute(rest)
            Case "summarized-pdf-text"
                Await VBNetSamples.Endpoint_Examples.JSON_Payload.SummarizedPdfText.Execute(rest)
            Case "translated-pdf-text"
                Await VBNetSamples.Endpoint_Examples.JSON_Payload.TranslatedPdfText.Execute(rest)
            Case "markdown-multipart"
                Await VBNetSamples.Endpoint_Examples.Multipart_Payload.Markdown.Execute(rest)
            Case "rasterized-pdf-multipart", "rasterize-multipart"
                Await VBNetSamples.Endpoint_Examples.Multipart_Payload.RasterizedPdf.Execute(rest)
            Case "blank-pdf-multipart"
                Await VBNetSamples.Endpoint_Examples.Multipart_Payload.BlankPdf.Execute(rest)
            Case "summarized-pdf-text-multipart"
                Await VBNetSamples.Endpoint_Examples.Multipart_Payload.SummarizedPdfText.Execute(rest)
            Case "translated-pdf-text-multipart"
                Await VBNetSamples.Endpoint_Examples.Multipart_Payload.TranslatedPdfText.Execute(rest)
            Case "merge-different-file-types", "merge"
                Await VBNetSamples.Complex_Flow_Examples.MergeDifferentFileTypes.Execute(rest)
        Case Else
                Console.Error.WriteLine($"Unknown command: {cmd}")
                PrintUsage()
                Environment.Exit(1)
        End Select
    End Function

    ' DotNetEnv handles .env loading; no manual parser needed.

    Private Sub PrintUsage()
        Console.Error.WriteLine("Usage: dotnet run -- <command> [args]")
        Console.Error.WriteLine("")
        Console.Error.WriteLine("Commands:")
        Console.Error.WriteLine("  markdown | markdown-json         Upload then convert to Markdown (JSON two-step)")
        Console.Error.WriteLine("  rasterized-pdf | rasterize-json  Upload then rasterize PDF (JSON two-step)")
        Console.Error.WriteLine("  blank-pdf                        Generate an empty PDF (JSON request)")
        Console.Error.WriteLine("  summarized-pdf-text              Upload then summarize text (JSON two-step)")
        Console.Error.WriteLine("  translated-pdf-text              Upload then translate text (JSON two-step)")
        Console.Error.WriteLine("  markdown-multipart               Convert to Markdown (single multipart request)")
        Console.Error.WriteLine("  rasterized-pdf-multipart         Rasterize PDF (single multipart request)")
        Console.Error.WriteLine("  blank-pdf-multipart              Generate an empty PDF (multipart request)")
        Console.Error.WriteLine("  summarized-pdf-text-multipart   Summarize text (single multipart request)")
        Console.Error.WriteLine("  translated-pdf-text-multipart   Translate text (single multipart request)")
        Console.Error.WriteLine("  merge-different-file-types|merge Merge PDFs and non-PDFs into one PDF")
        Console.Error.WriteLine("")
        Console.Error.WriteLine("Examples:")
        Console.Error.WriteLine("  dotnet run -- markdown /path/to/input.pdf")
        Console.Error.WriteLine("  dotnet run -- rasterized-pdf /path/to/input.pdf")
        Console.Error.WriteLine("  dotnet run -- markdown-multipart /path/to/input.pdf")
        Console.Error.WriteLine("  dotnet run -- rasterized-pdf-multipart /path/to/input.pdf")
        Console.Error.WriteLine("  dotnet run -- merge-different-file-types file1.pdf file2.docx image.png")
    End Sub
End Module

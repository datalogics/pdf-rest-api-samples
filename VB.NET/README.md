# pdfRest VB.NET Samples

These VB.NET samples demonstrate how to call pdfRest APIs using HttpClient. A single console project (`VBNetSamples.vbproj`) acts as a dispatcher so you can run any sample with a simple command.

## Prerequisites
- .NET SDK 8.0 or newer (check with `dotnet --version`).
- A pdfRest API key.
- Internet access to reach `https://api.pdfrest.com` (or the EU endpoint below).

## Quick Start
1) From this `VB.NET/` folder, copy the environment template and set your API key:
- `cp .env.example .env`
- Edit `.env` and set `PDFREST_API_KEY=your_api_key_here`

2) Optional: EU/GDPR endpoint for European data residency and performance:
- In `.env`, set `PDFREST_URL=https://eu-api.pdfrest.com`
- More info: https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work

3) Restore and build the project:
- `dotnet restore VBNetSamples.vbproj`
- `dotnet build VBNetSamples.vbproj`

4) Run a sample (examples below). The dispatcher pattern is:
- `dotnet run --project VBNetSamples.vbproj -- <command> [args]`

## How It’s Organized
- `VBNetSamples.vbproj`: single console app that compiles all VB samples in this folder.
- `Program.vb`: a small dispatcher that routes `<command>` to a specific sample’s `Execute` method.
- `Endpoint Examples/JSON Payload/`: two‑step samples (upload, then operate with a JSON body).
- `Endpoint Examples/Multipart Payload/`: single multipart/form-data request per operation.
- `Complex Flow Examples/`: multi-step workflows combining endpoints.
- `.env`: holds configuration (`PDFREST_API_KEY`, optional `PDFREST_URL`). Loaded automatically via DotNetEnv.

## Available Samples
JSON two‑step (upload → operate via JSON):
- `markdown` — Convert PDF to Markdown
  - Run: `dotnet run --project VBNetSamples.vbproj -- markdown /path/to/input.pdf`
- `rasterized-pdf` — Rasterize a PDF
  - Run: `dotnet run --project VBNetSamples.vbproj -- rasterized-pdf /path/to/input.pdf`

Multipart single request (send file directly):
- `markdown-multipart` — Convert PDF to Markdown
  - Run: `dotnet run --project VBNetSamples.vbproj -- markdown-multipart /path/to/input.pdf`
- `rasterized-pdf-multipart` — Rasterize a PDF
  - Run: `dotnet run --project VBNetSamples.vbproj -- rasterized-pdf-multipart /path/to/input.pdf`

Complex flow:
- `merge-different-file-types` — Merge multiple inputs (PDFs and non‑PDFs)
  - Run: `dotnet run --project VBNetSamples.vbproj -- merge-different-file-types file1.pdf file2.docx image.png`

## Output & Error Handling
- Successful calls print the API’s JSON response to stdout.
- Failures write a concise message to stderr and exit with a non‑zero code.
- Tip: redirect output to a file, e.g., `... > response.json`.

## Troubleshooting
- Missing API key:
  - Ensure `.env` exists with `PDFREST_API_KEY` set, or export it in your shell before running.
- EU endpoint:
  - If using the EU endpoint, verify `PDFREST_URL=https://eu-api.pdfrest.com` in `.env`.
- .NET version:
  - Check with `dotnet --version`. Use .NET 8.0 or newer.
- Clean build:
  - `rm -rf bin obj && dotnet build VBNetSamples.vbproj`
- Proxy environments:
  - If your network requires a proxy, set standard environment variables like `HTTPS_PROXY`.

## Notes
- These are focused samples to help you evaluate pdfRest quickly.
- Do not commit secrets. Keep `.env` out of version control.
- For more APIs, options, and regions, see the pdfRest documentation and pricing page.

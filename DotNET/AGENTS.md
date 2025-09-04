# Repository Guidelines

## Project Structure & Module Organization
- DotNET samples are organized by scenario:
  - `Endpoint Examples/Multipart Payload`: send files + params via `multipart/form-data`.
  - `Endpoint Examples/JSON Payload`: upload files first, then call endpoints with JSON.
  - `Complex Flow Examples`: multi‑step workflows across endpoints.
- Each `.cs` file is a self‑contained console sample targeting `https://api.pdfrest.com`.

## Build, Test, and Development Commands
- Prereq: .NET 6+ SDK (`dotnet --version`).
- Quick run (copy a sample into a console app):
  - `dotnet new console -n PdfRestSample`
  - `cd PdfRestSample`
  - `cp ../DotNET/Endpoint\ Examples/Multipart\ Payload/pdf.cs Program.cs`
  - Replace the placeholder API key and file paths in `Program.cs`.
  - `dotnet run`
- JSON flow example:
  - `cp ../DotNET/Endpoint\ Examples/JSON\ Payload/merged-pdf.cs Program.cs && dotnet run`

## Coding Style & Naming Conventions
- Language: C# 10+ with top‑level statements and `async/await`.
- Indentation: 4 spaces; keep samples single‑file and minimal.
- Filenames: kebab‑case describing action, e.g., `pdf-with-ocr-text.cs`, `merged-pdf.cs`.
- HTTP: use `HttpClient`, set `Api-Key` header, prefer `await httpClient.SendAsync(...)`.

## Testing Guidelines
- No unit test suite in this folder; validate by running the sample and inspecting console output/JSON.
- For binary outputs, write response bytes to a file and open locally (e.g., PDF/png) as needed.

## Commit & Pull Request Guidelines
- Commits: concise and scoped to a sample. Suggested prefixes: `feat(sample)`, `fix(sample)`, `docs(dotnet)`.
- PRs should include:
  - Purpose and endpoint(s) touched, links to API docs if relevant.
  - Input assumptions (paths, content types) and example command to run.
  - Before/after behavior or sample output snippet.

## Security & Configuration Tips
- Do not commit secrets. Replace `"xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"` with a local value only.
- Prefer environment variables, e.g., `var apiKey = Environment.GetEnvironmentVariable("PDFREST_API_KEY");` and fall back to a placeholder during development.
- Verify content types and file paths match your inputs (e.g., `text/html`, `application/pdf`).

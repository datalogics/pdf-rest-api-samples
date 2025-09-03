# Repository Guidelines

## Project Structure & Module Organization
- `VBNetSamples.vbproj`: Single root console project compiling all VB samples.
- `Program.vb`: Dispatcher routing `dotnet run -- <command>` to a specific sample.
- `Endpoint Examples/JSON Payload/`: Two-step samples (upload then operate), e.g., `markdown.vb`.
- `Endpoint Examples/Multipart Payload/`: Single multipart request samples, e.g., `rasterized-pdf.vb`.
- `Complex Flow Examples/`: Multi-step workflows chaining several endpoints.
- `.env.example` → copy to `.env` and set `PDFREST_API_KEY` (loaded by DotNetEnv at startup).
- `README.md`: High-level setup and usage for VB.NET samples.

## Build, Test, and Development Commands
- Install .NET SDK (8.0+): verify with `dotnet --version`.
- Restore/build (from VB.NET folder): `dotnet restore && dotnet build`.
- Run a sample via dispatcher: `dotnet run -- <command> [args]`.
  - Example: `dotnet run -- markdown /path/to/input.pdf`.
- Dependencies:
  - `.env` loading is included via `DotNetEnv`.
  - Optional JSON helper: `dotnet add package Newtonsoft.Json` (only if a sample uses it).
- File-focused testing: keep each sample self-contained (accept input path as first arg; minimal edits).

## Coding Style & Naming Conventions
- VB style: 4-space indentation, `PascalCase` for types/methods, `camelCase` for locals/parameters.
- File naming: match pdfRest endpoint with hyphenated lowercase, e.g., `markdown.vb`, `rasterized-pdf.vb`.
- Sample structure: each sample exposes `Public Async Function Execute(args() As String) As Task` and lives under a namespace mirroring its folder. Use underscores for folder segments that contain spaces:
  - JSON two-step: `Namespace VBNetSamples.Endpoint_Examples.JSON_Payload`
  - Multipart: `Namespace VBNetSamples.Endpoint_Examples.Multipart_Payload`
  - Complex flows: `Namespace VBNetSamples.Complex_Flow_Examples`
  - Note: This satisfies IDE checks like “Namespace does not correspond to file location”.
- Dispatcher: `Program.vb` contains `Sub Main` and routes commands to `...Execute(args)`; do not add additional `Main` methods in samples.
- Env config: read `PDFREST_API_KEY` and optional `PDFREST_URL`; `.env` is auto-loaded by `DotNetEnv`.
- HTTP: use `HttpClient` with explicit `Accept`/`Content-Type`; set `Api-Key` per request; use `MultipartFormDataContent`; default base URL `https://api.pdfrest.com` with `PDFREST_URL` override.
- Errors: check `response.IsSuccessStatusCode`; on failure, write a concise error to `Console.Error` and exit non-zero.

## Testing Guidelines
- No formal test suite required. Validate by running samples against known inputs.
- Success criteria: exit non-zero on failures; print API JSON response to stdout on success.
- New samples: accept input path as first argument when practical; document optional params inline near usage.

## Commit & Pull Request Guidelines
- Commits: imperative mood and focused scope (e.g., "Add multipart markdown VB sample").
- Describe what changed and why; reference endpoint and path.
- PRs: include clear description, run commands used for verification, expected response snippet, and any notes on options.
- Link related issues; avoid unrelated changes in the same PR.

## Security & Configuration Tips
- Keep secrets out of VCS: do not commit `.env`; include a `.env.example` with placeholder keys if the folder uses `.env`.
- Support proxies via standard env vars (e.g., `HTTPS_PROXY`) when applicable.
- Never log API keys; print response bodies and minimal diagnostics only (send diagnostics to `Console.Error`).
- API base override: set `PDFREST_URL` to change regions. For EU GDPR compliance and improved performance in Europe, you may use `https://eu-api.pdfrest.com/`. More info: https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work

---

## Audience And Tone (Internal)

These samples are customer-facing and intended to help potential customers evaluate pdfRest quickly. Keep all code, comments, and documentation clear, minimal, and task-focused. Avoid internal jargon and advanced implementation details in customer-visible files. Do not surface internal process notes in `README.md` or the samples.

Key points:
- Clarity: explain what the sample does in 1–2 bullets.
- Guidance: show how to set the API key and how to run.
- Region: mention optional `PDFREST_URL` with the EU endpoint for GDPR and proximity.
- Safety: never log secrets; print only response bodies and minimal diagnostics to stderr.
- Errors: exit non‑zero on non‑2xx with a concise message.

## Sample Header Convention (Internal)

Add this standardized header comment at the top of every VB.NET sample. This header is customer-visible but the convention itself is for us to remember here (do not duplicate these instructions in README).

Template:

'''
' What this sample does:
' - <One–two bullets describing purpose and request style>
'
' Setup (environment):
' - Copy .env.example to .env
' - Set PDFREST_API_KEY=your_api_key_here
' - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
'     PDFREST_URL=https://eu-api.pdfrest.com
'   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
'
' Usage (via dispatcher):
'   dotnet run -- <command-name> /path/to/input.pdf
'
' Output:
' - Prints the API JSON response to stdout. Non-2xx responses write a concise message to stderr and exit non-zero.
' - Tip: pipe output to a file: dotnet run -- <command-name> ... > response.json
'''

Notes:
- Match the endpoint name and request style (JSON two‑step vs multipart single request) in the bullets.
- Keep the header concise; avoid adding options unless essential.
- If a sample accepts optional parameters, mention them inline in code near where they are used.

## README Scope (Internal)

Keep `README.md` focused on user setup, running samples, and high‑level background. Avoid internal conventions or meta‑process content that could confuse customers. Place internal notes and templates in `AGENTS.md` (this file).

---

## VB.NET Implementation Notes (Internal)
- Dispatcher entry point: `Program.vb` exposes `Sub Main` (wrapping an async method) and selects a sample based on the first CLI argument. Only one entry point exists.
- Add a new sample:
  - Create a `.vb` file under the appropriate folder.
  - Namespace as noted above; expose `Public Async Function Execute(args() As String) As Task`.
  - Update `Program.vb` `Select Case` to map a command (e.g., `png`) to the new module’s `Execute`.
- JSON parsing: prefer `System.Text.Json`; if using `Newtonsoft.Json`, add the NuGet package and import `Newtonsoft.Json.Linq` for simple extraction.
- Multipart uploads: `New MultipartFormDataContent()` with `ByteArrayContent` or `StreamContent`. Set `Content-Type` and filename as needed.
- Headers: set `Api-Key` per request; always send `Accept: application/json`.
- Base URL: default `https://api.pdfrest.com`; support `PDFREST_URL` override.
- Env loader: `.env` is loaded automatically via `DotNetEnv` at startup.
- Errors: prefer `If Not response.IsSuccessStatusCode Then ... Environment.Exit(1)`.

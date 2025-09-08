# Repository Guidelines

## Project Structure & Module Organization
- DotNET samples are organized by scenario:
  - `Endpoint Examples/Multipart Payload`: send files + params via `multipart/form-data`.
  - `Endpoint Examples/JSON Payload`: upload files then call endpoints with JSON.
  - `Complex Flow Examples`: multi‑step workflows across endpoints.
- Recommended: a single .NET 8 console app project that dispatches to these samples via a command.

## Build, Run, and Environment
- Prereq: .NET 8 SDK (`dotnet --version`).
- Included here:
  - `DotNetSamples.csproj` wired to `Program.cs` (dispatcher).
  - `.env.example` template for local configuration.
- Environment config (mirrors VB.NET):
  - Copy `.env.example` to `.env` and set `PDFREST_API_KEY=...`.
  - Optional: `PDFREST_URL=https://eu-api.pdfrest.com` for EU/GDPR; default is `https://api.pdfrest.com`.
- Run commands from this folder:
  - `dotnet build`
  - `dotnet run -- markdown-json /path/to/input.pdf`
  - `dotnet run -- rasterized-pdf /path/to/input.pdf`
  - `dotnet run -- pdf-multipart /path/to/input.html text/html`
  - `dotnet run -- merge-different-file-types image.png slides.ppt`
  - `dotnet run -- extracted-text /path/to/input.pdf`
  - `dotnet run -- extracted-images /path/to/input.pdf`
  - `dotnet run -- pdf-info /path/to/input.pdf`
  - `dotnet run -- merged-pdf file1.pdf file2.pdf`
  - `dotnet run -- split-pdf /path/to/input.pdf`
  - `dotnet run -- upload /path/to/input.pdf`
  - `dotnet run -- get-resource <id> [out]`
  - `dotnet run -- delete-resource <id>`
  - `dotnet run -- batch-delete <id1> [id2] [...]`
- `.env` loading: uses `DotNetEnv` (`Env.Load()`), matching VB.NET.

Note: The project currently compiles only `Program.cs` and `Endpoint Examples/JSON Payload/markdown.cs` to avoid top‑level statement conflicts. To enable more samples, add them explicitly to `DotNetSamples.csproj` under a `<Compile Include="..." />` item or refactor them into classes with `Execute(string[] args)`.

Why no solution file?
- This folder contains a single project; removing the `.sln` lets `dotnet build` and `dotnet run` infer the project automatically. If you later add multiple projects, reintroduce a solution or pass `--project` explicitly.

## Coding Style & Naming Conventions
- C# 12 / .NET 8; 4‑space indentation; minimal, single‑responsibility methods.
- Filenames: kebab‑case describing action (e.g., `pdf-with-ocr-text.cs`).
- HTTP: use `HttpClient` with `Api-Key` header; base URL from `PDFREST_URL` to support region selection.
- Prefer low-indentation patterns:
  - File-scoped namespaces (`namespace X.Y;`).
  - `using var` for disposables instead of nested `using (...) {}` blocks.

## Testing Guidelines
- No unit tests here; validate by running commands and inspecting JSON output.
- For binary endpoints, write bytes to disk and manually verify (open PDF/image).

## Adapting Samples (Minimal Changes)
- Keep the original I/O style: if a sample uses sync calls, keep them sync; if it uses async/await, keep it async.
- Wrap in a small shim so Program.cs can call it:
  - `namespace Samples.<FolderPath>;`
  - `public static class <SampleName> { public static <void|Task> Execute(string[] args) { /* existing code */ } }`
- Replace literals with environment/args only:
  - API key: `Environment.GetEnvironmentVariable("PDFREST_API_KEY")`
  - Base URL: `Environment.GetEnvironmentVariable("PDFREST_URL") ?? "https://api.pdfrest.com"`
  - Input path: take from `args[0]` and use for `Content-Filename`.
- Opt the file into the project by adding `<Compile Include="…" />` to `DotNetSamples.csproj`.
- Always insert the Sample Header Template at the very beginning of the file (before any `using` lines).

## Sample Header Template
Use this comment block at the TOP of each sample file you adapt for dispatching — place it before any `using` directives or namespace (keep lines concise and include the GDPR info line):

```
/*
 * What this sample does:
 * - Brief description… (how it’s called from Program.cs)
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   dotnet run -- <command> [args]
 *
 * Output:
 * - Describe output and error behavior.
 */
```

## Commit & Pull Request Guidelines
- Commits: concise, scoped to a sample or infra. Prefixes: `feat(sample)`, `fix(sample)`, `chore(env)`, `docs(dotnet)`.
- PRs include: purpose, endpoints, run example (`dotnet run -- <command>`), assumptions (paths/content types), and sample output.

## Security & Region/GDPR Tips
- Never commit secrets or `.env`; provide `.env.example` only.
- Prefer `DotNetEnv` + `Environment.GetEnvironmentVariable` for keys/URLs.
- EU/GDPR: set `PDFREST_URL=https://eu-api.pdfrest.com` to keep data in-region; default is `https://api.pdfrest.com`.

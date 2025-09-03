# Repository Guidelines

## Project Structure & Module Organization
- `Endpoint Examples/JSON Payload/`: Two-step samples (upload then operate), e.g., `markdown.rb`.
- `Endpoint Examples/Multipart Payload/`: Single multipart request samples, e.g., `rasterized-pdf.rb`.
- `Gemfile` and `Gemfile.lock`: Dependencies (`faraday`, `faraday-multipart`, `faraday-retry`, `dotenv`).
- `.env.example` → copy to `.env` and set `PDFREST_API_KEY`.
- `README.md`: Setup, usage, and background details.

## Build, Test, and Development Commands
- Install Ruby (via rbenv): `rbenv install 3.4.4 && rbenv local 3.4.4`.
- Install deps: `bundle install`.
- Update deps: `bundle update`.
- Run JSON sample: `ruby "Endpoint Examples/JSON Payload/markdown.rb" /path/to/input.pdf`.
- Run Multipart sample: `ruby "Endpoint Examples/Multipart Payload/rasterized-pdf.rb" /path/to/input.pdf`.
- Tip: Pipe output to a file: `... > response.json`.

## Coding Style & Naming Conventions
- Ruby style: 2-space indentation, `snake_case` for files, methods, and variables.
- Script naming: match pdfRest endpoint (e.g., `markdown.rb`, `rasterized-pdf.rb`).
- Env config: load via `dotenv`; read `PDFREST_API_KEY` and abort with clear messages.
- HTTP: use `Faraday` with `:retry` (short, limited retries). Prefer explicit headers and content types. Base URL from `ENV['PDFREST_URL']` (defaults to `https://api.pdfrest.com`).
- Errors: `abort` on non-2xx with concise context; print API responses to stdout.

## Testing Guidelines
- No formal test suite (RSpec) present. Validate by running samples against known inputs.
- Success criteria: non-zero exit on failures; JSON printed to stdout on success.
- Add samples consistently: accept input path as first argument; document optional params inline.

## Commit & Pull Request Guidelines
- Commits: imperative mood and focused scope (e.g., "Add multipart markdown sample").
- Include what changed and why; reference endpoint and path.
- PRs: clear description, run commands used for verification, expected response snippet, and any notes on options.
- Link related issues; avoid unrelated changes.

## Security & Configuration Tips
- Keep secrets out of VCS: do not commit `.env`; use `.env.example` as a template.
- Handle proxies via standard env vars (e.g., `HTTPS_PROXY`) when needed.
- Never log API keys; prefer printing only response bodies and minimal diagnostics to `STDERR`.
- API base override: set `PDFREST_URL` in `.env` to change regions. For EU GDPR compliance and improved performance in Europe, you may use `https://eu-api.pdfrest.com/`. More info: https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work

---

## Audience And Tone (Internal)

These samples are customer‑facing and intended to help potential customers evaluate pdfRest quickly. Keep all code, comments, and documentation clear, minimal, and task‑focused. Avoid internal jargon and advanced implementation details in customer‑visible files. Do not surface internal process notes in `README.md` or the samples.

Key points:
- Clarity: explain what the sample does in 1–2 bullets.
- Guidance: show how to set up `.env` and how to run the script.
- Region: mention optional `PDFREST_URL` with the EU endpoint for GDPR and proximity.
- Safety: never log secrets; print only response bodies and minimal diagnostics to `STDERR`.
- Errors: abort on non‑2xx with a concise message; exit non‑zero.

## Sample Header Convention (Internal)

Add this standardized header comment at the top of every Ruby sample. This header is customer‑visible but the convention itself is for us to remember here (do not duplicate these instructions in README).

Template:

```
#!
# What this sample does:
# - <One–two bullets describing purpose and request style>
#
# Setup (.env):
# - Copy .env.example to .env
# - Set PDFREST_API_KEY=your_api_key_here
# - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
#     PDFREST_URL=https://eu-api.pdfrest.com
#
# Usage:
#   ruby "<relative path to this file>" /path/to/input.pdf
#
# Output:
# - Prints the API JSON response to stdout. Non-2xx responses abort with a concise message.
# - Tip: pipe output to a file: ruby ... > response.json
```

Notes:
- Match the endpoint name and request style (JSON two‑step vs multipart single request) in the bullets.
- Keep the header concise; avoid adding options unless they are essential.
- If a sample accepts optional parameters, mention them inline in code near where they are used.

## README Scope (Internal)

Keep `README.md` focused on user setup, running samples, and high‑level background. Avoid internal conventions or meta‑process content that could confuse customers. Place internal notes and templates in `AGENTS.md` (this file).

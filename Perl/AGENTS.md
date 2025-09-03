# Repository Guidelines

## Project Structure & Module Organization
- `Endpoint Examples/JSON Payload/`: Two-step samples (upload then operate), e.g., `markdown.pl`.
- `Endpoint Examples/Multipart Payload/`: Single multipart request samples, e.g., `rasterized-pdf.pl`.
- `Complex Flow Examples/`: Multi-step workflows that chain endpoints.
- `.env.example` → copy to `.env` and set `PDFREST_API_KEY`; optional `PDFREST_URL`.
- `README.md`: Setup and usage details for this language folder.

## Build, Test, and Development Commands
- Install deps from `cpanfile` (recommended):
  - `cpanm --installdeps .`
- macOS quick setup: `make install-macos` or run `scripts/setup-macos.sh`
- Run JSON sample:
  - `perl "Endpoint Examples/JSON Payload/markdown.pl" /path/to/input.pdf`
- Run Multipart sample:
  - `perl "Endpoint Examples/Multipart Payload/rasterized-pdf.pl" /path/to/input.pdf`
- Capture output to file: append `> response.json`


## Coding Style & Naming Conventions
- Indentation: 4 spaces; enable `use strict; use warnings; use utf8;` at top.
- Naming: `snake_case` for files/variables; script names mirror endpoints (e.g., `markdown.pl`).
- HTTP: prefer `LWP::UserAgent` + `HTTP::Request::Common`.
  - Base URL: `$ENV{PDFREST_URL} // 'https://api.pdfrest.com'`.
- I/O: print API responses to STDOUT; send diagnostics to STDERR; exit non‑2xx with non‑zero status.

## Testing Guidelines
- No formal test suite required for samples. Validate by running against small, known inputs.
- Success: non‑zero exit on failures; JSON body printed to STDOUT on success.
- If adding tests, use `Test::More` under `t/` and run with `prove -lr t`.

## Commit & Pull Request Guidelines
- Commits: imperative, scoped, and reference endpoint/path (e.g., "Add multipart markdown sample").
- PRs: include what/why, run commands used for verification, expected response snippet, and linked issues.
- Avoid unrelated changes or large binaries.

## Security & Configuration Tips
- Do not commit secrets. Provide `.env.example`; load `PDFREST_API_KEY` from environment.
- Optional region override: `PDFREST_URL=https://eu-api.pdfrest.com/` for EU/GDPR routing.
- Never print API keys; rely on concise error messages. Respect proxies via `HTTPS_PROXY` when needed.

## Troubleshooting
- HTTPS support missing: install `LWP::Protocol::https` and `Mozilla::CA` (included in `cpanfile`). Re-run `cpanm --installdeps .`.
- SSL toolchain: some systems require OpenSSL dev libs (e.g., macOS: `brew install openssl`, Debian/Ubuntu: `apt-get install libssl-dev`) before `cpanm` can build `Net::SSLeay`/`IO::Socket::SSL`.

---

## Audience And Tone (Internal)

These Perl samples are customer‑facing and intended to help potential customers evaluate pdfRest quickly. Keep all code, comments, and documentation clear, minimal, and task‑focused. Avoid internal jargon and keep meta‑process notes out of `README.md` and the samples.

Key points:
- Clarity: explain what the sample does in 1–2 bullets.
- Guidance: show how to set up `.env` and how to run the script.
- Region: mention optional `PDFREST_URL` with the EU endpoint for GDPR and proximity.
- Safety: never log secrets; print only response bodies and minimal diagnostics to `STDERR`.
- Errors: exit non‑zero on non‑2xx responses with a concise message.

## Sample Header Convention (Internal)

Add this standardized header comment at the top of every Perl sample. This header is customer‑visible; the convention itself is tracked here for us (don’t place this template in `README.md`).

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
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#
# Usage:
#   perl "<relative path to this file>" /path/to/input.pdf
#
# Output:
# - Prints the API JSON response to stdout. Non-2xx responses exit with a concise message.
# - Tip: pipe output to a file: perl ... > response.json
```

Notes:
- Match the endpoint name and request style (JSON two‑step vs multipart single request) in the bullets.
- Keep the header concise; list optional parameters near where they are used in code.

## README Scope (Internal)

Keep `README.md` focused on user setup, running samples, and high‑level background. Avoid internal conventions or meta‑process content that could confuse customers. Place internal notes and templates in `AGENTS.md` (this file).

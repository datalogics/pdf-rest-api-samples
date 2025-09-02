# Repository Guidelines

## Project Structure & Module Organization
- `Endpoint Examples/JSON Payload/`: Two-step samples (upload then operate), e.g., `markdown.R`.
- `Endpoint Examples/Multipart Payload/`: Single multipart request samples, e.g., `rasterized-pdf.R`.
- `Complex Flow Examples/`: Multi-step workflows that chain endpoints.
- `README.md`: Prerequisites, setup (.Renviron), and how to run samples.

## Build, Test, and Development Commands
- Run JSON sample: `Rscript "Endpoint Examples/JSON Payload/markdown.R" /path/to/input.pdf`
- Run Multipart sample: `Rscript "Endpoint Examples/Multipart Payload/rasterized-pdf.R" /path/to/input.pdf`
- Pipe output to file: `... > response.json`
- Lint code: `Rscript -e "lintr::lint_dir('.')"`
- Auto-format: `Rscript -e "styler::style_dir('.')"`

## Coding Style & Naming Conventions
- Indentation: 2 spaces; 80–100 col soft limit.
- Naming: `snake_case` for variables/functions; script names mirror endpoints (e.g., `markdown.R`).
- HTTP: use explicit headers; base URL from `Sys.getenv('PDFREST_URL', 'https://api.pdfrest.com')`.
- Errors: `quit(status = 1)` on non-2xx; print concise diagnostics to stderr, response to stdout.

## Output Conventions
- Final output: write the API response/body to stdout only.
- Diagnostics: write prompts, progress, and errors to stderr.
- Exit: return non-zero status on failures; zero on success.

## Testing Guidelines
- No formal test suite yet. Validate by running samples on known inputs you provide locally.
- Success criteria: non-zero exit on failures; JSON printed to stdout on success.
- When adding tests: use `testthat` under `tests/testthat/` (e.g., `test-markdown.R`); mock HTTP; keep deterministic.

## Commit & Pull Request Guidelines
- Commits: imperative mood and focused scope (e.g., `feat: add multipart markdown sample`).
- PRs: clear description, run commands used for verification, expected response snippet, and linked issues.
- Ensure lint and formatting pass; avoid unrelated changes or large binaries.

## Security & Configuration Tips
- Secrets: do not commit. Use `.Renviron` (project-local) or `~/.Renviron`; provide `.Renviron.example`.
- Required vars:
  - `PDFREST_API_KEY=...` (required)
  - `PDFREST_URL=https://api.pdfrest.com` (optional; EU: `https://eu-api.pdfrest.com/`)
- Load with `Sys.getenv()`; never print keys. Prefer printing only response bodies.
- Proxy support via standard env vars (e.g., `HTTPS_PROXY`) when needed.

## README Guidance
- Include prerequisites (R version), setup steps for `.Renviron`, and Quick Start with both JSON and Multipart examples.
- Document environment variables, EU endpoint option, and how to capture output to files.
- Explain folder layout and how script names map to pdfRest endpoints.

---

## Audience And Tone (Internal)

These R samples are customer‑facing and meant to help potential customers evaluate pdfRest quickly. Keep code, comments, and docs simple and task‑focused. Do not include internal notes/templates in `README.md` or samples.

Key points:
- Clarity: summarize what a sample does in 1–2 bullets.
- Guidance: show how to set up `.Renviron` and how to run with `Rscript`.
- Region: mention optional `PDFREST_URL` with the EU endpoint for GDPR and proximity.
- Safety: never log secrets; print response bodies to stdout and minimal diagnostics to stderr.
- Errors: exit non‑zero on non‑2xx with a concise message.

## Sample Header Convention (Internal)

Add this standardized header comment at the top of every R sample. This header is customer‑visible; the convention lives here for us.

Template:

```
#!
# What this sample does:
# - <One–two bullets describing purpose and request style>
#
# Setup (.Renviron):
# - Copy .Renviron.example to .Renviron
# - Set PDFREST_API_KEY=your_api_key_here
# - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
#     PDFREST_URL=https://eu-api.pdfrest.com
#
# Usage:
#   Rscript "<relative path to this file>" /path/to/input.pdf
#
# Output:
# - Prints the API JSON response to stdout. Non-2xx responses quit with a concise message.
# - Tip: pipe output to a file: Rscript ... > response.json
```

Notes:
- Match the endpoint name and request style (JSON two‑step vs multipart single request) in the bullets.
- Keep headers concise; mention optional parameters inline near where they are used.

## README Scope (Internal)

Keep `README.md` focused on user setup, running samples, and high‑level background. Avoid internal conventions or meta‑process content in README; place those here in `AGENTS.md`.

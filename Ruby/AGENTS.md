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
- HTTP: use `Faraday` with `:retry` (short, limited retries). Prefer explicit headers and content types.
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

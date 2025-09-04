## pdfRest R Samples

A small set of R scripts that mirror the Ruby examples and demonstrate calling the pdfRest API. Samples are organized by request style: JSON payload (upload then operate) and Multipart payload (single request). Complex flows chain multiple endpoints.

## Quick Start
- Install R packages:
  - One-shot: `Rscript -e "install.packages(c('httr','jsonlite'), repos='https://cloud.r-project.org')"`
  - Or run: `Rscript requirements.R` (adds linters and test libs too)
- Configure environment:
  - Copy `.Renviron.example` → `.Renviron`
  - Set `PDFREST_API_KEY`, optionally `PDFREST_URL` (defaults to `https://api.pdfrest.com`)
- Run a sample:
  - JSON: `Rscript "Endpoint Examples/JSON Payload/markdown.R" /path/to/input.pdf > response.json`
  - Multipart: `Rscript "Endpoint Examples/Multipart Payload/markdown.R" /path/to/input.pdf > response.json`
  - Complex flow: `Rscript "Complex Flow Examples/merge-different-file-types.R" file1.png file2.pptx file3.pdf > response.json`

## Output & Errors
- Final API response is written to stdout.
- Diagnostics and errors go to stderr.
- Scripts exit non-zero on failures.

## Project Structure
- `Endpoint Examples/JSON Payload/` — two-step (upload + JSON)
- `Endpoint Examples/Multipart Payload/` — single multipart request
- `Complex Flow Examples/` — multi-endpoint workflows

## Lint & Format
- Lint all samples: `Rscript -e "lintr::lint_dir('.')"`
- Lint just example folders: `Rscript -e "lintr::lint_dir('Endpoint Examples'); lintr::lint_dir('Complex Flow Examples')"`
- Auto-format: `Rscript -e "styler::style_dir('.')"`
- Tip: run formatting in a clean working tree to review diffs.

## Notes
- EU/GDPR endpoint: set `PDFREST_URL=https://eu-api.pdfrest.com/` in `.Renviron` if desired.
- Keep local `.Renviron` untracked. Never print or commit secrets.
- Optional tools: `lintr`, `styler`, `testthat` (install via `requirements.R`).

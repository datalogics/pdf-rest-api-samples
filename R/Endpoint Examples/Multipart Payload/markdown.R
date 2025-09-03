#!
# What this sample does:
# - Converts a PDF to Markdown using pdfRest.
# - Sends a single multipart/form-data request directly to /markdown with the file.
#
# Setup (.Renviron):
# - Copy .Renviron.example to .Renviron (R folder root)
# - Set PDFREST_API_KEY=your_api_key_here
# - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
#     PDFREST_URL=https://eu-api.pdfrest.com
#
# Usage:
#   Rscript "Endpoint Examples/Multipart Payload/markdown.R" /path/to/input.pdf
#
# Output:
# - Prints the API JSON response to stdout. Non-2xx responses quit with a concise message.
# - Tip: pipe output to a file: Rscript ... > response.json

suppressWarnings(suppressMessages({
  if (!requireNamespace("httr", quietly = TRUE)) stop("Please install 'httr' package")
}))

stderrf <- function(...) cat(sprintf(...), file = stderr())

api_key <- Sys.getenv("PDFREST_API_KEY", unset = "")
if (identical(api_key, "")) {
  stderrf("Missing PDFREST_API_KEY in environment (.Renviron or shell)\n")
  quit(status = 1)
}

api_base <- sub("/+$$", "", Sys.getenv("PDFREST_URL", unset = "https://api.pdfrest.com"))

args <- commandArgs(trailingOnly = TRUE)
pdf_path <- args[1]
if (is.null(pdf_path) || !file.exists(pdf_path)) {
  stderrf("Usage: Rscript markdown.R /path/to/file.pdf\n")
  quit(status = 1)
}

filename <- basename(pdf_path)

tryCatch({
  conn_url <- paste0(api_base, "/markdown")

  # Build multipart form body. httr sets multipart/form-data with boundary.
  body <- list(
    file = httr::upload_file(pdf_path, type = "application/pdf"),
    output = "pdfrest_markdown",
    page_break_comments = "on"
    # Optional parameters:
    # page_range = "1-3"
  )

  resp <- httr::POST(
    conn_url,
    httr::add_headers("api-key" = api_key),
    body = body,
    encode = "multipart"
  )

  txt <- httr::content(resp, as = "text", encoding = "UTF-8")
  cat(txt)
  if (httr::http_error(resp)) {
    stop(sprintf("Markdown conversion failed with status %s", httr::status_code(resp)))
  }

}, error = function(e) {
  stderrf("Error: %s: %s\n", class(e)[1], conditionMessage(e))
  quit(status = 1)
})

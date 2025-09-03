#!
# What this sample does:
# - Converts a PDF to Markdown using pdfRest.
# - Uses a JSON payload in two steps: upload to /upload, then call /markdown with the returned id.
#
# Setup (.Renviron):
# - Copy .Renviron.example to .Renviron (R folder root)
# - Set PDFREST_API_KEY=your_api_key_here
# - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
#     PDFREST_URL=https://eu-api.pdfrest.com
#
# Usage:
#   Rscript "Endpoint Examples/JSON Payload/markdown.R" /path/to/input.pdf
#
# Output:
# - Prints the API JSON response to stdout. Non-2xx responses quit with a concise message.
# - Tip: pipe output to a file: Rscript ... > response.json

suppressWarnings(suppressMessages({
  if (!requireNamespace("httr", quietly = TRUE)) stop("Please install 'httr' package")
  if (!requireNamespace("jsonlite", quietly = TRUE)) stop("Please install 'jsonlite' package")
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
file_bytes <- readBin(pdf_path, what = "raw", n = file.info(pdf_path)$size)

tryCatch(
  {
    # Step 1: Upload the file to receive a reusable id
    upload_url <- paste0(api_base, "/upload")
    upload_resp <- httr::POST(
      upload_url,
      httr::add_headers(
        "api-key" = api_key,
        "content-filename" = filename,
        "Content-Type" = "application/octet-stream"
      ),
      body = file_bytes
    )

    upload_text <- httr::content(upload_resp, as = "text", encoding = "UTF-8")
    message(upload_text)
    if (httr::http_error(upload_resp)) {
      stop(sprintf("Upload failed with status %s", httr::status_code(upload_resp)))
    }

    upload_json <- jsonlite::fromJSON(upload_text)
    # jsonlite parses arrays of objects as a data.frame; extract first id safely
    uploaded_id <- if (is.data.frame(upload_json$files)) upload_json$files$id[[1]] else upload_json$files[[1]]$id
    message(sprintf("Successfully uploaded with an id of: %s", uploaded_id))

    # Step 2: Request Markdown output using the uploaded id
    md_url <- paste0(api_base, "/markdown")
    body <- jsonlite::toJSON(list(id = uploaded_id, page_break_comments = "on"), auto_unbox = TRUE)
    md_resp <- httr::POST(
      md_url,
      httr::add_headers(
        "api-key" = api_key,
        "Content-Type" = "application/json"
      ),
      body = body
    )

    md_text <- httr::content(md_resp, as = "text", encoding = "UTF-8")
    cat(md_text)
    if (httr::http_error(md_resp)) {
      stop(sprintf("Markdown conversion failed with status %s", httr::status_code(md_resp)))
    }
  },
  error = function(e) {
    stderrf("Error: %s: %s\n", class(e)[1], conditionMessage(e))
    quit(status = 1)
  }
)

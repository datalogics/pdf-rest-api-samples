#!
# Summarize a PDF using a single multipart/form-data request to /summarized-pdf-text.

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
  stderrf("Usage: Rscript summarized-pdf-text.R /path/to/file.pdf\n")
  quit(status = 1)
}

filename <- basename(pdf_path)

tryCatch({
  conn_url <- paste0(api_base, "/summarized-pdf-text")
  body <- list(
    file = httr::upload_file(pdf_path, type = "application/pdf"),
    target_word_count = "100"
  )
  resp <- httr::POST(
    conn_url,
    httr::add_headers("api-key" = api_key),
    body = body,
    encode = "multipart"
  )
  txt <- httr::content(resp, as = "text", encoding = "UTF-8")
  cat(txt)
  if (httr::http_error(resp)) stop(sprintf("Summarize failed with status %s", httr::status_code(resp)))
}, error = function(e) {
  stderrf("Error: %s: %s\n", class(e)[1], conditionMessage(e))
  quit(status = 1)
})


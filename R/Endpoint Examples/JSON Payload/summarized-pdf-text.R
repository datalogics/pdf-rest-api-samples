#!
# Summarize a PDF using two-step JSON flow: upload then /summarized-pdf-text.

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
  stderrf("Usage: Rscript summarized-pdf-text.R /path/to/file.pdf\n")
  quit(status = 1)
}

filename <- basename(pdf_path)
file_bytes <- readBin(pdf_path, what = "raw", n = file.info(pdf_path)$size)

tryCatch({
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
  if (httr::http_error(upload_resp)) stop(sprintf("Upload failed with status %s", httr::status_code(upload_resp)))

  upload_json <- jsonlite::fromJSON(upload_text)
  uploaded_id <- if (is.data.frame(upload_json$files)) upload_json$files$id[[1]] else upload_json$files[[1]]$id
  message(sprintf("Successfully uploaded with an id of: %s", uploaded_id))

  url <- paste0(api_base, "/summarized-pdf-text")
  body <- jsonlite::toJSON(list(id = uploaded_id, target_word_count = 100), auto_unbox = TRUE)
  resp <- httr::POST(
    url,
    httr::add_headers("api-key" = api_key, "Content-Type" = "application/json"),
    body = body
  )
  txt <- httr::content(resp, as = "text", encoding = "UTF-8")
  cat(txt)
  if (httr::http_error(resp)) stop(sprintf("Summarize failed with status %s", httr::status_code(resp)))
}, error = function(e) {
  stderrf("Error: %s: %s\n", class(e)[1], conditionMessage(e))
  quit(status = 1)
})


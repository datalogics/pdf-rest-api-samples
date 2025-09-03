#!
# What this sample does:
# - Merges multiple inputs (PDFs and non-PDFs) into a single PDF.
# - Non-PDFs are converted to PDF; PDFs are uploaded. Collected IDs are merged via /merged-pdf.
#
# Setup (.Renviron):
# - Copy .Renviron.example to .Renviron (R folder root)
# - Set PDFREST_API_KEY=your_api_key_here
# - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
#     PDFREST_URL=https://eu-api.pdfrest.com
#
# Usage:
#   Rscript "Complex Flow Examples/merge-different-file-types.R" /path/to/file1 /path/to/file2 [/path/to/file3 ...]
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
if (length(args) < 2 || any(!file.exists(args))) {
  stderrf("Usage: Rscript merge-different-file-types.R /path/to/file1 /path/to/file2 [/path/to/file3 ... ]\n")
  quit(status = 1)
}

content_type_for <- function(path) {
  ext <- tolower(tools::file_ext(path))
  switch(ext,
    pdf = "application/pdf",
    png = "image/png",
    jpg = "image/jpeg", jpeg = "image/jpeg",
    gif = "image/gif",
    tif = "image/tiff", tiff = "image/tiff",
    bmp = "image/bmp",
    webp = "image/webp",
    doc = "application/msword",
    docx = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
    ppt = "application/vnd.ms-powerpoint",
    pptx = "application/vnd.openxmlformats-officedocument.presentationml.presentation",
    xls = "application/vnd.ms-excel",
    xlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
    txt = "text/plain",
    rtf = "application/rtf",
    html = "text/html", htm = "text/html",
    "application/octet-stream"
  )
}

tryCatch({
  collected_ids <- character(0)

  for (i in seq_along(args)) {
    p <- args[[i]]
    ext <- tolower(tools::file_ext(p))
    if (ext == "pdf") {
      # Upload PDF to get id
      upload_url <- paste0(api_base, "/upload")
      upload_resp <- httr::POST(
        upload_url,
        httr::add_headers(
          "api-key" = api_key,
          "content-filename" = basename(p),
          "Content-Type" = "application/octet-stream"
        ),
        body = readBin(p, what = "raw", n = file.info(p)$size)
      )
      txt <- httr::content(upload_resp, as = "text", encoding = "UTF-8")
      message(txt)
      if (httr::http_error(upload_resp)) {
        stop(sprintf("Upload failed for input #%d with status %s", i, httr::status_code(upload_resp)))
      }
      up_json <- jsonlite::fromJSON(txt)
      collected_ids <- c(collected_ids, if (is.data.frame(up_json$files)) up_json$files$id[[1]] else up_json$files[[1]]$id)
      message(sprintf("Uploaded PDF (#%d); id=%s", i, tail(collected_ids, 1)))
    } else {
      # Convert to PDF via /pdf to get outputId
      conv_url <- paste0(api_base, "/pdf")
      body <- list(file = httr::upload_file(p, type = content_type_for(p)))
      conv_resp <- httr::POST(conv_url, httr::add_headers("api-key" = api_key), body = body, encode = "multipart")
      txt <- httr::content(conv_resp, as = "text", encoding = "UTF-8")
      message(txt)
      if (httr::http_error(conv_resp)) {
        stop(sprintf("Conversion failed for input #%d with status %s", i, httr::status_code(conv_resp)))
      }
      cv_json <- jsonlite::fromJSON(txt)
      collected_ids <- c(collected_ids, cv_json$outputId)
      message(sprintf("Converted non-PDF (#%d); outputId=%s", i, tail(collected_ids, 1)))
    }
  }

  # Build x-www-form-urlencoded merge body
  enc <- function(x) utils::URLencode(x, reserved = TRUE)
  parts <- character(0)
  for (id in collected_ids) {
    parts <- c(parts, paste0("id[]=", enc(id)))
    parts <- c(parts, paste0("pages[]=", enc("1-last")))
    parts <- c(parts, paste0("type[]=", enc("id")))
  }
  merge_body <- paste(parts, collapse = "&")

  merge_url <- paste0(api_base, "/merged-pdf")
  merge_resp <- httr::POST(
    merge_url,
    httr::add_headers("api-key" = api_key, "Content-Type" = "application/x-www-form-urlencoded"),
    body = merge_body
  )

  merge_text <- httr::content(merge_resp, as = "text", encoding = "UTF-8")
  cat(merge_text)
  if (httr::http_error(merge_resp)) {
    stop(sprintf("Merge failed with status %s", httr::status_code(merge_resp)))
  }

}, error = function(e) {
  stderrf("Error: %s: %s\n", class(e)[1], conditionMessage(e))
  quit(status = 1)
})

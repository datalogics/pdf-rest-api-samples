#! 
# What this sample does:
# - Calls /blank-pdf via multipart/form-data to create a blank three-page PDF.
#
# Setup (.Renviron):
# - Copy .Renviron.example to .Renviron (R folder root)
# - Set PDFREST_API_KEY=your_api_key_here
# - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
#     PDFREST_URL=https://eu-api.pdfrest.com
#   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#
# Usage:
#   Rscript "Endpoint Examples/Multipart Payload/blank-pdf.R"
#
# Output:
# - Prints the API JSON response to stdout. Non-2xx responses quit with a concise message.

suppressWarnings(suppressMessages({
  if (!requireNamespace("httr", quietly = TRUE)) stop("Please install 'httr' package")
}))

stderrf <- function(...) cat(sprintf(...), file = stderr())

api_key <- Sys.getenv("PDFREST_API_KEY", unset = "")
if (identical(api_key, "")) {
  stderrf("Missing PDFREST_API_KEY in environment (.Renviron or shell)\n")
  quit(status = 1)
}

api_base <- sub("/+$", "", Sys.getenv("PDFREST_URL", unset = "https://api.pdfrest.com"))

body <- list(
  page_size = "letter",
  page_count = "3",
  page_orientation = "portrait"
)

resp <- httr::POST(
  paste0(api_base, "/blank-pdf"),
  httr::add_headers("api-key" = api_key),
  body = body,
  encode = "multipart"
)

txt <- httr::content(resp, as = "text", encoding = "UTF-8")
cat(txt)

if (httr::http_error(resp)) {
  stderrf("\nblank-pdf request failed with status %s\n", httr::status_code(resp))
  quit(status = 1)
}

#! 
# What this sample does:
# - Calls /blank-pdf with a JSON payload to create an empty three-page PDF.
#
# Setup (.Renviron):
# - Copy .Renviron.example to .Renviron (R folder root)
# - Set PDFREST_API_KEY=your_api_key_here
# - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
#     PDFREST_URL=https://eu-api.pdfrest.com
#   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#
# Usage:
#   Rscript "Endpoint Examples/JSON Payload/blank-pdf.R"
#
# Output:
# - Prints the API JSON response to stdout. Non-2xx responses quit with a concise message.

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

api_base <- sub("/+$", "", Sys.getenv("PDFREST_URL", unset = "https://api.pdfrest.com"))

payload <- jsonlite::toJSON(
  list(page_size = "letter", page_count = 3, page_orientation = "portrait"),
  auto_unbox = TRUE
)

resp <- httr::POST(
  paste0(api_base, "/blank-pdf"),
  httr::add_headers(
    "api-key" = api_key,
    "Content-Type" = "application/json"
  ),
  body = payload
)

body_text <- httr::content(resp, as = "text", encoding = "UTF-8")
cat(body_text)

if (httr::http_error(resp)) {
  stderrf("\nblank-pdf request failed with status %s\n", httr::status_code(resp))
  quit(status = 1)
}

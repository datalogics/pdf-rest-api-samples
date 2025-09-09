#!/bin/sh

# By default, we use the US-based API service. This is the primary endpoint for global use.
API_URL="https://api.pdfrest.com"

# For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
# API_URL = "https://eu-api.pdfrest.com"

UPLOAD_PDF_FILE_ID=$(curl --location "$API_URL/upload" \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'content-filename: filename.pdf' \
--data-binary '@/path/to/pdf_file' \
 | jq -r '.files.[0].id')

 echo "PDF file successfully uploaded with an ID of: $UPLOAD_PDF_FILE_ID"

UPLOAD_DATA_FILE_ID=$(curl --location "$API_URL/upload" \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'content-filename: filename.xml' \
--data-binary '@/path/to/data_file' \
| jq -r '.files.[0].id')

echo "Data file successfully uploaded with an ID of: $UPLOAD_DATA_FILE_ID"

curl "$API_URL/pdf-with-imported-form-data" \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'Content-Type: application/json' \
--data-raw "{ \"id\": \"$UPLOAD_PDF_FILE_ID\", \"data_file_id\": \"$UPLOAD_DATA_FILE_ID\"}" | jq -r '.'

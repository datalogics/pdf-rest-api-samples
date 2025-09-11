#!/bin/sh

# By default, we use the US-based API service. This is the primary endpoint for global use.
API_URL="https://api.pdfrest.com"

# For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
# API_URL="https://eu-api.pdfrest.com"

UPLOAD_ID=$(curl --location "$API_URL/upload" \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'content-filename: filename.pdf' \
--data-binary '@/path/to/file' \
 | jq -r '.files.[0].id')

echo "File successfully uploaded with an ID of: $UPLOAD_ID"

BOXES='{"boxes":[{"box":"media","pages":[{"range":"1","left":100,"top":100,"bottom":100,"right":100}]}]}'

curl "$API_URL/pdf-with-page-boxes-set" \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'Content-Type: application/json' \
--data-raw "{ \"output\": \"example_out.pdf\", \"id\": \"$UPLOAD_ID\", \"boxes\": \"{\\\"boxes\\\":[{\\\"box\\\":\\\"media\\\",\\\"pages\\\":[{\\\"range\\\":\\\"1\\\",\\\"left\\\":100,\\\"top\\\":100,\\\"bottom\\\":100,\\\"right\\\":100}]}]}\" }" | jq -r '.'

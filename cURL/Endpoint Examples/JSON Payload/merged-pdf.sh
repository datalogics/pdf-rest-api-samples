#!/bin/sh

# By default, we use the US-based API service. This is the primary endpoint for global use.
API_URL="https://api.pdfrest.com"

# For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
# API_URL="https://eu-api.pdfrest.com"

UPLOAD_FIRST_FILE_ID=$(curl --location "$API_URL/upload" \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'content-filename: first_filename.pdf' \
--data-binary '@/path/to/first_file' \
 | jq -r '.files.[0].id')

 echo "First file successfully uploaded with an ID of: $UPLOAD_FIRST_FILE_ID"

UPLOAD_SECOND_FILE_ID=$(curl --location "$API_URL/upload" \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'content-filename: second_filename.pdf' \
--data-binary '@/path/to/second_file' \
| jq -r '.files.[0].id')

echo "Second file successfully uploaded with an ID of: $UPLOAD_SECOND_FILE_ID"

curl "$API_URL/merged-pdf" \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'Content-Type: application/json' \
--data-raw "{ \"id\": [\"$UPLOAD_FIRST_FILE_ID\", \"$UPLOAD_SECOND_FILE_ID\"], \"pages\":[1,1], \"type\":[\"id\", \"id\"] }" | jq -r '.'

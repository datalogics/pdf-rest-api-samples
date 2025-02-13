#!/bin/sh

UPLOAD_ID=$(curl --location 'https://api.pdfrest.com/upload' \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'content-filename: filename.pdf' \
--data-binary '@/path/to/file' \
 | jq -r '.files.[0].id')

echo "File successfully uploaded with an ID of: $UPLOAD_ID"

REDACTIONS='[{\"type\":\"preset\",\"value\":\"uuid\"},{\"type\":\"regex\",\"value\":\"(\\\\+\\\\d{1,2}\\\\s)?\\\\(?\\\\d{3}\\\\)?[\\\\s.-]\\\\d{3}[\\\\s.-]\\\\d{4}\"},{\"type\":\"literal\",\"value\":\"word\"}]'

curl 'https://api.pdfrest.com/pdf-with-redacted-text-preview' \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'Content-Type: application/json' \
--data-raw "{ \"id\": \"$UPLOAD_ID\", \"redactions\": \"$REDACTIONS\"}" | jq -r '.'

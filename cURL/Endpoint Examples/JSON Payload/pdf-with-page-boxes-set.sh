#!/bin/sh

UPLOAD_ID=$(curl --location 'https://api.pdfrest.com/upload' \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'content-filename: filename.pdf' \
--data-binary '@/path/to/file' \
 | jq -r '.files.[0].id')

echo "File successfully uploaded with an ID of: $UPLOAD_ID"

BOXES='{"boxes":[{"box":"media","pages":[{"range":"1","left":100,"top":100,"bottom":100,"right":100}]}]}'

curl 'https://api.pdfrest.com/pdf-with-page-boxes-set' \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'Content-Type: application/json' \
--data-raw "{ \"output\": \"example_out.pdf\", \"id\": \"$UPLOAD_ID\", \"boxes\": \"{\\\"boxes\\\":[{\\\"box\\\":\\\"media\\\",\\\"pages\\\":[{\\\"range\\\":\\\"1\\\",\\\"left\\\":100,\\\"top\\\":100,\\\"bottom\\\":100,\\\"right\\\":100}]}]}\" }" | jq -r '.'

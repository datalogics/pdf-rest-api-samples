#!/bin/sh

UPLOAD_ID=$(curl --location 'https://api.pdfrest.com/upload' \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'content-filename: filename.pdf' \
--data-binary '@/path/to/file' \
 | jq -r '.files.[0].id')

echo "File successfully uploaded with an ID of: $UPLOAD_ID"

REDACTED_OUTPUT=$(curl 'https://api.pdfrest.com/pdf-with-redacted-text-applied' \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'Content-Type: application/json' \
--data-raw "{ \"id\": \"$UPLOAD_ID\"}")

echo $REDACTED_OUTPUT | jq -r '.'

# All files uploaded or generated are automatically deleted based on the 
# File Retention Period as shown on https://pdfrest.com/pricing. 
# For immediate deletion of files, particularly when sensitive data 
# is involved, an explicit delete call can be made to the API.

# The following code is an optional step to delete unredacted files from pdfRest servers.

PREVIEW_PDF_ID=$(jq -r '.inputId' <<< $PREVIEW_OUTPUT)
curl --request POST "https://api.pdfrest.com/delete" \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'Content-Type: application/json' \
--data-raw "{ \"ids\": \"$UPLOAD_ID, $PREVIEW_PDF_ID\"}" | jq -r '.'
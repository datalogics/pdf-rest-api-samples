#!/bin/sh

UPLOAD_ID=$(curl --location 'https://api.pdfrest.com/upload' \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'content-filename: filename.pdf' \
--data-binary '@/path/to/file' \
 | jq -r '.files.[0].id')

echo "File successfully uploaded with an ID of: $UPLOAD_ID"

REDACTIONS='[{\"type\":\"preset\",\"value\":\"email\"},{\"type\":\"regex\",\"value\":\"(\\\\+\\\\d{1,2}\\\\s)?\\\\(?\\\\d{3}\\\\)?[\\\\s.-]\\\\d{3}[\\\\s.-]\\\\d{4}\"},{\"type\":\"literal\",\"value\":\"word\"}]'

PREVIEW_OUTPUT=$(curl 'https://api.pdfrest.com/pdf-with-redacted-text-preview' \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'Content-Type: application/json' \
--data-raw "{ \"id\": \"$UPLOAD_ID\", \"redactions\": \"$REDACTIONS\"}")

echo $PREVIEW_OUTPUT | jq -r '.'

# All files uploaded or generated are automatically deleted based on the 
# File Retention Period as shown on https://pdfrest.com/pricing. 
# For immediate deletion of files, particularly when sensitive data 
# is involved, an explicit delete call can be made to the API.

# IMPORTANT: Do not delete the PREVIEW_PDF_ID file until after the redaction is applied
# with the /pdf-with-redacted-text-applied endpoint.

PREVIEW_PDF_ID=$(jq -r '.outputId' <<< $PREVIEW_OUTPUT)
# Optional deletion step — OFF by default.
# Deletes all files in the workflow, including outputs. Save all desired files before enabling this step.
# Enable by uncommenting the next line to delete sensitive files
# DELETE_SENSITIVE_FILES=true
if [ "$DELETE_SENSITIVE_FILES" = "true" ]; then
curl --request POST "https://api.pdfrest.com/delete" \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'Content-Type: application/json' \
--data-raw "{ \"ids\": \"$UPLOAD_ID, $PREVIEW_PDF_ID\"}" | jq -r '.'
fi

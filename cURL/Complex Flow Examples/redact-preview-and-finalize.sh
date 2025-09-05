#!/bin/sh

# This sample demonstrates the workflow from unredacted document to fully
# redacted document. The output file from the preview tool is immediately
# forwarded to the finalization stage. We recommend inspecting the output from
# the preview stage before utilizing this workflow to ensure that content is
# redacted as intended.

API_KEY="xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" # place your api key here
REDACTIONS='[{"type":"regex","value":"[Tt]he"}]'
PREVIEW_OUTPUT=$(curl -X POST "https://api.pdfrest.com/pdf-with-redacted-text-preview" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: $API_KEY" \
  -F "file=@/path/to/file" \
  -F "redactions=$REDACTIONS" \
  -F "output=example_out")

PREVIEW_PDF_ID=$(jq -r '.outputId' <<< $PREVIEW_OUTPUT)

curl -X POST "https://api.pdfrest.com/pdf-with-redacted-text-applied" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: $API_KEY" \
  -F "id=$PREVIEW_PDF_ID" \
  -F "output=example_out"

# All files uploaded or generated are automatically deleted based on the 
# File Retention Period as shown on https://pdfrest.com/pricing. 
# For immediate deletion of files, particularly when sensitive data 
# is involved, an explicit delete call can be made to the API.

# Optional deletion step — OFF by default.
# Deletes sensitive files (unredacted, unwatermarked, unencrypted, or unrestricted).
# Enable by uncommenting the next line to delete sensitive files
# DELETE_SENSITIVE_FILES=true
if [ "$DELETE_SENSITIVE_FILES" = "true" ]; then
  INPUT_PDF_ID=$(jq -r '.inputId' <<< $PREVIEW_OUTPUT)
  curl -X POST "https://api.pdfrest.com/delete" \
    -H "Accept: application/json" \
    -H "Content-Type: multipart/form-data" \
    -H "Api-Key: $API_KEY" \
    -F "ids=$INPUT_PDF_ID, $PREVIEW_PDF_ID"
fi

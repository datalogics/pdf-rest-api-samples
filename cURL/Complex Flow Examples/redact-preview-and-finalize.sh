#!/bin/sh

# This sample demonstrates the workflow from unredacted document to fully
# redacted document. The output file from the preview tool is immediately
# forwarded to the finalization stage. We recommend inspecting the output from
# the preview stage before utilizing this workflow to ensure that content is
# redacted as intended.

API_KEY="xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" # place your api key here
REDACTIONS='[{"type":"regex","value":"[Tt]he"}]'
PDF_ID=$(curl -X POST "https://api.pdfrest.com/pdf-with-redacted-text-preview" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: $API_KEY" \
  -F "file=@/path/to/file" \
  -F "redactions=$REDACTIONS" \
  -F "output=example_out" \
  | jq -r '.outputId')

curl -X POST "https://api.pdfrest.com/pdf-with-redacted-text-applied" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: $API_KEY" \
  -F "id=$PDF_ID" \
  -F "output=example_out"


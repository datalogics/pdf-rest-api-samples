#!/bin/sh

API_KEY="xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"

REQUEST_ID=$(curl -X POST "https://api.pdfrest.com/pdfa" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: $API_KEY" \
  -H "response-type: requestId" \
  -F "file=@/path/to/file.pdf" \
  -F "output_type=PDF/A-3b" \
  -F "output=example_out" \
  | jq -r '.requestId')

curl -X GET "https://api.pdfrest.com/request-status/$REQUEST_ID" \
  -H "Api-Key: $API_KEY"
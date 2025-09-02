#!/bin/sh

UPLOAD_PDF_FILE_ID=$(curl --location 'https://api.pdfrest.com/upload' \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'content-filename: filename.pdf' \
--data-binary '@/path/to/file' \
 | jq -r '.files.[0].id')

 echo "PDF file successfully uploaded with an ID of: $UPLOAD_PDF_FILE_ID"


curl 'https://api.pdfrest.com/watermarked-pdf' \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'Content-Type: application/json' \
--data-raw "{ \"id\": \"$UPLOAD_PDF_FILE_ID\", \"watermark_text\": \"watermark\" }" | jq -r '.'

# All files uploaded or generated are automatically deleted based on the 
# File Retention Period as shown on https://pdfrest.com/pricing. 
# For immediate deletion of files, particularly when sensitive data 
# is involved, an explicit delete call can be made to the API.

# The following code is an optional step to delete unwatermarked files from pdfRest servers.

curl --request POST "https://api.pdfrest.com/delete" \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'Content-Type: application/json' \
--data-raw "{ \"ids\": \"$UPLOAD_PDF_FILE_ID\"}" | jq -r '.'

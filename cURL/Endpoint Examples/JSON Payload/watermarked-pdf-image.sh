#!/bin/sh
UPLOAD_PDF_FILE_ID=$(curl --location 'https://api.pdfrest.com/upload' \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'content-filename: pdf_filename.pdf' \
--data-binary '@/path/to/pdf_file' \
 | jq -r '.files.[0].id')

 echo "PDF file successfully uploaded with an ID of: $UPLOAD_PDF_FILE_ID"

UPLOAD_WATERMARK_FILE_ID=$(curl --location 'https://api.pdfrest.com/upload' \
 --header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
 --header 'content-filename: watermark_filename.pdf' \
 --data-binary '@/path/to/watermark_file' \
| jq -r '.files.[0].id')

echo "Watermark file successfully uploaded with an ID of: $UPLOAD_WATERMARK_FILE_ID"

WATERMARKED_OUTPUT=$(curl 'https://api.pdfrest.com/watermarked-pdf' \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'Content-Type: application/json' \
--data-raw "{ \"id\": \"$UPLOAD_PDF_FILE_ID\", \"watermark_file_id\": \"$UPLOAD_WATERMARK_FILE_ID\" }")

echo $WATERMARKED_OUTPUT | jq -r '.'

# All files uploaded or generated are automatically deleted based on the 
# File Retention Period as shown on https://pdfrest.com/pricing. 
# For immediate deletion of files, particularly when sensitive data 
# is involved, an explicit delete call can be made to the API.

# Optional deletion step — OFF by default.
# Deletes all files in the workflow, including outputs. Save all desired files before enabling this step.
# Enable by uncommenting the next line to delete sensitive files
# DELETE_SENSITIVE_FILES=true
if [ "$DELETE_SENSITIVE_FILES" = "true" ]; then
WATERMARKED_OUTPUT_ID=$(jq -r '.outputId' <<< $WATERMARKED_OUTPUT)
curl --request POST "https://api.pdfrest.com/delete" \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'Content-Type: application/json' \
--data-raw "{ \"ids\": \"$UPLOAD_PDF_FILE_ID, $UPLOAD_WATERMARK_FILE_ID, $WATERMARKED_OUTPUT_ID\"}" | jq -r '.'
fi

#!/bin/sh

UPLOAD_PDF_FILE_ID=$(curl --location 'https://api.pdfrest.com/upload' \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'content-filename: filename.pdf' \
--data-binary '@/path/to/pdf_file' \
 | jq -r '.files.[0].id')

echo "PDF file successfully uploaded with an ID of: $UPLOAD_PDF_FILE_ID"

UPLOAD_ATTACHMENT_FILE_ID=$(curl --location 'https://api.pdfrest.com/upload' \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'content-filename: filename.xml' \
--data-binary '@/path/to/attachment_file' \
| jq -r '.files.[0].id')

echo "Attachment file successfully uploaded with an ID of: $UPLOAD_ATTACHMENT_FILE_ID"

curl 'https://api.pdfrest.com/pdf-with-added-attachment' \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'Content-Type: application/json' \
--data-raw "{ \"id\": \"$UPLOAD_PDF_FILE_ID\", \"id_to_attach\": \"$UPLOAD_ATTACHMENT_FILE_ID\"}" | jq -r '.'

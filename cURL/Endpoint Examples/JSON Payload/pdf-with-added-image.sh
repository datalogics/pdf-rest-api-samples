#!/bin/sh

UPLOAD_PDF_FILE_ID=$(curl --location 'https://api.pdfrest.com/upload' \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'content-filename: filename.pdf' \
--data-binary '@/path/to/pdf_file' \
 | jq -r '.files.[0].id')

echo "PDF file successfully uploaded with an ID of: $UPLOAD_PDF_FILE_ID"

UPLOAD_IMAGE_FILE_ID=$(curl --location 'https://api.pdfrest.com/upload' \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'content-filename: filename.png' \
--data-binary '@/path/to/image_file' \
| jq -r '.files.[0].id')

echo "Image file successfully uploaded with an ID of: $UPLOAD_IMAGE_FILE_ID"

curl 'https://api.pdfrest.com/pdf-with-added-image' \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'Content-Type: application/json' \
--data-raw "{ \"id\": \"$UPLOAD_PDF_FILE_ID\", \"image_id\": \"$UPLOAD_IMAGE_FILE_ID\", \"page\":1, \"x\":0, \"y\":0 }" | jq -r '.'

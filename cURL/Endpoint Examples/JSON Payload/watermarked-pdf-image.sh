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

curl 'https://api.pdfrest.com/watermarked-pdf' \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'Content-Type: application/json' \
--data-raw "{ \"id\": \"$UPLOAD_PDF_FILE_ID\", \"watermark_file_id\": \"$UPLOAD_WATERMARK_FILE_ID\" }" | jq -r '.'

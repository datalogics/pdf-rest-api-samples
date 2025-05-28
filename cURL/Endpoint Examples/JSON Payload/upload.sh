#!/bin/sh

UPLOAD_ID=$(curl --location 'https://api.pdfrest.com/upload' \
--header 'api-key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'content-filename: filename.pdf' \
--data-binary '@/path/to/file' \
 | jq -r '.files.[0].id')

echo "File successfully uploaded with an ID of: $UPLOAD_ID"

#!/bin/sh

UPLOAD_FIRST_FILE_ID=$(curl --location 'https://api.pdfrest.com/upload' \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'content-filename: first_filename.pdf' \
--data-binary '@/path/to/first_file' \
 | jq -r '.files.[0].id')

 echo "First file successfully uploaded with an ID of: $UPLOAD_FIRST_FILE_ID"

UPLOAD_SECOND_FILE_ID=$(curl --location 'https://api.pdfrest.com/upload' \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'content-filename: second_filename.pdf' \
--data-binary '@/path/to/second_file' \
| jq -r '.files.[0].id')

echo "Second file successfully uploaded with an ID of: $UPLOAD_SECOND_FILE_ID"

curl 'https://api.pdfrest.com/merged-pdf' \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'Content-Type: application/json' \
--data-raw "{ \"id\": [\"$UPLOAD_FIRST_FILE_ID\", \"$UPLOAD_SECOND_FILE_ID\"], \"pages\":[1,1], \"type\":[\"id\", \"id\"] }" | jq -r '.'

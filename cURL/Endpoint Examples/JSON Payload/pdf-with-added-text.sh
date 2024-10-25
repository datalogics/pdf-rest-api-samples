#!/bin/sh

UPLOAD_ID=$(curl --location 'https://api.pdfrest.com/upload' \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'content-filename: filename.pdf' \
--data-binary '@/path/to/file' \
 | jq -r '.files.[0].id')

echo "File successfully uploaded with an ID of: $UPLOAD_ID"

TEXT_OPTIONS='[{\"font\":\"Times New Roman\",\"max_width\":\"175\",\"opacity\":\"1\",\"page\":\"1\",\"rotation\":\"0\",\"text\":\"sample text in PDF\",\"text_color_rgb\":\"0,0,0\",\"text_size\":\"30\",\"x\":\"72\",\"y\":\"144\"}]'

curl 'https://api.pdfrest.com/pdf-with-added-text' \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'Content-Type: application/json' \
--data-raw "{ \"id\": \"$UPLOAD_ID\", \"text_objects\": \"$TEXT_OPTIONS\"}" | jq -r '.'

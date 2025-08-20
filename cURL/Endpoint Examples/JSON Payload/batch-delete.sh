#!/bin/sh
curl --request POST "https://api.pdfrest.com/delete" \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'Content-Type: application/json' \
--data-raw "{ \"ids\": \"xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx, xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx\"}" | jq -r '.'

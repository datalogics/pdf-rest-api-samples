curl 'https://api.pdfrest.com/pdf' \
--header 'Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' \
--header 'Content-Type: application/json' \
--data-raw '{ "url": "https://pdfrest.com/"}' | jq -r '.'

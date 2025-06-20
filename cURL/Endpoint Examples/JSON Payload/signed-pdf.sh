#!/bin/sh

API_KEY=xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx

PDF_ID=$(curl --location 'https://api.pdfrest.com/upload' \
--header "Api-Key: $API_KEY" \
--header 'content-filename: input.pdf' \
--data-binary '@/path/to/input.pdf' \
 | jq -r '.files.[0].id')

echo "PDF successfully uploaded with an ID of: $PDF_ID"

CREDS_ID=$(curl --location 'https://api.pdfrest.com/upload' \
--header "Api-Key: $API_KEY" \
--header 'content-filename: credentials.pfx' \
--data-binary '@/path/to/credentials.pfx' \
 | jq -r '.files.[0].id')

echo "Credential file successfully uploaded with an ID of: $CREDS_ID"

PASSPHRASE_ID=$(curl --location 'https://api.pdfrest.com/upload' \
--header "Api-Key: $API_KEY" \
--header 'content-filename: passphrase.txt' \
--data-binary '@/path/to/passphrase.txt' \
 | jq -r '.files.[0].id')

echo "Passphrase file successfully uploaded with an ID of: $PASSPHRASE_ID"

LOGO_ID=$(curl --location 'https://api.pdfrest.com/upload' \
--header "Api-Key: $API_KEY" \
--header 'content-filename: logo.png' \
--data-binary '@/path/to/logo.png' \
 | jq -r '.files.[0].id')

echo "Logo image successfully uploaded with an ID of: $LOGO_ID"

SIGNATURE_CONFIG='{\"type\": \"new\",\"name\": \"esignature\",\"logo_opacity\": \"0.25\",\"location\": {\"bottom_left\": { \"x\": \"0\", \"y\": \"0\" },\"top_right\": { \"x\": \"216\", \"y\": \"72\" },\"page\": 1},\"display\": {\"include_distinguished_name\": \"true\",\"include_datetime\": \"true\",\"contact\": \"My contact info\",\"location\": \"My location\",\"name\": \"John Doe\",\"reason\": \"My reason for signing\"}}'

curl 'https://api.pdfrest.com/signed-pdf' \
--header "Api-Key: $API_KEY" \
--header 'Content-Type: application/json' \
--data-raw "{ \"id\": \"$PDF_ID\", \"pfx_credential_id\": \"$CREDS_ID\", \"pfx_passphrase_id\": \"$PASSPHRASE_ID\", \"logo_id\": \"$LOGO_ID\", \"signature_configuration\": \"$SIGNATURE_CONFIG\"}" | jq -r '.'

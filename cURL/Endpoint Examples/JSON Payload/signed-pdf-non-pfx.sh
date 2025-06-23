#!/bin/sh

API_KEY=xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx

PDF_ID=$(curl --location 'https://api.pdfrest.com/upload' \
--header "Api-Key: $API_KEY" \
--header 'content-filename: input.pdf' \
--data-binary '@/path/to/input.pdf' \
 | jq -r '.files.[0].id')

echo "PDF successfully uploaded with an ID of: $PDF_ID"

CERT_ID=$(curl --location 'https://api.pdfrest.com/upload' \
--header "Api-Key: $API_KEY" \
--header 'content-filename: certificate.pem' \
--data-binary '@/path/to/certificate.pem' \
 | jq -r '.files.[0].id')

echo "Certificate file successfully uploaded with an ID of: $CREDS_ID"

KEY_ID=$(curl --location 'https://api.pdfrest.com/upload' \
--header "Api-Key: $API_KEY" \
--header 'content-filename: private_key.pem' \
--data-binary '@/path/to/private_key.pem' \
 | jq -r '.files.[0].id')

echo "Key file successfully uploaded with an ID of: $PASSPHRASE_ID"

SIGNATURE_CONFIG='{\"type\": \"new\",\"name\": \"esignature\",\"location\": {\"bottom_left\": { \"x\": \"0\", \"y\": \"0\" },\"top_right\": { \"x\": \"216\", \"y\": \"72\" },\"page\": 1},\"display\": {\"include_datetime\": \"true\"}}'

curl 'https://api.pdfrest.com/signed-pdf' \
--header "Api-Key: $API_KEY" \
--header 'Content-Type: application/json' \
--data-raw "{ \"id\": \"$PDF_ID\", \"certificate_id\": \"$CERT_ID\", \"private_key_id\": \"$KEY_ID\", \"signature_configuration\": \"$SIGNATURE_CONFIG\"}" | jq -r '.'

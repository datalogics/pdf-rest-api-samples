# By default, we use the US-based API service. This is the primary endpoint for global use.
API_URL="https://api.pdfrest.com"

# For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
# API_URL="https://eu-api.pdfrest.com"

SIGNATURE_CONFIG='{"type": "new","name": "esignature","location": {"bottom_left": { "x": "0", "y": "0" },"top_right": { "x": "216", "y": "72" },"page": 1},"display": {"include_datetime": "true"}}'

curl -X POST "$API_URL/signed-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@/path/to/file" \
  -F "certificate_file=@/path/to/file" \
  -F "private_key_file=@/path/to/file" \
  -F "signature_configuration=$SIGNATURE_CONFIG" \
  -F "output=example_out"

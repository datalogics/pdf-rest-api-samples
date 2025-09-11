# By default, we use the US-based API service. This is the primary endpoint for global use.
API_URL="https://api.pdfrest.com"

# For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
# API_URL="https://eu-api.pdfrest.com"

DECRYPTED_OUTPUT=$(curl -X POST "$API_URL/decrypted-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@/path/to/file" \
  -F "output=example_out" \
  -F "current_open_password=password")

echo $DECRYPTED_OUTPUT | jq -r '.'

# All files uploaded or generated are automatically deleted based on the
# File Retention Period as shown on https://pdfrest.com/pricing.
# For immediate deletion of files, particularly when sensitive data
# is involved, an explicit delete call can be made to the API.

# Optional deletion step — OFF by default.
# Deletes all files in the workflow, including outputs. Save all desired files before enabling this step.
# Enable by uncommenting the next line to delete sensitive files
# DELETE_SENSITIVE_FILES=true
if [ "$DELETE_SENSITIVE_FILES" = "true" ]; then
  DECRYPTED_ID=$(jq -r '.outputId' <<< $DECRYPTED_OUTPUT)
  INPUT_PDF_ID=$(jq -r '.inputId' <<< $DECRYPTED_OUTPUT)
  curl -X POST "$API_URL/delete" \
    -H "Accept: application/json" \
    -H "Content-Type: multipart/form-data" \
    -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
    -F "ids=$INPUT_PDF_ID, $DECRYPTED_ID" | jq -r '.'
fi

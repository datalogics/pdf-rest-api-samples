REDACTIONS='[{"type":"preset","value":"email"},{"type":"regex","value":"(\\+\\d{1,2}\\s)?\\(?\\d{3}\\)?[\\s.-]\\d{3}[\\s.-]\\d{4}"},{"type":"literal","value":"word"}]'

PREVIEW_OUTPUT=$(curl -X POST "https://api.pdfrest.com/pdf-with-redacted-text-preview" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@/path/to/file" \
  -F "redactions=$REDACTIONS" \
  -F "output=example_out")

echo $PREVIEW_OUTPUT | jq -r '.'

# All files uploaded or generated are automatically deleted based on the 
# File Retention Period as shown on https://pdfrest.com/pricing. 
# For immediate deletion of files, particularly when sensitive data 
# is involved, an explicit delete call can be made to the API.

# IMPORTANT: Do not delete the PREVIEW_PDF_ID file until after the redaction is applied
# with the /pdf-with-redacted-text-applied endpoint.

# Optional deletion step — OFF by default.
# Deletes all files in the workflow, including outputs. Save all desired files before enabling this step.
# Enable by uncommenting the next line to delete sensitive files
# DELETE_SENSITIVE_FILES=true

if [ "$DELETE_SENSITIVE_FILES" = "true" ]; then
  INPUT_PDF_ID=$(jq -r '.inputId' <<< $PREVIEW_OUTPUT)
  PREVIEW_PDF_ID=$(jq -r '.outputId' <<< $PREVIEW_OUTPUT)
  curl -X POST "https://api.pdfrest.com/delete" \
    -H "Accept: application/json" \
    -H "Content-Type: multipart/form-data" \
    -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
    -F "ids=$INPUT_PDF_ID, $PREVIEW_PDF_ID" | jq -r '.'
fi

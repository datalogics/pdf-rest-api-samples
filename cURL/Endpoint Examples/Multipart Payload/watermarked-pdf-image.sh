WATERMARKED_OUTPUT=$(curl -X POST "https://api.pdfrest.com/watermarked-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@/path/to/file" \
  -F "watermark_file=@/path/to/file" \
  -F "output=example_out")

echo $WATERMARKED_OUTPUT | jq -r '.'

# All files uploaded or generated are automatically deleted based on the 
# File Retention Period as shown on https://pdfrest.com/pricing. 
# For immediate deletion of files, particularly when sensitive data 
# is involved, an explicit delete call can be made to the API.

# Optional deletion step — OFF by default.
# Deletes all files in the workflow, including outputs. Save all desired files before enabling this step.
# Enable by uncommenting the next line to delete sensitive files
# DELETE_SENSITIVE_FILES=true
if [ "$DELETE_SENSITIVE_FILES" = "true" ]; then
  INPUT_IDS=$(jq -r '.inputId | join(",")' <<< $WATERMARKED_OUTPUT)
  OUTPUT_PDF_ID=$(jq -r '.outputId' <<< $WATERMARKED_OUTPUT)
  curl -X POST "https://api.pdfrest.com/delete" \
    -H "Accept: application/json" \
    -H "Content-Type: multipart/form-data" \
    -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
    -F "ids=$INPUT_IDS, $OUTPUT_PDF_ID"
fi

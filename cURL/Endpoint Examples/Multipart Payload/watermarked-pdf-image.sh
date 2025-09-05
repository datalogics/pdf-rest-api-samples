WATERMARKED_OUTPUT=$(curl -X POST "https://api.pdfrest.com/watermarked-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@/path/to/file" \
  -F "watermark_file=@/path/to/file" \
  -F "output=example_out")

echo $WATERMARKED_OUTPUT

# All files uploaded or generated are automatically deleted based on the 
# File Retention Period as shown on https://pdfrest.com/pricing. 
# For immediate deletion of files, particularly when sensitive data 
# is involved, an explicit delete call can be made to the API.

# Optional deletion step — OFF by default.
# Deletes sensitive files (unredacted, unwatermarked, unencrypted, or unrestricted).
# Enable by uncommenting the next line to delete sensitive files
# PDFREST_DELETE_SENSITIVE_FILES=true
if [ "$PDFREST_DELETE_SENSITIVE_FILES" = "true" ]; then
  INPUT_IDS=$(jq -r '.inputId | join(",")' <<< $WATERMARKED_OUTPUT)
  curl -X POST "https://api.pdfrest.com/delete" \
    -H "Accept: application/json" \
    -H "Content-Type: multipart/form-data" \
    -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
    -F "ids=$INPUT_IDS"
fi

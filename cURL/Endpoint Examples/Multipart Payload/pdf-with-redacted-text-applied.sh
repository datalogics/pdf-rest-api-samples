REDACTED_OUTPUT=$(curl -X POST "https://api.pdfrest.com/pdf-with-redacted-text-applied" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@/path/to/file" \
  -F "output=example_out")

echo $REDACTED_OUTPUT

# All files uploaded or generated are automatically deleted based on the 
# File Retention Period as shown on https://pdfrest.com/pricing. 
# For immediate deletion of files, particularly when sensitive data 
# is involved, an explicit delete call can be made to the API.

# The following code is an optional step to delete unredacted files from pdfRest servers.

PREVIEW_PDF_ID=$(jq -r '.inputId' <<< $REDACTED_OUTPUT)
curl -X POST "https://api.pdfrest.com/delete" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "ids=$PREVIEW_PDF_ID"
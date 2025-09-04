#!/bin/sh

# In this sample, we will show how to watermark a PDF document and then restrict
# editing on the document so that the watermark cannot be removed, as discussed in
# https://pdfrest.com/solutions/add-pdf-watermarks-that-cannot-be-removed/.
# We will be running the input file through /watermarked-pdf to apply the watermark
# and then /restricted-pdf to lock the watermark in.


API_KEY="xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" # place your api key here

WATERMARKED_OUTPUT=$(curl -X POST "https://api.pdfrest.com/watermarked-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: $API_KEY" \
  -F "file=@/path/to/file.pdf" \
  -F "watermark_text=watermark" \
  -F "output=example_out")

WATERMARKED_ID=$(jq -r '.outputId' <<< $WATERMARKED_OUTPUT)

curl -X POST "https://api.pdfrest.com/restricted-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: $API_KEY" \
  -F "id=$WATERMARKED_ID" \
  -F "output=example_out" \
  -F "restrictions[]=edit_annotations" \
  -F "restrictions[]=edit_content" \
  -F "restrictions[]=copy_content" \
  -F "new_permissions_password=password"

# All files uploaded or generated are automatically deleted based on the 
# File Retention Period as shown on https://pdfrest.com/pricing. 
# For immediate deletion of files, particularly when sensitive data 
# is involved, an explicit delete call can be made to the API.

# The following code is an optional step to delete unwatermarked and 
# watermarked files from pdfRest servers.

INPUT_PDF_ID=$(jq -r '.inputId' <<< $WATERMARKED_OUTPUT)
curl -X POST "https://api.pdfrest.com/delete" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: $API_KEY" \
  -F "ids=$INPUT_PDF_ID, $WATERMARKED_ID"

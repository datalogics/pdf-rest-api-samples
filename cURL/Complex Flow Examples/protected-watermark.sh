#!/bin/sh

# In this sample, we will show how to watermark a PDF document and then restrict
# editing on the document so that the watermark cannot be removed, as discussed in
# https://pdfrest.com/solutions/add-pdf-watermarks-that-cannot-be-removed/.
# We will be running the input file through /watermarked-pdf to apply the watermark
# and then /restricted-pdf to lock the watermark in.

WATERMARKED_ID=$(curl -X POST "https://api.pdfrest.com/watermarked-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@/path/to/file.pdf" \
  -F "watermark_text=watermark" \
  -F "output=example_out" \
  | jq -r '.outputId')

curl -X POST "https://api.pdfrest.com/restricted-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "id=$WATERMARKED_ID" \
  -F "output=example_out" \
  -F "restrictions[]=edit_annotations" \
  -F "restrictions[]=edit_content" \
  -F "restrictions[]=copy_content" \
  -F "new_permissions_password=password"

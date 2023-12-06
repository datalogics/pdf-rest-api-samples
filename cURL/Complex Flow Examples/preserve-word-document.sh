#!/bin/sh

# In this sample we will show how to optimize a word file for long term preservation
# as discussed in https://pdfrest.com/solutions/optimize-word-excel-and-powerpoint-files-for-long-term-preservation/
# We will take our word (or excel or powerpoint) document and first convert it to
# a PDF with a call to the /pdf route. Then we will take that converted PDF
# and convert it to the PDF/A format for long term storage.

PDF_ID=$(curl -X POST "https://api.pdfrest.com/pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@/path/to/file.doc" \
  | jq -r '.outputId')

curl -X POST "https://api.pdfrest.com/pdfa" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "id=$PDF_ID" \
  -F "output=example_out" \
  -F "output_type=PDF/A-3b" \

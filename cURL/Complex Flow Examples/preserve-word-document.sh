#!/bin/sh

# In this sample, we will show how to optimize a Word file for long-term preservation
# as discussed in https://pdfrest.com/solutions/optimize-word-excel-and-powerpoint-files-for-long-term-preservation/
# We will take our Word (or Excel or PowerPoint) document and first convert it to
# a PDF with a call to the /pdf route. Then, we will take that converted PDF
# and convert it to the PDF/A format for long-term storage.

API_KEY="xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" # place your api key here

PDF_ID=$(curl -X POST "https://api.pdfrest.com/pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: $API_KEY" \
  -F "file=@/path/to/file.doc" \
  | jq -r '.outputId')

curl -X POST "https://api.pdfrest.com/pdfa" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: $API_KEY" \
  -F "id=$PDF_ID" \
  -F "output=example_out" \
  -F "output_type=PDF/A-3b" \

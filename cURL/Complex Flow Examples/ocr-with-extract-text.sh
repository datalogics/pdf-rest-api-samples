#!/bin/sh

# In this sample, we will show how to convert a scanned document into a PDF with
# searchable and extractable text using Optical Character Recognition (OCR), and then
# extract that text from the newly created document.
#
# First, we will upload a scanned PDF to the /pdf-with-ocr-text route and capture the
# output ID. Then, we will send the output ID to the /extracted-text route, which will
# return the newly added text.

API_KEY="xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" # Replace with your API key

# Upload PDF for OCR
OCR_PDF_ID=$(curl -s -X POST "https://api.pdfrest.com/pdf-with-ocr-text" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: $API_KEY" \
  -F "file=@/path/to/file.pdf" \
  -F "output=example_pdf-with-ocr-text_out"\
  | jq -r '.outputId')


# Extract text from OCR'd PDF
EXTRACT_TEXT_RESPONSE=$(curl -s -X POST "https://api.pdfrest.com/extracted-text" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: $API_KEY" \
  -F "id=$OCR_PDF_ID")


FULL_TEXT=$(echo $EXTRACT_TEXT_RESPONSE | jq -r '.fullText')
echo "Extracted text: $FULL_TEXT"
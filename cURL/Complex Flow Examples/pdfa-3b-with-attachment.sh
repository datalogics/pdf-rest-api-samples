#!/bin/sh

# In this sample, we will show how to attach an xml document to a PDF file and then
# convert the file with the attachment to conform to the PDF/A standard, which
# can be useful for invoicing and standards compliance. We will be running the
# input document through /pdf-with-added-attachment to add the attachment and
# then /pdfa to do the PDF/A conversion.

# Note that there is nothing special about attaching an xml file, and any appropriate
# file may be attached and wrapped into the PDF/A conversion.

# By default, we use the US-based API service. This is the primary endpoint for global use.
API_URL="https://api.pdfrest.com"

# For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
# API_URL="https://eu-api.pdfrest.com"

API_KEY="xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" # place your api key here

ATTACHED_ID=$(curl -X POST "$API_URL/pdf-with-added-attachment" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: $API_KEY" \
  -F "file=@/path/to/file.pdf" \
  -F "file_to_attach=@/path/to/file_to_attach.xml" \
  -F "output=example_out" \
  | jq -r '.outputId')

curl -X POST "$API_URL/pdfa" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: $API_KEY" \
  -F "id=$ATTACHED_ID" \
  -F "output=example_out" \
  -F "output_type=PDF/A-3b" \

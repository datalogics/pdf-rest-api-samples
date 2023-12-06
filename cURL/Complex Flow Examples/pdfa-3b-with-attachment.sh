#!/bin/sh

# In this sample we will show how attach an xml document to a PDF file and then
# convert the file with the attachment to conform to the PDF/A standard which
# can be useful for invoicing and standards compliance. We will be running the
#input document through /pdf-with-added-attachment to add the attachment and
# then /pdfa to do the PDF/A conversion.

# Note that there is nothing special about attaching an xml file, and any approprite
# file may be attached and wrapped into the PDF/A conversion.

ATTACHED_ID=$(curl -X POST "https://api.pdfrest.com/pdf-with-added-attachment" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@/path/to/file.pdf" \
  -F "file_to_attach=@/path/to/file_to_attach.xml" \
  -F "output=example_out" \
  | jq -r '.outputId')

curl -X POST "https://api.pdfrest.com/pdfa" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "id=$ATTACHED_ID" \
  -F "output=example_out" \
  -F "output_type=PDF/A-3b" \

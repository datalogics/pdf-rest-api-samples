curl -X POST "https://api.pdfrest.com/pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@../Sample_Input/pdfRest.docx" \
  -F "tagged_pdf=on" \
  -F "output=example_out"

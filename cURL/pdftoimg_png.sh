curl -X POST "https://api.pdfrest.com/png" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@../Sample_Input/pdfRest.pdf" \
  -F "output=example_out"

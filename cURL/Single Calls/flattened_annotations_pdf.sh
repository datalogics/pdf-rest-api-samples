curl -X POST "https://api.pdfrest.com/flattened-annotations-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@../Sample_Input/ducky.pdf" \
  -F "output=example_out"

curl -X POST "https://api.pdfrest.com/exported-form-data" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@/path/to/file" \
  -F "data_format=xml" \
  -F "output=example_out"

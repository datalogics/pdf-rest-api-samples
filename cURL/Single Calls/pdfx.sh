curl -X POST "https://api.pdfrest.com/pdfx" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@/path/to/file.pdf" \
  -F "output=example_out" \
  -F "output_type=PDF/X-4" \
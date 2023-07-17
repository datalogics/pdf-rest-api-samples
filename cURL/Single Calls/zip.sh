curl -X POST "https://api.pdfrest.com/zip" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@/path/to/file.pdf" \
  -F "file=@/path/to/file2.pdf" \
  -F "file=@/path/to/file.jpg" \
  -F "output=example_out"

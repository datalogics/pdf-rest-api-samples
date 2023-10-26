curl -X POST "https://api.pdfrest.com/split-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@/path/to/file" \
  -F "output=example_out" \
  -F "pages[]=1-4" \
  -F "pages[]=2" \
  -F "pages[]=4,6-3"

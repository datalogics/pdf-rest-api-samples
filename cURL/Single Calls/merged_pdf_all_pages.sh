curl -X POST "https://api.pdfrest.com/merged-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@/path/to/file1.pdf" \
  -F "pages[]=all" -F "type[]=file" \
  -F "file=@/path/to/file2.pdf" \
  -F "pages[]=all" -F "type[]=file" \
  -F "output=example_out"

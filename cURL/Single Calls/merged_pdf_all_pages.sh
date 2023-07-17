curl -X POST "https://api.pdfrest.com/merged-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@../Sample_Input/merge1.pdf" \
  -F "pages[]=all" -F "type[]=file" \
  -F "file=@../Sample_Input/merge2.pdf" \
  -F "pages[]=all" -F "type[]=file" \
  -F "output=example_out"

curl -X POST "https://api.pdfrest.com/merged-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@../Sample_Input/toMergeA.pdf" \
  -F "pages[]=even" -F "type[]=file" \
  -F "file=@../Sample_Input/toMergeB.pdf" \
  -F "pages[]=odd" -F "type[]=file" \
  -F "output=example_out"

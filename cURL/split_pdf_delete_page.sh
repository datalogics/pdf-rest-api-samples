curl -X POST "https://api.pdfrest.com/split-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@../Sample_Input/PDFToBeSplit.pdf" \
  -F "output=example_out" \
  -F "pages[]=1-3,5,6"

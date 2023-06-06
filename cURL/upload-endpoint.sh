curl -X POST "https://api.pdfrest.com/upload" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@../Sample_Input/ducky.pdf" \
  -F "file=@../Sample_Input/merge1.pdf" \
  -F "file=@../Sample_Input/strawberries.jpg" \
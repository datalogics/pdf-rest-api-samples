curl -X POST "https://api.pdfrest.com/pdf-with-added-image" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@../Sample_Input/ducky.pdf" \
  -F "image_file=@../Sample_Input/strawberries.jpg" \
  -F "output=example_out" \
  -F "x=10" \
  -F "y=10" \
  -F "page=1"
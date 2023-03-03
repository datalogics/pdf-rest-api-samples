curl -X POST "https://api.pdfrest.com/pdf-with-added-image" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@../Sample_Input/toAddImage.pdf" \
  -F "image_file=@../Sample_Input/pdfRestApiLab.png" \
  -F "output=example_out" \
  -F "x=200" \
  -F "y=650" \
  -F "page=1"
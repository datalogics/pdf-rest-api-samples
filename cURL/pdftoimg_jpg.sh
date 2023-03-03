curl -X POST "https://api.pdfrest.com/jpg" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@../Sample_Input/pdfRest.pdf" \
  -F "output=example_out" \
  -F "pages=1-last" \
  -F "resolution=300" \
  -F "color_model=rgb" \
  -F "jpeg_quality=100"

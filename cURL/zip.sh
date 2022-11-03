curl -X POST "https://api.pdfrest.com/zip" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@../Sample_Input/Datalogics.bmp" \
  -F "file=@../Sample_Input/ducky.pdf" \
  -F "file=@../Sample_Input/rainbow.tif" \
  -F "output=example_out"

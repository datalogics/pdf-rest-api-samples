curl -X POST "https://api.pdfrest.com/zip" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@../Sample_Input/Datalogics.png" \
  -F "file=@../Sample_Input/pdfRest.pdf" \
  -F "file=@../Sample_Input/pdfRestApiLab.png" \
  -F "output=example_out"

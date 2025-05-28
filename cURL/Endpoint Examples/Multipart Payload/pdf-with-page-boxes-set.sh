BOXES='{"boxes":[{"box":"media","pages":[{"range":"1","left":100,"top":100,"bottom":100,"right":100}]}]}'

curl -X POST "https://api.pdfrest.com/pdf-with-page-boxes-set" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@/path/to/file" \
  -F "boxes=$BOXES" \
  -F "output=example_out.pdf"

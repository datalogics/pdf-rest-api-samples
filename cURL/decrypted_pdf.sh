curl -X POST "https://api.pdfrest.com/decrypted-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@../Sample_Input/toDecrypt.pdf" \
  -F "output=example_out" \
  -F "current_open_password=password"

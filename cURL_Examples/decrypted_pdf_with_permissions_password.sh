curl -X POST "https://api.pdfrest.com/decrypted-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@../Sample_Input/twoPasswords.pdf" \
  -F "output=example_out" \
  -F "current_open_password=encryptedducky" \
  -F "current_permissions_password=restrictedducky"

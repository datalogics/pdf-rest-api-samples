curl -X POST "https://api.pdfrest.com/encrypted-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@/path/to/file.pdf" \
  -F "output=example_out" \
  -F "current_open_password=password" \
  -F "new_open_password=newpassword"

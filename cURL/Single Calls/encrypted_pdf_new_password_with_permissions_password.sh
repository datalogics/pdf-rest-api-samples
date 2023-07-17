curl -X POST "https://api.pdfrest.com/encrypted-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@../Sample_Input/toUnrestrict.pdf" \
  -F "output=example_out" \
  -F "new_open_password=newpassword" \
  -F "current_permissions_password=password"

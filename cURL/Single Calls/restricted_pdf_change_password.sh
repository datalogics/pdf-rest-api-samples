curl -X POST "https://api.pdfrest.com/restricted-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@/path/to/file.pdf" \
  -F "output=example_out" \
  -F "current_permissions_password=password" \
  -F "new_permissions_password=newpassword" \
  -F "restrictions[]=print_low"

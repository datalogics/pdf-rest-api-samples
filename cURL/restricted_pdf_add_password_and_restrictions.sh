curl -X POST "https://api.pdfrest.com/restricted-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@../Sample_Input/toRestrict.pdf" \
  -F "output=example_out" \
  -F "new_permissions_password=password" \
  -F "restrictions[]=print_low" \
  -F "restrictions[]=accessibility_off"

curl -X POST "https://api.pdfrest.com/compressed-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@../Sample_Input/toOptimize.pdf" \
  -F "output=example_out" \
  -F "compression_level=custom" \
  -F "profile=@PATH_TO_FILE/example_profile.json"

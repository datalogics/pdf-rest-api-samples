curl -X POST "https://api.pdfrest.com/pdfa" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@../Sample_Input/ducky.pdf" \
  -F "output=example_out" \
  -F "output_type=pdf/a-1b" \
  -F "rasterize_if_errors_encountered=off"

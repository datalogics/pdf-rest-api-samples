curl -X POST "https://api.pdfrest.com/watermarked-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@/path/to/file" \
  -F "watermark_file=@/path/to/file" \
  -F "output=example_out"

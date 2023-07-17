curl -X POST "https://api.pdfrest.com/pdf-info" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@../Sample_Input/ducky.pdf" \
  -F "queries=title,page_count,doc_language,author"

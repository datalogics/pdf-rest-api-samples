REDACTIONS='[{"type":"preset","value":"uuid"},{"type":"regex","value":"(\\+\\d{1,2}\\s)?\\(?\\d{3}\\)?[\\s.-]\\d{3}[\\s.-]\\d{4}"},{"type":"literal","value":"word"}]'

curl -X POST "https://api.pdfrest.com/pdf-with-redacted-text-preview" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@/path/to/file" \
  -F "redactions=$REDACTIONS" \
  -F "output=example_out"

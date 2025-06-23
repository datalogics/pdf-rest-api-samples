SIGNATURE_CONFIG='{"type": "new","name": "esignature","location": {"bottom_left": { "x": "0", "y": "0" },"top_right": { "x": "216", "y": "72" },"page": 1},"display": {"include_datetime": "true"}}'

curl -X POST "https://api.pdfrest.com/signed-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@/path/to/file" \
  -F "certificate_file=@/path/to/file" \
  -F "private_key_file=@/path/to/file" \
  -F "signature_configuration=$SIGNATURE_CONFIG" \
  -F "output=example_out"

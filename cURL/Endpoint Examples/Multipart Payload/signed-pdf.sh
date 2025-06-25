SIGNATURE_CONFIG='{"type": "new","name": "esignature","logo_opacity": "0.25","location": {"bottom_left": { "x": "0", "y": "0" },"top_right": { "x": "216", "y": "72" },"page": 1},"display": {"include_distinguished_name": "true","include_datetime": "true","contact": "My contact info","location": "My location","name": "John Doe","reason": "My reason for signing"}}'

curl -X POST "https://api.pdfrest.com/signed-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@/path/to/file" \
  -F "pfx_credential_file=@/path/to/file" \
  -F "pfx_passphrase_file=@/path/to/file" \
  -F "logo_file=@/path/to/file" \
  -F "signature_configuration=$SIGNATURE_CONFIG" \
  -F "output=example_out"

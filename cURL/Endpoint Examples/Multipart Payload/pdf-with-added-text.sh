TEXT_OPTIONS='[{"font":"Times New Roman","max_width":"175","opacity":"1","page":"1","rotation":"0","text":"sample text in PDF","text_color_rgb":"0,0,0","text_size":"30","x":"72","y":"144"}]'

curl -X POST "https://api.pdfrest.com/pdf-with-added-text" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@/path/to/file" \
  -F "text_objects=$TEXT_OPTIONS" \
  -F "output=example_out"

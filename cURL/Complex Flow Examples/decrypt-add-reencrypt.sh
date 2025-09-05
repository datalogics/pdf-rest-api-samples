#!/bin/sh

# In this sample, we will show how to take an encrypted file and decrypt, modify
# and re-encrypt it to create an encryption-at-rest solution as discussed in
# https://pdfrest.com/solutions/create-secure-document-workflows-with-pdf-password-protection/
# We will be running the document through /decrypted-pdf to open the document
# to modification, running the decrypted result through /pdf-with-added-image,
# and then sending the output with the new image through /encrypted-pdf to
# lock it up again.

API_KEY="xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" # place your api key here

DECRYPTED_ID=$(curl -X POST "https://api.pdfrest.com/decrypted-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: $API_KEY" \
  -F "file=@/path/to/file.pdf" \
  -F "output=example_out" \
  -F "current_open_password=password" \
  | jq -r '.outputId')


ADDED_IMAGE_ID=$(curl -X POST "https://api.pdfrest.com/pdf-with-added-image" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: $API_KEY" \
  -F "id=$DECRYPTED_ID" \
  -F "image_file=@/path/to/image.png" \
  -F "output=example_out" \
  -F "x=10" \
  -F "y=10" \
  -F "page=1" \
  | jq -r '.outputId')


curl -X POST "https://api.pdfrest.com/encrypted-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: $API_KEY" \
  -F "id=$ADDED_IMAGE_ID" \
  -F "output=example_out" \
  -F "new_open_password=password"


# All files uploaded or generated are automatically deleted based on the 
# File Retention Period as shown on https://pdfrest.com/pricing. 
# For immediate deletion of files, particularly when sensitive data 
# is involved, an explicit delete call can be made to the API.

# Optional deletion step — OFF by default.
# Deletes sensitive files (unredacted, unwatermarked, unencrypted, or unrestricted).
# Enable by uncommenting the next line to delete sensitive files
# PDFREST_DELETE_SENSITIVE_FILES=true
if [ "$PDFREST_DELETE_SENSITIVE_FILES" = "true" ]; then
  curl -X POST "https://api.pdfrest.com/delete" \
    -H "Accept: application/json" \
    -H "Content-Type: multipart/form-data" \
    -H "Api-Key: $API_KEY" \
    -F "ids=$DECRYPTED_ID, $ADDED_IMAGE_ID"
fi

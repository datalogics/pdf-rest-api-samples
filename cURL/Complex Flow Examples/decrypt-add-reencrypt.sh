#!/bin/sh

# In this sample, we will show how to take an encrypted file and decrypt, modify
# and re-encrypt it to create an encryption-at-rest solution as discussed in
# https://pdfrest.com/solutions/create-secure-document-workflows-with-pdf-password-protection/
# We will be running the document through /decrypted-pdf to open the document
# to modification, running the decrypted result through /pdf-with-added-image,
# and then sending the output with the new image through /encrypted-pdf to
# lock it up again.

# By default, we use the US-based API service. This is the primary endpoint for global use.
API_URL="https://api.pdfrest.com"

# For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
# API_URL = "https://eu-api.pdfrest.com"

API_KEY="xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" # place your api key here

DECRYPTED_OUTPUT=$(curl -X POST "$API_URL/decrypted-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: $API_KEY" \
  -F "file=@/path/to/file.pdf" \
  -F "output=example_out" \
  -F "current_open_password=password")

DECRYPTED_ID=$(jq -r '.outputId' <<< $DECRYPTED_OUTPUT)


ADDED_IMAGE_OUTPUT=$(curl -X POST "$API_URL/pdf-with-added-image" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: $API_KEY" \
  -F "id=$DECRYPTED_ID" \
  -F "image_file=@/path/to/file.png" \
  -F "output=example_out" \
  -F "x=10" \
  -F "y=10" \
  -F "page=1")

ADDED_IMAGE_ID=$(jq -r '.outputId' <<< $ADDED_IMAGE_OUTPUT)


ENCRYPTED_OUTPUT=$(curl -X POST "$API_URL/encrypted-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: $API_KEY" \
  -F "id=$ADDED_IMAGE_ID" \
  -F "output=example_out" \
  -F "new_open_password=password")

echo $ENCRYPTED_OUTPUT | jq -r '.'


# All files uploaded or generated are automatically deleted based on the
# File Retention Period as shown on https://pdfrest.com/pricing.
# For immediate deletion of files, particularly when sensitive data
# is involved, an explicit delete call can be made to the API.

# Optional deletion step — OFF by default.
# Deletes all files in the workflow, including outputs. Save all desired files before enabling this step.
# Enable by uncommenting the next line to delete sensitive files
# DELETE_SENSITIVE_FILES=true
if [ "$DELETE_SENSITIVE_FILES" = "true" ]; then
  INPUT_PDF_ID=$(jq -r '.inputId' <<< $DECRYPTED_OUTPUT)
  ADDED_IMAGE_INPUTS=$(jq -r '.inputId | join(",")' <<< $ADDED_IMAGE_OUTPUT) # Includes the DECRYPTED_ID
  REENCRYPTED_ID=$(jq -r '.outputId' <<< $ENCRYPTED_OUTPUT)
  curl -X POST "$API_URL/delete" \
    -H "Accept: application/json" \
    -H "Content-Type: multipart/form-data" \
    -H "Api-Key: $API_KEY" \
    -F "ids=$INPUT_PDF_ID, $ADDED_IMAGE_INPUTS, $ADDED_IMAGE_ID, $REENCRYPTED_ID" | jq -r '.'
fi

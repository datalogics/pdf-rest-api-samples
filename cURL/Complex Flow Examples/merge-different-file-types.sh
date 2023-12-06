#!/bin/sh

# In this sample we will show how to merge different file types together as
# discussed in https://pdfrest.com/solutions/merge-multiple-types-of-files-together/.
# Specifically we will be uploadng an image file to the /pdf route and capturing
# the output ID, uploading a powerpoint file to the /pdf route and capturing the
# output ID and then passing both of those IDs to the /merged-pdf route to get
# a singular output PDF combining the two inputs

# Note that there is nothing special about an image and a powepoint file and
# this sample could be easily used to convert and combine any two file types
# that the /pdf route takes as inputs

IMAGE_ID=$(curl -X POST "https://api.pdfrest.com/pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@/path/to/file.png" \
  -F "output=example_out" \
  | jq -r '.outputId')


PPT_ID=$(curl -X POST "https://api.pdfrest.com/pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "file=@/path/to/powerpoint.ppt" \
  -F "output=example_out" \
  | jq -r '.outputId')


curl -X POST "https://api.pdfrest.com/merged-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" \
  -F "id=$IMAGE_ID" \
  -F "pages[]=all" -F "type[]=id" \
  -F "id=$PPT_ID" \
  -F "pages[]=all" -F "type[]=id" \
  -F "output=example_out"

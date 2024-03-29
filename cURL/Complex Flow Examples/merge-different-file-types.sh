#!/bin/sh

# In this sample, we will show how to merge different file types together as
# discussed in https://pdfrest.com/solutions/merge-multiple-types-of-files-together/.
# First, we will upload an image file to the /pdf route and capture the output ID.
# Next, we will upload a PowerPoint file to the /pdf route and capture its output
# ID. Finally, we will pass both IDs to the /merged-pdf route to combine both inputs
# into a single PDF.
#
# Note that there is nothing special about an image and a PowerPoint file, and
# this sample could be easily used to convert and combine any two file types
# that the /pdf route takes as inputs.

API_KEY="xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" # place your api key here

IMAGE_ID=$(curl -X POST "https://api.pdfrest.com/pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: $API_KEY" \
  -F "file=@/path/to/file.png" \
  -F "output=example_out" \
  | jq -r '.outputId')


PPT_ID=$(curl -X POST "https://api.pdfrest.com/pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: $API_KEY" \
  -F "file=@/path/to/powerpoint.ppt" \
  -F "output=example_out" \
  | jq -r '.outputId')


curl -X POST "https://api.pdfrest.com/merged-pdf" \
  -H "Accept: application/json" \
  -H "Content-Type: multipart/form-data" \
  -H "Api-Key: $API_KEY" \
  -F "id=$IMAGE_ID" \
  -F "pages[]=all" -F "type[]=id" \
  -F "id=$PPT_ID" \
  -F "pages[]=all" -F "type[]=id" \
  -F "output=example_out"

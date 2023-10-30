from requests_toolbelt import MultipartEncoder
import requests
import json

merged_pdf_endpoint_url = 'https://api.pdfrest.com/merged-pdf'

# The /merged-pdf endpoint can take one or more PDF files or ids as input.
# This sample takes 2 PDF files and merges all the pages in the document into a single document.

merge_request_data = []

# Array of tuples that contains information about the 2 files that will be merged
files = [
    ('file_name.pdf', open('/path/to/file', 'rb'), 'application/pdf'),
    ('file_name2.pdf', open('/path/to/file', 'rb'), 'application/pdf')
]

# Structure the data that will be sent to POST merge request as an array of tuples
for i in range(len(files)):
    merge_request_data.append(("file", files[i]))
    merge_request_data.append(("pages", "1-last"))
    merge_request_data.append(("type", "file"))

merge_request_data.append(('output', 'example_mergedPdf_out'))

mp_encoder_mergedPdf = MultipartEncoder(
    fields=merge_request_data
)

# Let's set the headers that the merged-pdf endpoint expects.
# Since MultipartEncoder is used, the 'Content-Type' header gets set to 'multipart/form-data' via the content_type attribute below.
headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_mergedPdf.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to merged-pdf endpoint...")
response = requests.post(merged_pdf_endpoint_url, data=mp_encoder_mergedPdf, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

# If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.py' sample.

from requests_toolbelt import MultipartEncoder
import requests
import json

split_pdf_endpoint_url = 'https://api.pdfrest.com/split-pdf'

# The /split-pdf endpoint can take one PDF file or id as input.
# This sample takes one PDF file that has at least 5 pages and splits it into two documents when given two page ranges.

# Create a list of tuples for data that will be sent to the request
split_request_data = []
split_request_data.append(('file',('PDFToBeSplit.pdf', open('../Sample_Input/PDFToBeSplit.pdf', 'rb'), 'application/pdf')))
split_request_data.append(('pages', '1,2,5'))
split_request_data.append(('pages', '3,4'))
split_request_data.append(('output', 'example_splitPdf_out'))

mp_encoder_splitPdf = MultipartEncoder(
    fields=split_request_data
)

# Let's set the headers that the split-pdf endpoint expects.
# Since MultipartEncoder is used, the 'Content-Type' header gets set to 'multipart/form-data' via the content_type attribute below.
headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_splitPdf.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to split-pdf endpoint...")
response = requests.post(split_pdf_endpoint_url, data=mp_encoder_splitPdf, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

# If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.py' sample.

from requests_toolbelt import MultipartEncoder
import requests
import json

compressed_pdf_endpoint_url = 'https://api.pdfrest.com/compressed-pdf'

# The /compressed-pdf endpoint can take a single PDF file or id as input.
# This sample demonstrates setting compression_level to 'medium'.
# We have preset 'high', 'medium', and 'low' compression levels available for use. These preset levels do not require the 'profile' parameter.
mp_encoder_compressedPdf = MultipartEncoder(
    fields={
        'file': ('toOptimize.pdf', open('../Sample_Input/toOptimize.pdf', 'rb'), 'application/pdf'),
        'output' : 'example_compressedPdf_out',
        'compression_level': 'medium',
    }
)

# Let's set the headers that the compressed-pdf endpoint expects.
# Since MultipartEncoder is used, the 'Content-Type' header gets set to 'multipart/form-data' via the content_type attribute below.
headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_compressedPdf.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to compressed-pdf endpoint...")
response = requests.post(compressed_pdf_endpoint_url, data=mp_encoder_compressedPdf, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

# If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.py' sample.

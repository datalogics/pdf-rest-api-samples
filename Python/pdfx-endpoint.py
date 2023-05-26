from requests_toolbelt import MultipartEncoder
import requests
import json

pdfx_endpoint_url = 'https://api.pdfrest.com/pdfx'

# The /pdfx endpoint can take a single PDF file or id as input.
mp_encoder_pdfx = MultipartEncoder(
    fields={
        'file': ('ducky.pdf', open('../Sample_Input/ducky.pdf', 'rb'), 'application/pdf'),
        'output_type': 'PDF/X-4',
        'output' : 'example_pdfx_out'
    }
)

# Let's set the headers that the pdfx endpoint expects.
# Since MultipartEncoder is used, the 'Content-Type' header gets set to 'multipart/form-data' via the content_type attribute below.
headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_pdfx.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to pdfx endpoint...")
response = requests.post(pdfx_endpoint_url, data=mp_encoder_pdfx, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

# If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.py' sample.

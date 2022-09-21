from requests_toolbelt import MultipartEncoder
import requests
import json

unrestricted_pdf_endpoint_url = 'https://api.pdfrest.com/unrestricted-pdf'

# The /unrestricted-pdf endpoint can take a single PDF file or id as input.
# This sample demonstrates removing security restrictions from a PDF.
mp_encoder_unrestrictedPdf = MultipartEncoder(
    fields={
        'file': ('toUnrestrict.pdf', open('../Sample_Input/toUnrestrict.pdf', 'rb'), 'application/pdf'),
        'output' : 'example_unrestrictedPdf_out',
        'current_permissions_password': 'password'
    }
)

# Let's set the headers that the unrestricted-pdf endpoint expects.
# Since MultipartEncoder is used, the 'Content-Type' header gets set to 'multipart/form-data' via the content_type attribute below.
headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_unrestrictedPdf.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to unrestricted-pdf endpoint...")
response = requests.post(unrestricted_pdf_endpoint_url, data=mp_encoder_unrestrictedPdf, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

# If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.py' sample.

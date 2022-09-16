from requests_toolbelt import MultipartEncoder
import requests
import json

linearized_pdf_endpoint_url = 'https://api.pdfrest.com/linearized-pdf'

# The /linearized-pdf endpoint can take a single PDF file or id as input.
# This sample demonstrates linearizing a PDF file.
mp_encoder_linearizedPdf = MultipartEncoder(
    fields={
        'file': ('toLinearized.pdf', open('../Sample_Input/toLinearize.pdf', 'rb'), 'application/pdf'),
        'output' : 'example_linearizedPdf_out',
    }
)

# Let's set the headers that the linearized-pdf endpoint expects.
# Since MultipartEncoder is used, the 'Content-Type' header gets set to 'multipart/form-data' via the content_type attribute below.
headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_linearizedPdf.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to linearized-pdf endpoint...")
response = requests.post(linearized_pdf_endpoint_url, data=mp_encoder_linearizedPdf, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

# If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.py' sample.

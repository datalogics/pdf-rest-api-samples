from requests_toolbelt import MultipartEncoder
import requests
import json

acroform_endpoint_url = 'https://api.pdfrest.com/pdf-with-acroforms'

# The /pdf-with-acroforms endpoint can take a single PDF file or id as input.
# This sample demonstrates converting an XFA PDF to an acroform document.
mp_encoder_acroform = MultipartEncoder(
    fields={
        'file': ('file_name', open('/path/to/file', 'rb'), 'application/pdf'),
        'output' : 'example_acroform_out',
    }
)

# Let's set the headers that the acroform endpoint expects.
# Since MultipartEncoder is used, the 'Content-Type' header gets set to 'multipart/form-data' via the content_type attribute below.
headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_acroform.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to pdf-with-acroforms endpoint...")
response = requests.post(acroform_endpoint_url, data=mp_encoder_acroform, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

# If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.py' sample.

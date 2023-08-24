from requests_toolbelt import MultipartEncoder
import requests
import json

exported_form_data_endpoint_url = 'https://api.pdfrest.com/exported-form-data'

# The /exported-form-data endpoint can take a single PDF file or id as input.
# This sample demonstrates encryption of a PDF with the password 'password'.
mp_encoder_exportedFormData = MultipartEncoder(
    fields={
        'file': ('file_name', open('/path/to/file', 'rb'), 'application/pdf'),
        'output' : 'example_exportedFormData_out',
        'data_format': 'xml',
    }
)

# Let's set the headers that the encrypted-pdf endpoint expects.
# Since MultipartEncoder is used, the 'Content-Type' header gets set to 'multipart/form-data' via the content_type attribute below.
headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_exportedFormData.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to exported-form-data endpoint...")
response = requests.post(exported_form_data_endpoint_url, data=mp_encoder_exportedFormData, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

# If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.py' sample.

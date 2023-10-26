from requests_toolbelt import MultipartEncoder
import requests
import json

import_form_data_endpoint_url = 'https://api.pdfrest.com/pdf-with-imported-form-data'

mp_encoder_importFormData = MultipartEncoder(
    fields={
        'file': ('file_name', open('/path/to/file', 'rb'), 'application/pdf'),
        'data_file': ('file_name', open('/path/to/datafile', 'rb'), 'application/xml'), # Update for your data file format
        'output' : 'example_out'
    }
)

headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_importFormData.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to pdf-with-imported-form-data endpoint...")
response = requests.post(import_form_data_endpoint_url, data=mp_encoder_importFormData, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

from requests_toolbelt import MultipartEncoder
import requests
import json

unzip_endpoint_url = 'https://api.pdfrest.com/unzip'

mp_encoder_unzip = MultipartEncoder(
    fields={
        'file': ('file_name.zip', open('/path/to/file', 'rb'), 'application/zip'),
        'output' : 'example_out'
    }
)

headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_unzip.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to unzip endpoint...")
response = requests.post(unzip_endpoint_url, data=mp_encoder_unzip, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

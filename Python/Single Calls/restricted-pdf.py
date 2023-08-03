from requests_toolbelt import MultipartEncoder
import requests
import json

restricted_pdf_endpoint_url = 'https://api.pdfrest.com/restricted-pdf'

# The /restricted-pdf endpoint can take a single PDF file or id as input.
# This sample demonstrates setting the permissions password to 'password' and adding restrictions.
mp_encoder_restrictedPdf = MultipartEncoder(
    fields=[
        ('file', ('file_name', open('/path/to/file', 'rb'), 'application/pdf')),
        ('output', 'example_restrictedPdf_out'),
        ('new_permissions_password', 'password'),
        ('restrictions', 'print_high'),
        ('restrictions', 'print_low'),
        ('restrictions', 'edit_content')
    ]
)

# Let's set the headers that the restricted-pdf endpoint expects.
# Since MultipartEncoder is used, the 'Content-Type' header gets set to 'multipart/form-data' via the content_type attribute below.
headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_restrictedPdf.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to restricted-pdf endpoint...")
response = requests.post(restricted_pdf_endpoint_url, data=mp_encoder_restrictedPdf, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

# If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.py' sample.

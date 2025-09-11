from requests_toolbelt import MultipartEncoder
import requests
import json

# By default, we use the US-based API service. This is the primary endpoint for global use.
api_url = "https://api.pdfrest.com"

# For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#api_url = "https://eu-api.pdfrest.com"

signed_pdf_endpoint_url = api_url+'/signed-pdf'

signature_config = {
    "type": "new",
    "name": "esignature",
    "logo_opacity": "0.5",
    "location": {
        "bottom_left": { "x": "0", "y": "0" },
        "top_right": { "x": "216", "y": "72" },
        "page": 1
    },
    "display": {
        "include_distinguished_name": "true",
        "include_datetime": "true",
        "contact": "My contact information",
        "location": "My signing location",
        "name": "John Doe",
        "reason": "My reason for signing"
    }
}

mp_encoder_signPDF = MultipartEncoder(
    fields={
        'file': ('file_name.pdf', open('/path/to/file', 'rb'), 'application/pdf'),
        'pfx_credential_file': ('file_name.pfx', open('/path/to/file_name.pfx', 'rb'), 'application/x-pkcs12'),
        'pfx_passphrase_file': ('file_name.txt', open('/path/to/file_name.txt', 'rb'), 'text/plain'),
        'logo_file': ('file_name.jpg', open('/path/to/file_name.jpg', 'rb'), 'image/jpeg'),
        'signature_configuration': json.dumps(signature_config),
        'output' : 'example_out'
    }
)

headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_signPDF.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to signed-pdf endpoint...")
response = requests.post(signed_pdf_endpoint_url, data=mp_encoder_signPDF, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

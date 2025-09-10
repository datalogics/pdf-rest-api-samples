import requests
import json

# By default, we use the US-based API service. This is the primary endpoint for global use.
api_url = "https://api.pdfrest.com"

# For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#api_url = "https://eu-api.pdfrest.com"

def upload_file(file_path, file_name):
    with open(file_path, 'rb') as f:
        upload_data = f.read()
        print("Uploading file...")
        upload_response = requests.post(url=api_url+'/upload',
                            data=upload_data,
                            headers={'Content-Type': 'application/octet-stream', 'Content-Filename': file_name, "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})
        print("Upload response status code: " + str(upload_response.status_code))

        if upload_response.ok:
            upload_response_json = upload_response.json()
            print(json.dumps(upload_response_json, indent = 2))

            return upload_response_json['files'][0]['id']
        else:
            print(upload_response.text)
            return ""

input_pdf = '/path/to/input.pdf'
input_id = upload_file(input_pdf, 'input.pdf')

credential_path = '/path/to/credentials.pfx'
credential_id = upload_file(credential_path, 'credentials.pfx')

passphrase_path = '/path/to/passphrase.txt'
passphrase_id = upload_file(passphrase_path, 'passphrase.txt')

logo_path = '/path/to/logo.jpg'
logo_id = upload_file(logo_path, 'logo.jpg')

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
boxes_data = { "id" : input_id, "pfx_credential_id": credential_id, "pfx_passphrase_id": passphrase_id, "logo_id": logo_id, "signature_configuration": json.dumps(signature_config) }

print(json.dumps(boxes_data, indent = 2))


print("Processing file...")
boxes_response = requests.post(url=api_url+'/signed-pdf',
                    data=json.dumps(boxes_data),
                    headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})



print("Processing response status code: " + str(boxes_response.status_code))
if boxes_response.ok:
    boxes_response_json = boxes_response.json()
    print(json.dumps(boxes_response_json, indent = 2))

else:
    print(boxes_response.text)

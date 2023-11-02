from requests_toolbelt import MultipartEncoder
import requests
import json

pdf_with_added_attachment_endpoint_url = 'https://api.pdfrest.com/pdf-with-added-attachment'

mp_encoder_pdfWithAddedAttachment = MultipartEncoder(
    fields={
        'file': ('file_name', open('/path/to/file', 'rb'), 'application/pdf'),
        'file_to_attach': ('file_name', open('/path/to/file', 'rb'), 'application/xml'), # Update content type
        'output' : 'example_out',
    }
)

headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_pdfWithAddedAttachment.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to pdf-with-added-attachment endpoint...")
response = requests.post(pdf_with_added_attachment_endpoint_url, data=mp_encoder_pdfWithAddedAttachment, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

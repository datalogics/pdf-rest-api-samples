from requests_toolbelt import MultipartEncoder
import requests
import json

# By default, we use the US-based API service. This is the primary endpoint for global use.
api_url = "https://api.pdfrest.com"

# For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#api_url = "https://eu-api.pdfrest.com"

pdf_with_added_image_endpoint_url = api_url+'/pdf-with-added-image'

mp_encoder_pdfWithAddedImage = MultipartEncoder(
    fields={
        'file': ('file_name.pdf', open('/path/to/file', 'rb'), 'application/pdf'),
        'image_file': ('file_name.jpg', open('/path/to/file', 'rb'), 'image/jpeg'),
        'output' : 'example_out',
        'x' : '10',
        'y' : '10',
        'page' : '1',
    }
)

headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_pdfWithAddedImage.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to pdf-with-added-image endpoint...")
response = requests.post(pdf_with_added_image_endpoint_url, data=mp_encoder_pdfWithAddedImage, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

from requests_toolbelt import MultipartEncoder
import requests
import json

extracted_images_endpoint_url = 'https://api.pdfrest.com/extracted-images'

# The /extracted-images endpoint can take a single PDF file or id as input.
# This sample demonstrates image extraction from all pages of a document.
mp_encoder_extractedImages = MultipartEncoder(
    fields={
        'file': ('file_name.pdf', open('/path/to/file', 'rb'), 'application/pdf'),
        'output' : 'example_extractedImages_out',
        'pages': '1-last',
    }
)

# Let's set the headers that the extracted-images endpoint expects.
# Since MultipartEncoder is used, the 'Content-Type' header gets set to 'multipart/form-data' via the content_type attribute below.
headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_extractedImages.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to extracted-images endpoint...")
response = requests.post(extracted_images_endpoint_url, data=mp_encoder_extractedImages, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

# If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.py' sample.
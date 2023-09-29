from requests_toolbelt import MultipartEncoder
import requests
import json

extract_text_endpoint_url = 'https://api.pdfrest.com/extract-text'

# The /extract-text endpoint can take a single PDF file or id as input.
#This sample demonstrates querying the title, page count, document language and author
mp_encoder_extractText = MultipartEncoder(
    fields={
        'file': ('file_name', open('/path/to/file', 'rb'), 'application/pdf'),
    }
)

# Let's set the headers that the extract-text endpoint expects.
# Since MultipartEncoder is used, the 'Content-Type' header gets set to 'multipart/form-data' via the content_type attribute below.
headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_extractText.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to extract-text endpoint...")
response = requests.post(extract_text_endpoint_url, data=mp_encoder_extractText, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

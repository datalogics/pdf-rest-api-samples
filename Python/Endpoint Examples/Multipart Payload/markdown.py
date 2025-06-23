from requests_toolbelt import MultipartEncoder
import requests
import json

markdown_endpoint_url = 'https://api.pdfrest.com/markdown'

# The /markdown endpoint can take a single PDF file or id as input.
# This sample demonstrates converting the document to markdown and returning it as JSON.
mp_encoder_markdown = MultipartEncoder(
    fields={
        'file': ('file_name.pdf', open('/path/to/file', 'rb'), 'application/pdf'),
        'page_break_comments': 'on',
    }
)

# Let's set the headers that the markdown endpoint expects.
# Since MultipartEncoder is used, the 'Content-Type' header gets set to 'multipart/form-data' via the content_type attribute below.
headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_markdown.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to markdown endpoint...")
response = requests.post(markdown_endpoint_url, data=mp_encoder_markdown, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent=2))
else:
    print(response.text)
from requests_toolbelt import MultipartEncoder
import requests
import json

# By default, we use the US-based API service. This is the primary endpoint for global use.
api_url = "https://api.pdfrest.com"

# For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#api_url = "https://eu-api.pdfrest.com"

markdown_endpoint_url = api_url+'/markdown'

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

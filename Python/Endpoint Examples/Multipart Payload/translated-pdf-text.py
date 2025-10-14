from requests_toolbelt import MultipartEncoder
import requests
import json

# By default, we use the US-based API service. This is the primary endpoint for global use.
api_url = "https://api.pdfrest.com"

# For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#api_url = "https://eu-api.pdfrest.com"

endpoint_url = api_url+'/translated-pdf-text'

# The endpoint can take a single PDF file or id as input.
mp_encoder = MultipartEncoder(
    fields={
        'file': ('file_name.pdf', open('/path/to/file', 'rb'), 'application/pdf'),
        # Translates text to American English. Format the output_language as a 2-3 character ISO 639 code, optionally with a region/script (e.g., 'en', 'es', 'zh-Hant', 'eng-US').
        'output_language': 'en-US',
    }
)

headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'
}

print("Sending POST request to translated-pdf-text endpoint...")
response = requests.post(endpoint_url, data=mp_encoder, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent=2))
else:
    print(response.text)


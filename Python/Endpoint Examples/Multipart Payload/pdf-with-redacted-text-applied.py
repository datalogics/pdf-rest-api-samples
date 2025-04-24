from requests_toolbelt import MultipartEncoder
import requests
import json

pdf_with_redacted_text_endpoint_url = 'https://api.pdfrest.com/pdf-with-redacted-text-applied'

# The /pdf-with-redacted-text-applied endpoint can take a single PDF file or id as input.
# This sample demonstrates a request to redact text on a document that was processed by /pdf-with-redacted-text-preview
mp_encoder_pdf_with_redacted_text = MultipartEncoder(
    fields={
        'file': ('file_name.pdf', open('/path/to/file', 'rb'), 'application/pdf'),
        'output' : 'example_pdf-with-redacted-text-applied_out',
    }
)

# Let's set the headers that the pdf-with-redacted-text-applied endpoint expects.
# Since MultipartEncoder is used, the 'Content-Type' header gets set to 'multipart/form-data' via the content_type attribute below.
headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_pdf_with_redacted_text.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to pdf-with-redacted-text-applied endpoint...")
response = requests.post(pdf_with_redacted_text_endpoint_url, data=mp_encoder_pdf_with_redacted_text, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

# If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.py' sample.

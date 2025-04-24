from requests_toolbelt import MultipartEncoder
import requests
import json

pdf_with_redacted_text_endpoint_url = 'https://api.pdfrest.com/pdf-with-redacted-text-preview'

redaction_options = [{
        "type": "preset",
        "value": "email",
    },
    {
        "type": "regex",
        "value": "(\\+\\d{1,2}\\s)?\\(?\\d{3}\\)?[\\s.-]\\d{3}[\\s.-]\\d{4}",
    },
    {
        "type": "literal",
        "value": "word",
    }]

mp_encoder_redactedtextPDF = MultipartEncoder(
    fields={
        'file': ('file_name.pdf', open('/path/to/file', 'rb'), 'application/pdf'),
        'redactions': json.dumps(redaction_options),
        'output' : 'example_out'
    }
)

headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_redactedtextPDF.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to pdf-with-redacted-text endpoint...")
response = requests.post(pdf_with_redacted_text_endpoint_url, data=mp_encoder_redactedtextPDF, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

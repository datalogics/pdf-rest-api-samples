from requests_toolbelt import MultipartEncoder
import requests
import json

# Toggle deletion of sensitive files (default: False)
DELETE_SENSITIVE_FILES = False

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

# All files uploaded or generated are automatically deleted based on the 
# File Retention Period as shown on https://pdfrest.com/pricing. 
# For immediate deletion of files, particularly when sensitive data 
# is involved, an explicit delete call can be made to the API.
#
# Deletes all files in the workflow, including outputs. Save all desired files before enabling this step.

if DELETE_SENSITIVE_FILES and response.ok:
    delete_data = { "ids": f"{response_json['inputId']}, {response_json['outputId']}" }
    delete_response = requests.post(url='https://api.pdfrest.com/delete',
                    data=json.dumps(delete_data),
                    headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})
    print("Delete response status code: " + str(delete_response.status_code))
    print(delete_response.text if not delete_response.ok else json.dumps(delete_response.json(), indent = 2))

from requests_toolbelt import MultipartEncoder
import requests
import json

# Toggle deletion of sensitive files (default: False)
DELETE_SENSITIVE_FILES = False

decrypted_pdf_endpoint_url = 'https://api.pdfrest.com/decrypted-pdf'

# The /decrypted-pdf endpoint can take a single PDF file or id as input.
# This sample demonstrates decryption of a PDF with the password 'password'.
mp_encoder_decryptedPdf = MultipartEncoder(
    fields={
        'file': ('file_name.pdf', open('/path/to/file', 'rb'), 'application/pdf'),
        'output' : 'example_decryptedPdf_out',
        'current_open_password': 'password',
    }
)

# Let's set the headers that the decrypted-pdf endpoint expects.
# Since MultipartEncoder is used, the 'Content-Type' header gets set to 'multipart/form-data' via the content_type attribute below.
headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_decryptedPdf.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to decrypted-pdf endpoint...")
response = requests.post(decrypted_pdf_endpoint_url, data=mp_encoder_decryptedPdf, headers=headers)

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
# The following code is an optional step to delete sensitive files
# (unredacted, unencrypted, unrestricted, or unwatermarked) from pdfRest servers.

if DELETE_SENSITIVE_FILES and response.ok:
    result_id = response_json['outputId']
    delete_data = { "ids": result_id }
    delete_response = requests.post(url='https://api.pdfrest.com/delete',
                        data=json.dumps(delete_data),
                        headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})
    print("Delete response status code: " + str(delete_response.status_code))
    print(json.dumps(delete_response.json(), indent = 2))

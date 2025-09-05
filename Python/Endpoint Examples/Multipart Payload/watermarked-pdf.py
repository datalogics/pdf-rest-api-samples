from requests_toolbelt import MultipartEncoder
import requests
import json

# Toggle deletion of sensitive files (default: False)
DELETE_SENSITIVE_FILES = False

watermarked_pdf_endpoint_url = 'https://api.pdfrest.com/watermarked-pdf'

mp_encoder_watermarkedPDF = MultipartEncoder(
    fields={
        'file': ('file_name.pdf', open('/path/to/file', 'rb'), 'application/pdf'),
        'watermark_text': 'watermark',
        'output' : 'example_out'
    }
)

headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_watermarkedPDF.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to watermarked-pdf endpoint...")
response = requests.post(watermarked_pdf_endpoint_url, data=mp_encoder_watermarkedPDF, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

# All files uploaded or generated are automatically deleted based on the 
# File Retention Period as shown on https://pdfrest.com/pricing. 
# For immediate deletion of files, particularly when sensitive data 
# is involved, an explicit delete call can be made to the API.
#
# The following code is an optional step to delete sensitive files
# (unredacted, unencrypted, unrestricted, or unwatermarked) from pdfRest servers.

if DELETE_SENSITIVE_FILES and response.ok:
    delete_data = { "ids": response_json['inputId'][0] }
    delete_response = requests.post(url='https://api.pdfrest.com/delete',
                    data=json.dumps(delete_data),
                    headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})
    print("Delete response status code: " + str(delete_response.status_code))
    print(delete_response.text if not delete_response.ok else json.dumps(delete_response.json(), indent = 2))

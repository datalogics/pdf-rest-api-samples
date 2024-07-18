from requests_toolbelt import MultipartEncoder
import requests
import json

pdf_with_ocr_text_endpoint_url = 'https://api.pdfrest.com/pdf-with-ocr-text'

# The /pdf-with-ocr-text endpoint can take a single PDF file or id as input.
# This sample demonstrates a request to add text to a document by using OCR on images of text.
mp_encoder_pdf_with_ocr_text = MultipartEncoder(
    fields={
        'file': ('file_name', open('/path/to/file', 'rb'), 'application/pdf'),
        'output' : 'example_pdf-with-ocr-text_out',
    }
)

# Let's set the headers that the pdf-with-ocr-text endpoint expects.
# Since MultipartEncoder is used, the 'Content-Type' header gets set to 'multipart/form-data' via the content_type attribute below.
headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_pdf_with_ocr_text.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to pdf-with-ocr-text endpoint...")
response = requests.post(pdf_with_ocr_text_endpoint_url, data=mp_encoder_pdf_with_ocr_text, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

# If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.py' sample.

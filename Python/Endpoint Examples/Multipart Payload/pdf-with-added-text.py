from requests_toolbelt import MultipartEncoder
import requests
import json

pdf_with_added_text_endpoint_url = 'https://api.pdfrest.com/pdf-with-added-text'

text_options = [{
    "font":"Times New Roman",
    "max_width":"175",
    "opacity":"1",
    "page":"1",
    "rotation":"0",
    "text":"sample text in PDF",
    "text_color_rgb":"0,0,0",
    "text_size":"30",
    "x":"72",
    "y":"144"
}]

mp_encoder_addedtextPDF = MultipartEncoder(
    fields={
        'file': ('file_name.pdf', open('/path/to/file', 'rb'), 'application/pdf'),
        'text_objects': json.dumps(text_options),
        'output' : 'example_out'
    }
)

headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_addedtextPDF.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to pdf-with-added-text endpoint...")
response = requests.post(pdf_with_added_text_endpoint_url, data=mp_encoder_addedtextPDF, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

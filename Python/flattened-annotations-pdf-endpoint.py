from requests_toolbelt import MultipartEncoder
import requests
import json

flattened_annotations_pdf_endpoint_url = 'https://api.pdfrest.com/flattened-annotations-pdf'

mp_encoder_flattenedPDF = MultipartEncoder(
    fields={
        'file': ('ducky.pdf', open('../Sample_Input/ducky.pdf', 'rb'), 'application/pdf'),
        'output' : 'example_out'
    }
)

headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_flattenedPDF.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to flattened-annotations-pdf endpoint...")
response = requests.post(flattened_annotations_pdf_endpoint_url, data=mp_encoder_flattenedPDF, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

from requests_toolbelt import MultipartEncoder
import requests
import json

# By default, we use the US-based API service. This is the primary endpoint for global use.
api_url = "https://api.pdfrest.com"

# For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#api_url = "https://eu-api.pdfrest.com"

pdf_with_page_boxes_endpoint_url = api_url+'/pdf-with-page-boxes-set'

box_options = {
    "boxes": [
        {
            "box": "media",
            "pages": [
                {
                    "range": "1",
                    "left": 100,
                    "top": 100,
                    "bottom": 100,
                    "right": 100
                }
            ]
        }
    ]
}

mp_encoder_setBoxesPDF = MultipartEncoder(
    fields={
        'file': ('file_name.pdf', open('/path/to/file', 'rb'), 'application/pdf'),
        'boxes': json.dumps(box_options),
        'output' : 'example_out'
    }
)

headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_setBoxesPDF.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to pdf-with-page-boxes-set endpoint...")
response = requests.post(pdf_with_page_boxes_endpoint_url, data=mp_encoder_setBoxesPDF, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

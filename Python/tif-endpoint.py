from requests_toolbelt import MultipartEncoder
import requests
import json

tif_endpoint_url = 'https://api.pdfrest.com/tif'

# The /tif endpoint can take a single PDF file or id as input and turn them into TIFF image files.
# This sample takes in a PDF and converts all pages into grayscale TIFF files.
mp_encoder_tif = MultipartEncoder(
    fields={
        'file': ('pdfRest.pdf', open('../Sample_Input/pdfRest.pdf', 'rb'), 'application/pdf'),
        'pages': '1-last',
        'resolution': '600',
        'color_model': 'gray',
        'output' : 'example_tif_out',
    }
)

# Let's set the headers that the tif endpoint expects.
# Since MultipartEncoder is used, the 'Content-Type' header gets set to 'multipart/form-data' via the content_type attribute below.
headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_tif.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to tif endpoint...")
response = requests.post(tif_endpoint_url, data=mp_encoder_tif, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

# If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.py' sample.

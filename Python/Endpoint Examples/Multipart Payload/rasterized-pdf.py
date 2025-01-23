from requests_toolbelt import MultipartEncoder
import requests
import json

rasterize_endpoint_url = 'https://api.pdfrest.com/rasterized-pdf'

# The /rasterized endpoint can take a single PDF file or id as input and turn it into a rasterized PDF file.
mp_encoder_rasterize = MultipartEncoder(
    fields={
        'file': ('file_name.pdf', open('/path/to/file', 'rb'), 'application/pdf'),
        'output' : 'example_rasterize_out',
    }
)

# Let's set the headers that the rasterized-pdf endpoint expects.
# Since MultipartEncoder is used, the 'Content-Type' header gets set to 'multipart/form-data' via the content_type attribute below.
headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_rasterize.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to rasterized-pdf endpoint...")
response = requests.post(rasterize_endpoint_url, data=mp_encoder_rasterize, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

# If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.py' sample.

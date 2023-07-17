from requests_toolbelt import MultipartEncoder
import requests
import json

pdfa_endpoint_url = 'https://api.pdfrest.com/pdfa'

# The /pdfa endpoint can take a single PDF file or id as input.
mp_encoder_pdfa = MultipartEncoder(
    fields={
        'file': ('ducky.pdf', open('../Sample_Input/ducky.pdf', 'rb'), 'application/pdf'),
        'output_type': 'PDF/A-1b',
        'rasterize_if_errors_encountered': 'on',
        'output' : 'example_pdfa_out',
    }
)

# Let's set the headers that the pdfa endpoint expects.
# Since MultipartEncoder is used, the 'Content-Type' header gets set to 'multipart/form-data' via the content_type attribute below.
headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_pdfa.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to pdfa endpoint...")
response = requests.post(pdfa_endpoint_url, data=mp_encoder_pdfa, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

# If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.py' sample.

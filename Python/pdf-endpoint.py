from requests_toolbelt import MultipartEncoder
import requests
import json

pdf_endpoint_url = 'https://api.pdfrest.com/pdf'

# The /pdf endpoint can take a single file, id, or url as input. 
# This sample passes a tif file to the endpoint, but there's a variety of input file types that are accepted by this endpoint.
# The 'image/tiff' string below is known as a MIME type, which is a label used to identify the type of a file so that it is handled properly by software.
# Please see https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types for more information about MIME types.
mp_encoder_pdf = MultipartEncoder(
    fields={
        'file': ('rainbow.tif', open('../Sample_Input/rainbow.tif', 'rb'), 'image/tiff'),
        'output' : 'example_pdf_out',
    }
)

# Let's set the headers that the pdf endpoint expects.
# Since MultipartEncoder is used, the 'Content-Type' header gets set to 'multipart/form-data' via the content_type attribute below.
headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_pdf.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to pdf endpoint...")
response = requests.post(pdf_endpoint_url, data=mp_encoder_pdf, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

# If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.py' sample.

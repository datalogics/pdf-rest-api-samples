from requests_toolbelt import MultipartEncoder
import requests
import json

pdf_info_endpoint_url = 'https://api.pdfrest.com/pdf-info'

# The /pdf-info endpoint can take a single PDF file or id as input.
#This sample demonstrates querying the title, page count, document language and author
mp_encoder_pdfInfo = MultipartEncoder(
    fields={
        'file': ('ducky.pdf', open('../Sample_Input/ducky.pdf', 'rb'), 'application/pdf'),
        'queries': 'title,page_count,doc_language,author',
    }
)

# Let's set the headers that the pdf-info endpoint expects.
# Since MultipartEncoder is used, the 'Content-Type' header gets set to 'multipart/form-data' via the content_type attribute below.
headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_pdfInfo.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to pdf-info endpoint...")
response = requests.post(pdf_info_endpoint_url, data=mp_encoder_pdfInfo, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

from requests_toolbelt import MultipartEncoder
import requests
import json

# By default, we use the US-based API service. This is the primary endpoint for global use.
api_url = "https://api.pdfrest.com"

# For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#api_url = "https://eu-api.pdfrest.com"

blank_pdf_endpoint_url = api_url + '/blank-pdf'

# The /blank-pdf endpoint generates an empty PDF, so no file upload is required.
mp_encoder_blank_pdf = MultipartEncoder(
    fields={
        'page_size': 'letter',
        'page_count': '3',
        'page_orientation': 'portrait',
    }
)

# Let's set the headers that the blank-pdf endpoint expects.
headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_blank_pdf.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'  # place your api key here
}

print("Sending POST request to blank-pdf endpoint...")
response = requests.post(blank_pdf_endpoint_url, data=mp_encoder_blank_pdf, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent=2))
else:
    print(response.text)

# If you would like to download the file instead of getting the JSON response, please see the 'get-resource-multipart.py' sample.

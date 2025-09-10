from requests_toolbelt import MultipartEncoder
import requests
import json

# By default, we use the US-based API service. This is the primary endpoint for global use.
api_url = "https://api.pdfrest.com"

# For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#api_url = "https://eu-api.pdfrest.com"

html_endpoint_url = api_url+'/html'

# The /html endpoint can take a string of HTML content and convert it to a HTML (.html) file.
# This sample takes in a string of HTML content that displays "Hello World!" and turns it into a HTML file.
mp_encoder_html = MultipartEncoder(
    fields={
        'content': '<html><head><title>Web Page</title></head><body>Hello World!</body></html>',
        'output': 'example_html_out'
    }
)

# Let's set the headers that the html endpoint expects.
# Since MultipartEncoder is used, the 'Content-Type' header gets set to 'multipart/form-data' via the content_type attribute below.
headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_html.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to html endpoint...")
response = requests.post(html_endpoint_url, data=mp_encoder_html, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

# If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.py' sample.

import requests
import json

# By default, we use the US-based API service. This is the primary endpoint for global use.
api_url = "https://api.pdfrest.com"

# For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#api_url = "https://eu-api.pdfrest.com"

blank_pdf_data = {
    "page_size": "letter",
    "page_count": 3,
    "page_orientation": "portrait"
}

print("Requesting blank PDF...")
response = requests.post(
    url=api_url + '/blank-pdf',
    data=json.dumps(blank_pdf_data),
    headers={
        'Content-Type': 'application/json',
        'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'  # place your api key here
    }
)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent=2))
else:
    print(response.text)

# If you would like to download the file instead of getting the JSON response, please see the 'get-resource.py' sample.

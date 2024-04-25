from requests_toolbelt import MultipartEncoder
import requests
import json

api_polling_endpoint_url = 'https://api.pdfrest.com/request-status'

request_id = 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place requestId to poll here

api_polling_endpoint_url = f'https://api.pdfrest.com/request-status/{request_id}'

headers = {
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending GET request to request-status endpoint...")
response = requests.get(api_polling_endpoint_url, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

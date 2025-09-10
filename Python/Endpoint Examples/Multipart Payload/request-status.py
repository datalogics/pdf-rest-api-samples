from requests_toolbelt import MultipartEncoder
import requests
import json
import time

# By default, we use the US-based API service. This is the primary endpoint for global use.
api_url = "https://api.pdfrest.com"

# For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#api_url = "https://eu-api.pdfrest.com"

api_key = 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here

pdfa_endpoint_url = api_url+'/pdfa'

mp_encoder_pdfa = MultipartEncoder(
    fields={
        'file': ('file_name.pdf', open('/path/to/file.pdf', 'rb'), 'application/pdf'),
        'output_type': 'PDF/A-1b',
    }
)

# Send a request to a pdfRest tool with the Response-Type header to get a request ID,
pdfa_headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_pdfa.content_type,
    'Response-Type': "requestId",
    'Api-Key': api_key
}

print("Sending POST request to pdfa endpoint...")
response = requests.post(pdfa_endpoint_url, data=mp_encoder_pdfa, headers=pdfa_headers)

print("Response status code: " + str(response.status_code))

if response.ok:

    response_json = response.json()
    request_id = response_json["requestId"]
    api_polling_endpoint_url = f'{api_url}/request-status/{request_id}'

    headers = {
        'Api-Key': api_key
    }

    print("Sending GET request to request-status endpoint...")
    response = requests.get(api_polling_endpoint_url, headers=headers)

    print("Response status code: " + str(response.status_code))

    if response.ok:
        response_json = response.json()
        while response_json["status"] == "pending":
            # This example will get the request status every 5 seconds until the request is completed.
            print(json.dumps(response_json, indent = 2))
            time.sleep(5)
            response = requests.get(api_polling_endpoint_url, headers=headers)
            response_json = response.json()
        print(json.dumps(response_json, indent = 2))
    else:
        print(response.text)

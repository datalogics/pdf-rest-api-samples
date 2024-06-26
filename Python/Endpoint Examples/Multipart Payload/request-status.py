from requests_toolbelt import MultipartEncoder
import requests
import json
import time

api_key = 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here

pdfa_endpoint_url = 'https://api.pdfrest.com/pdfa'

mp_encoder_pdfa = MultipartEncoder(
    fields={
        'file': ('file_name.pdf', open('/path/to/file.pdf', 'rb'), 'application/pdf'),
        'output_type': 'PDF/A-1b',
    }
)

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
    api_polling_endpoint_url = f'https://api.pdfrest.com/request-status/{request_id}'

    headers = {
        'Api-Key': api_key
    }

    print("Sending GET request to request-status endpoint...")
    response = requests.get(api_polling_endpoint_url, headers=headers)

    print("Response status code: " + str(response.status_code))

    if response.ok:
        response_json = response.json()
        while response_json["status"] == "pending":
            print(json.dumps(response_json, indent = 2))
            time.sleep(5)
            response = requests.get(api_polling_endpoint_url, headers=headers)
            response_json = response.json()
        print(json.dumps(response_json, indent = 2))
    else:
        print(response.text)

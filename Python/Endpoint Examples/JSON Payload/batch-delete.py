import requests
import json

delete_data = { "ids" : "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx, xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" }


print("Processing files...")
delete_response = requests.post(url='https://api.pdfrest.com/delete',
                data=json.dumps(delete_data),
                headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})



print("Processing response status code: " + str(delete_response.status_code))
if delete_response.ok:
    delete_response_json = delete_response.json()
    print(json.dumps(delete_response_json, indent = 2))

else:
    print(delete_response.text)

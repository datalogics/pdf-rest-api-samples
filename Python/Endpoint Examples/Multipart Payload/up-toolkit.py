import requests
import json

# up-forms and up-office can be used to query the other tools
up_url = f"https://api.pdfrest.com/up-toolkit"

print("Sending GET request to /up-toolkit endpoint...")
response = requests.get(up_url)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

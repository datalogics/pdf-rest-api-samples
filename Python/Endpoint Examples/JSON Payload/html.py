import requests
import json


data = {
    'content': '<html><head><title>Web Page</title></head><body>Hello World!</body></html>',
    'output': 'example_html_out'
}
print("Processing...")
response = requests.post(url='https://api.pdfrest.com/html',
                    data=json.dumps(data),
                    headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})

print("HTML response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))



else:
    print(response.text)

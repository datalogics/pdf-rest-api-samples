import requests
import json

with open('/path/to/file', 'rb') as f:
    upload_data = f.read()aaa

print("Uploading file...")
upload_response = requests.post(url='https://api.pdfrest.com/upload',
                    data=upload_data,
                    headers={'Content-Type': 'application/octet-stream', 'Content-Filename': 'file.zip', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})

print("Upload response status code: " + str(upload_response.status_code))

if upload_response.ok:
    upload_response_json = upload_response.json()
    print(json.dumps(upload_response_json, indent = 2))


    uploaded_id = upload_response_json['files'][0]['id']
    unzip_data = { "id" : uploaded_id }
    print(json.dumps(unzip_data, indent = 2))


    print("Processing file...")
    unzip_response = requests.post(url='https://api.pdfrest.com/unzip',
                        data=json.dumps(unzip_data),
                        headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})



    print("Processing response status code: " + str(unzip_response.status_code))
    if unzip_response.ok:
        unzip_response_json = unzip_response.json()
        print(json.dumps(unzip_response_json, indent = 2))

    else:
        print(unzip_response.text)
else:
    print(upload_response.text)

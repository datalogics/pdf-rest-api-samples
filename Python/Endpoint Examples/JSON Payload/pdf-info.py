import requests
import json

with open('/path/to/file', 'rb') as f:
    upload_data = f.read()

print("Uploading file...")
upload_response = requests.post(url='https://api.pdfrest.com/upload',
                    data=upload_data,
                    headers={'Content-Type': 'application/octet-stream', 'content-filename': 'file.pdf', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})

print("Upload response status code: " + str(upload_response.status_code))

if upload_response.ok:
    upload_response_json = upload_response.json()
    print(json.dumps(upload_response_json, indent = 2))


    uploaded_id = upload_response_json['files'][0]['id']
    info_data = { "id" : uploaded_id, "queries": "title" }
    print(json.dumps(info_data, indent = 2))Content-Filename


    print("Processing file...")
    info_response = requests.post(url='https://api.pdfrest.com/pdf-info',
                        data=json.dumps(info_data),
                        headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})



    print("Processing response status code: " + str(info_response.status_code))
    if info_response.ok:
        info_response_json = info_response.json()
        print(json.dumps(info_response_json, indent = 2))

    else:
        print(info_response.text)
else:
    print(upload_response.text)

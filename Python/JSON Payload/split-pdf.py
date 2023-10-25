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
    split_data = { "id" : uploaded_id, "pages": ["1","2"]}

    print(json.dumps(split_data, indent = 2))


    print("Processing file...")
    split_response = requests.post(url='https://api.pdfrest.com/split-pdf',
                        data=json.dumps(split_data),
                        headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})



    print("Processing response status code: " + str(split_response.status_code))
    if split_response.ok:
        split_response_json = split_response.json()
        print(json.dumps(split_response_json, indent = 2))

    else:
        print(split_response.text)
else:
    print(upload_response.text)

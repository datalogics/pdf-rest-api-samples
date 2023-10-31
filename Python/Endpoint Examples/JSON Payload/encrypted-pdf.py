import requests
import json

with open('/path/to/file', 'rb') as f:
    upload_data = f.read()

print("Uploading file...")
upload_response = requests.post(url='https://api.pdfrest.com/upload',
                    data=upload_data,
                    headers={'Content-Type': 'application/octet-stream', 'Content-Filename': 'file.pdf', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})

print("Upload response status code: " + str(upload_response.status_code))

if upload_response.ok:
    upload_response_json = upload_response.json()
    print(json.dumps(upload_response_json, indent = 2))


    uploaded_id = upload_response_json['files'][0]['id']
    encrypt_data = { "id" : uploaded_id, 'new_open_password': 'password' }
    print(json.dumps(encrypt_data, indent = 2))

    print("Processing file...")
    encrypt_response = requests.post(url='https://api.pdfrest.com/encrypted-pdf',
                        data=json.dumps(encrypt_data),
                        headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})



    print("Processing response status code: " + str(encrypt_response.status_code))
    if encrypt_response.ok:
        encrypt_response_json = encrypt_response.json()
        print(json.dumps(encrypt_response_json, indent = 2))

    else:
        print(encrypt_response.text)
else:
    print(upload_response.text)

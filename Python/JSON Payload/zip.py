import requests
import json

with open('/path/to/file1', 'rb') as f:
    upload_data1 = f.read()

print("Uploading file1...")
upload_response1 = requests.post(url='https://api.pdfrest.com/upload',
                    data=upload_data1,
                    headers={'Content-Type': 'application/octet-stream', 'content-filename': 'file1.pdf', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})

print("Upload response status code: " + str(upload_response1.status_code))


with open('/path/to/file2', 'rb') as f:
    upload_data2 = f.read()

print("Uploading file2...")
upload_response2 = requests.post(url='https://api.pdfrest.com/upload',
                    data=upload_data2,
                    headers={'Content-Type': 'application/octet-stream', 'content-filename': 'file2.pdf', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})

print("Upload response status code: " + str(upload_response2.status_code))

if upload_response1.ok and upload_response2.ok:
    upload_response_json1 = upload_response1.json()
    print(json.dumps(upload_response_json1, indent = 2))

    upload_response_json2 = upload_response2.json()
    print(json.dumps(upload_response_json2, indent = 2))


    uploaded_id1 = upload_response_json1['files'][0]['id']
    uploaded_id2 = upload_response_json2['files'][0]['id']
    zip_data = { "id" : [uploaded_id1, uploaded_id2] }
    print(json.dumps(zip_data, indent = 2))


    print("Processing file...")
    zip_response = requests.post(url='https://api.pdfrest.com/zip',
                        data=json.dumps(zip_data),
                        headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})



    print("Processing response status code: " + str(zip_response.status_code))
    if zip_response.ok:
        zip_response_json = zip_response.json()
        print(json.dumps(zip_response_json, indent = 2))

    else:
        print(zip_response.text)
else:
    print(upload_response1.text)
    print(upload_response2.text)

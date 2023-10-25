import requests
import json

with open('/path/to/file', 'rb') as f:
    upload_data1 = f.read()

print("Uploading file1...")
upload_response1 = requests.post(url='https://api.pdfrest.com/upload',
                    data=upload_data1,
                    headers={'Content-Type': 'application/octet-stream', 'content-filename': 'file.pdf', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})

print("Upload response status code: " + str(upload_response1.status_code))


with open('/path/to/file', 'rb') as f:
    upload_data2 = f.read()

print("Uploading file2...")
upload_response2 = requests.post(url='https://api.pdfrest.com/upload',
                    data=upload_data2,
                    headers={'Content-Type': 'application/octet-stream', 'content-filename': 'file.png', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})

print("Upload response status code: " + str(upload_response2.status_code))

if upload_response1.ok and upload_response2.ok:
    upload_response_json1 = upload_response1.json()
    print(json.dumps(upload_response_json1, indent = 2))

    upload_response_json2 = upload_response2.json()
    print(json.dumps(upload_response_json2, indent = 2))


    uploaded_id1 = upload_response_json1['files'][0]['id']
    uploaded_id2 = upload_response_json2['files'][0]['id']
    added_image_data = { "id" : uploaded_id1, "image_id": uploaded_id2, "x":0, "y":0, "page":1 }
    print(json.dumps(added_image_data, indent = 2))


    print("Processing file...")
    added_image_response = requests.post(url='https://api.pdfrest.com/pdf-with-added-image',
                        data=json.dumps(added_image_data),
                        headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})



    print("Processing response status code: " + str(added_image_response.status_code))
    if added_image_response.ok:
        added_image_response_json = added_image_response.json()
        print(json.dumps(added_image_response_json, indent = 2))

    else:
        print(added_image_response.text)
else:
    print(upload_response1.text)
    print(upload_response2.text)

import requests
import json

with open('/path/to/first_file', 'rb') as f: # Note that the full file name plus extension needs to be reflected in 'Content-Filename below'
    upload_first_file_data = f.read()

print("Uploading first file...")
upload_first_file_response = requests.post(url='https://api.pdfrest.com/upload',
                    data=upload_first_file_data,
                    headers={'Content-Type': 'application/octet-stream', 'Content-Filename': 'first_file', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})

print("First upload response status code: " + str(upload_first_file_response.status_code))


with open('/path/to/second_file', 'rb') as f: # Note that the full file name plus extension needs to be reflected in 'Content-Filename below'
    upload_second_file_data = f.read()

print("Uploading second file...")
upload_second_file_response = requests.post(url='https://api.pdfrest.com/upload',
                    data=upload_second_file_data,
                    headers={'Content-Type': 'application/octet-stream', 'content-filename': 'second_file', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})

print("Second upload response status code: " + str(upload_second_file_response.status_code))

if upload_first_file_response.ok and upload_second_file_response.ok:
    upload_first_file_response_json = upload_first_file_response.json()
    print(json.dumps(upload_first_file_response_json, indent = 2))

    upload_second_file_response_json = upload_second_file_response.json()
    print(json.dumps(upload_second_file_response_json, indent = 2))


    uploaded_first_file_id = upload_first_file_response_json['files'][0]['id']
    uploaded_second_file_id = upload_second_file_response_json['files'][0]['id']
    zip_data = { "id" : [uploaded_first_file_id, uploaded_second_file_id] }
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
    print(upload_first_file_response.text)
    print(upload_second_file_response.text)

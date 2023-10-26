import requests
import json

with open('/path/to/first_file', 'rb') as f:
    upload_first_file_data = f.read()

print("Uploading first PDF file...")
upload_first_file_response = requests.post(url='https://api.pdfrest.com/upload',
                    data=upload_first_file_data,
                    headers={'Content-Type': 'application/octet-stream', 'content-filename': 'first_file.pdf', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})

print("First upload response status code: " + str(upload_first_file_response.status_code))


with open('/path/to/second_file', 'rb') as f:
    upload_second_file_data  = f.read()

print("Uploading second PDF file...")
upload_second_file_response = requests.post(url='https://api.pdfrest.com/upload',
                    data=upload_second_file_data ,
                    headers={'Content-Type': 'application/octet-stream', 'content-filename': 'second_file.pdf', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})

print("Second upload response status code: " + str(upload_second_file_response.status_code))

if upload_first_file_response.ok and upload_second_file_response.ok:
    upload_first_file_response_json = upload_first_file_response.json()
    print(json.dumps(upload_first_file_response_json, indent = 2))

    upload_second_file_response_json = upload_second_file_response.json()
    print(json.dumps(upload_second_file_response_json, indent = 2))


    uploaded_first_file_id = upload_first_file_response_json['files'][0]['id']
    uploaded_second_file_id = upload_second_file_response_json['files'][0]['id']
    merge_data = { "id" : [uploaded_first_file_id, uploaded_second_file_id], "pages":[1,1], "type":["id", "id"] }
    print(json.dumps(merge_data, indent = 2))


    print("Processing file...")
    merge_response = requests.post(url='https://api.pdfrest.com/merged-pdf',
                        data=json.dumps(merge_data),
                        headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})



    print("Processing response status code: " + str(merge_response.status_code))
    if merge_response.ok:
        merge_response_json = merge_response.json()
        print(json.dumps(merge_response_json, indent = 2))

    else:
        print(merge_response.text)
else:
    print(upload_first_file_response.text)
    print(upload_second_file_response.text)

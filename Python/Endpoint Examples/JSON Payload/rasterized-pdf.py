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
    rasterize_data = { "id" : uploaded_id }
    print(json.dumps(rasterize_data, indent = 2))


    print("Processing file...")
    rasterize_response = requests.post(url='https://api.pdfrest.com/rasterized-pdf',
                        data=json.dumps(rasterize_data),
                        headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})



    print("Processing response status code: " + str(rasterize_response.status_code))
    if rasterize_response.ok:
        rasterize_response_json = rasterize_response.json()
        print(json.dumps(rasterize_response_json, indent = 2))

    else:
        print(rasterize_response.text)
else:
    print(upload_response.text)

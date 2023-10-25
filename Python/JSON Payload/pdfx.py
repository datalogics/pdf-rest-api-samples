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
    pdfx_data = { "id" : uploaded_id, "output_type": "PDF/X-1a" }
    print(json.dumps(pdfx_data, indent = 2))


    print("Processing file...")
    pdfx_response = requests.post(url='https://api.pdfrest.com/pdfx',
                        data=json.dumps(pdfx_data),
                        headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})



    print("Processing response status code: " + str(pdfx_response.status_code))
    if pdfx_response.ok:
        pdfx_response_json = pdfx_response.json()
        print(json.dumps(pdfx_response_json, indent = 2))

    else:
        print(pdfx_response.text)
else:
    print(upload_response.text)

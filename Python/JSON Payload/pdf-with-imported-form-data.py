import requests
import json

with open('/path/to/pdf_file', 'rb') as f:
    upload_pdf_data = f.read()

print("Uploading PDF file...")
upload_pdf_response = requests.post(url='https://api.pdfrest.com/upload',
                    data=upload_pdf_data,
                    headers={'Content-Type': 'application/octet-stream', 'content-filename': 'pdf_file.pdf', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})

print("Upload PDF response status code: " + str(upload_pdf_response.status_code))


with open('/path/to/data_file', 'rb') as f:
    upload_data_data = f.read()

print("Uploading data file...")
upload_data_response = requests.post(url='https://api.pdfrest.com/upload',
                    data=upload_data_data,
                    headers={'Content-Type': 'application/octet-stream', 'Content-Filename': 'data_file.xml', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})

print("Upload data file response status code: " + str(upload_data_response.status_code))

if upload_pdf_response.ok and upload_data_response.ok:
    upload_pdf_response_json = upload_pdf_response.json()
    print(json.dumps(upload_pdf_response_json, indent = 2))

    upload_data_response_json = upload_data_response.json()
    print(json.dumps(upload_data_response_json, indent = 2))


    uploaded_pdf_id = upload_pdf_response_json['files'][0]['id']
    uploaded_data_id = upload_data_response_json['files'][0]['id']
    added_image_data = { "id" : uploaded_pdf_id, "data_id": uploaded_data_id }
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
    print(upload_pdf_response.text)
    print(upload_data_response.text)

import requests
import json

with open('/path/to/pdf_file', 'rb') as f:
    upload_pdf_data = f.read()

print("Uploading PDF file...")
upload_pdf_response = requests.post(url='https://api.pdfrest.com/upload',
                    data=upload_pdf_data,
                    headers={'Content-Type': 'application/octet-stream', 'content-filename': 'pdf_file.pdf', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})

print("Upload PDF response status code: " + str(upload_pdf_response.status_code))


with open('/path/to/file_attachment', 'rb') as f:
    upload_attachment_data = f.read()

print("Uploading file attachment...")
upload_attachment_response = requests.post(url='https://api.pdfrest.com/upload',
                    data=upload_attachment_data,
                    headers={'Content-Type': 'application/octet-stream', 'Content-Filename': 'data_file.xml', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})

print("Upload data file response status code: " + str(upload_attachment_response.status_code))

if upload_pdf_response.ok and upload_attachment_response.ok:
    upload_pdf_response_json = upload_pdf_response.json()
    print(json.dumps(upload_pdf_response_json, indent = 2))

    upload_attachment_response_json = upload_attachment_response.json()
    print(json.dumps(upload_attachment_response_json, indent = 2))


    uploaded_pdf_id = upload_pdf_response_json['files'][0]['id']
    uploaded_attachment_id = upload_attachment_response_json['files'][0]['id']
    added_attachment_data = { "id" : uploaded_pdf_id, "id_to_attach": uploaded_attachment_id }
    print(json.dumps(added_attachment_data, indent = 2))


    print("Processing file...")
    added_attachment_response = requests.post(url='https://api.pdfrest.com/pdf-with-added-attachment',
                        data=json.dumps(added_attachment_data),
                        headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})



    print("Processing response status code: " + str(added_attachment_response.status_code))
    if added_attachment_response.ok:
        added_attachment_response_json = added_attachment_response.json()
        print(json.dumps(added_attachment_response_json, indent = 2))

    else:
        print(added_attachment_response.text)
else:
    print(upload_pdf_response.text)
    print(upload_attachment_response.text)

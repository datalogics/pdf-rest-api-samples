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
    pdf_with_ocr_text_data = { "id" : uploaded_id }
    print(json.dumps(pdf_with_ocr_text_data, indent = 2))


    print("Processing file...")
    pdf_with_ocr_text_response = requests.post(url='https://api.pdfrest.com/pdf-with-ocr-text',
                        data=json.dumps(pdf_with_ocr_text_data),
                        headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})



    print("Processing response status code: " + str(pdf_with_ocr_text_response.status_code))
    if pdf_with_ocr_text_response.ok:
        pdf_with_ocr_text_response_json = pdf_with_ocr_text_response.json()
        print(json.dumps(pdf_with_ocr_text_response_json, indent = 2))

    else:
        print(pdf_with_ocr_text_response.text)
else:
    print(upload_response.text)

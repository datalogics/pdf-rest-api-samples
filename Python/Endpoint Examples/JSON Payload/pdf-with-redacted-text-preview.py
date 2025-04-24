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
    redaction_options = [{
        "type": "preset",
        "value": "email",
    },
    {
        "type": "regex",
        "value": "(\\+\\d{1,2}\\s)?\\(?\\d{3}\\)?[\\s.-]\\d{3}[\\s.-]\\d{4}",
    },
    {
        "type": "literal",
        "value": "word",
    }]
    redact_text_data = { "id" : uploaded_id, "text_objects": json.dumps(redaction_options) }

    print(json.dumps(redact_text_data, indent = 2))


    print("Processing file...")
    redact_text_response = requests.post(url='https://api.pdfrest.com/pdf-with-redacted-text-preview',
                        data=json.dumps(redact_text_data),
                        headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})



    print("Processing response status code: " + str(redact_text_response.status_code))
    if redact_text_response.ok:
        redact_text_response_json = redact_text_response.json()
        print(json.dumps(redact_text_response_json, indent = 2))

    else:
        print(redact_text_response.text)
else:
    print(upload_response.text)

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
    text_options = [{
        "font":"Times New Roman",
        "max_width":"175",
        "opacity":"1",
        "page":"1",
        "rotation":"0",
        "text":"sample text in PDF",
        "text_color_rgb":"0,0,0",
        "text_size":"30",
        "x":"72",
        "y":"144"
    }]
    add_text_data = { "id" : uploaded_id, "text_objects": json.dumps(text_options) }

    print(json.dumps(add_text_data, indent = 2))


    print("Processing file...")
    add_text_response = requests.post(url='https://api.pdfrest.com/pdf-with-added-text',
                        data=json.dumps(add_text_data),
                        headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})



    print("Processing response status code: " + str(add_text_response.status_code))
    if add_text_response.ok:
        add_text_response_json = add_text_response.json()
        print(json.dumps(add_text_response_json, indent = 2))

    else:
        print(add_text_response.text)
else:
    print(upload_response.text)

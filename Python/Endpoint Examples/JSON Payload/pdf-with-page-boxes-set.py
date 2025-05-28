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
    box_options = {
        "boxes": [
            {
                "box": "media",
                "pages": [
                    {
                        "range": "1",
                        "left": 100,
                        "top": 100,
                        "bottom": 100,
                        "right": 100
                    }
                ]
            }
        ]
    }
    boxes_data = { "id" : uploaded_id, "boxes": json.dumps(box_options) }

    print(json.dumps(boxes_data, indent = 2))


    print("Processing file...")
    boxes_response = requests.post(url='https://api.pdfrest.com/pdf-with-page-boxes-set',
                        data=json.dumps(boxes_data),
                        headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})



    print("Processing response status code: " + str(boxes_response.status_code))
    if boxes_response.ok:
        boxes_response_json = boxes_response.json()
        print(json.dumps(boxes_response_json, indent = 2))

    else:
        print(boxes_response.text)
else:
    print(upload_response.text)

import requests
import json

def upload_file(file_path, file_name):
    with open(file_path, 'rb') as f:
        upload_data = f.read()
        print("Uploading file...")
        upload_response = requests.post(url='https://api.pdfrest.com/upload',
                            data=upload_data,
                            headers={'Content-Type': 'application/octet-stream', 'Content-Filename': file_name, "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})
        print("Upload response status code: " + str(upload_response.status_code))

        if upload_response.ok:
            upload_response_json = upload_response.json()
            print(json.dumps(upload_response_json, indent = 2))

            return upload_response_json['files'][0]['id']
        else:
            print(upload_response.text)
            return ""

input_pdf = '/path/to/input.pdf'
input_id = upload_file(input_pdf, 'input.pdf')

certificate_path = '/path/to/certificate.pem'
certificate_id = upload_file(certificate_path, 'certificate.pem')

private_key_path = '/path/to/private_key.pem'
private_key_id = upload_file(private_key_path, 'private_key.pem')

signature_config = {
    "type": "new",
    "name": "esignature",
    "location": {
        "bottom_left": { "x": "0", "y": "0" },
        "top_right": { "x": "216", "y": "72" },
        "page": 1
    },
    "display": {
        "include_datetime": "true"
    }
}
boxes_data = { "id" : input_id, "certificate_id": certificate_id, "private_key_id": private_key_id, "signature_configuration": json.dumps(signature_config) }

print(json.dumps(boxes_data, indent = 2))


print("Processing file...")
boxes_response = requests.post(url='https://api.pdfrest.com/signed-pdf',
                    data=json.dumps(boxes_data),
                    headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})



print("Processing response status code: " + str(boxes_response.status_code))
if boxes_response.ok:
    boxes_response_json = boxes_response.json()
    print(json.dumps(boxes_response_json, indent = 2))

else:
    print(boxes_response.text)

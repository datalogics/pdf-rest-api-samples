import requests
import json

# Toggle deletion of sensitive files (default: False)
DELETE_SENSITIVE_FILES = False

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
    watermark_data = { "id" : uploaded_id, "watermark_text": "I am watermarked" }

    print(json.dumps(watermark_data, indent = 2))


    print("Processing file...")
    watermark_response = requests.post(url='https://api.pdfrest.com/watermarked-pdf',
                        data=json.dumps(watermark_data),
                        headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})



    print("Processing response status code: " + str(watermark_response.status_code))
    if watermark_response.ok:
        watermark_response_json = watermark_response.json()
        print(json.dumps(watermark_response_json, indent = 2))

        # All files uploaded or generated are automatically deleted based on the 
        # File Retention Period as shown on https://pdfrest.com/pricing. 
        # For immediate deletion of files, particularly when sensitive data 
        # is involved, an explicit delete call can be made to the API.
        #
        # Deletes all files in the workflow, including outputs. Save all desired files before enabling this step.

        if DELETE_SENSITIVE_FILES:
            watermarked_output_id = watermark_response_json['outputId']
            delete_data = { "ids": f"{uploaded_id}, {watermarked_output_id}" }
            delete_response = requests.post(url='https://api.pdfrest.com/delete',
                                data=json.dumps(delete_data),
                                headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})
            if delete_response.ok:
                print(json.dumps(delete_response.json(), indent = 2))
            else:
                print(delete_response.text)

    else:
        print(watermark_response.text)
else:
    print(upload_response.text)

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
    restrict_data = { "id" : uploaded_id, "new_permissions_password": "password", "restrictions": ["print_low", "print_high", "edit_content"] }

    print(json.dumps(restrict_data, indent = 2))


    print("Processing file...")
    restrict_response = requests.post(url='https://api.pdfrest.com/restricted-pdf',
                        data=json.dumps(restrict_data),
                        headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})



    print("Processing response status code: " + str(restrict_response.status_code))
    if restrict_response.ok:
        restrict_response_json = restrict_response.json()
        print(json.dumps(restrict_response_json, indent = 2))

        # All files uploaded or generated are automatically deleted based on the 
        # File Retention Period as shown on https://pdfrest.com/pricing. 
        # For immediate deletion of files, particularly when sensitive data 
        # is involved, an explicit delete call can be made to the API.
        #
        # Deletes all files in the workflow, including outputs. Save all desired files before enabling this step.

        if DELETE_SENSITIVE_FILES:
            restricted_output_id = restrict_response_json['outputId']
            delete_data = { "ids": f"{uploaded_id}, {restricted_output_id}" }
            delete_response = requests.post(url='https://api.pdfrest.com/delete',
                                data=json.dumps(delete_data),
                                headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})
            if delete_response.ok:
                print(json.dumps(delete_response.json(), indent = 2))
            else:
                print(delete_response.text)

    else:
        print(restrict_response.text)
else:
    print(upload_response.text)

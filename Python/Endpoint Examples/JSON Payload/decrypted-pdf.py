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
    decrypt_data = { "id" : uploaded_id, 'current_open_password': 'password' }
    print(json.dumps(decrypt_data, indent = 2))


    print("Processing file...")
    decrypt_response = requests.post(url='https://api.pdfrest.com/decrypted-pdf',
                        data=json.dumps(decrypt_data),
                        headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})



    print("Processing response status code: " + str(decrypt_response.status_code))
    if decrypt_response.ok:
        decrypt_response_json = decrypt_response.json()
        print(json.dumps(decrypt_response_json, indent = 2))

        # All files uploaded or generated are automatically deleted based on the 
        # File Retention Period as shown on https://pdfrest.com/pricing. 
        # For immediate deletion of files, particularly when sensitive data 
        # is involved, an explicit delete call can be made to the API.
        #
        # The following code is an optional step to delete sensitive files
        # (unredacted, unencrypted, unrestricted, or unwatermarked) from pdfRest servers.

        result_id = decrypt_response_json['outputId']
        delete_data = { "ids": result_id }
        delete_response = requests.post(url='https://api.pdfrest.com/delete',
                            data=json.dumps(delete_data),
                            headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})
        print(json.dumps(delete_response.json(), indent = 2))

    else:
        print(decrypt_response.text)
else:
    print(upload_response.text)

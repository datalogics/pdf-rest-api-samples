import requests
import json

# By default, we use the US-based API service. This is the primary endpoint for global use.
api_url = "https://api.pdfrest.com"

# For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#api_url = "https://eu-api.pdfrest.com"

# Toggle deletion of sensitive files (default: False)
DELETE_SENSITIVE_FILES = False

with open('/path/to/file', 'rb') as f:
    upload_data = f.read()

print("Uploading file...")
upload_response = requests.post(url=api_url+'/upload',
                    data=upload_data,
                    headers={'Content-Type': 'application/octet-stream', 'Content-Filename': 'file.pdf', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})

print("Upload response status code: " + str(upload_response.status_code))

if upload_response.ok:
    upload_response_json = upload_response.json()
    print(json.dumps(upload_response_json, indent = 2))


    uploaded_id = upload_response_json['files'][0]['id']
    unrestrict_data = { "id" : uploaded_id, "current_permissions_password": "password" }

    print(json.dumps(unrestrict_data, indent = 2))


    print("Processing file...")
    unrestrict_response = requests.post(url=api_url+'/unrestricted-pdf',
                        data=json.dumps(unrestrict_data),
                        headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})



    print("Processing response status code: " + str(unrestrict_response.status_code))
    if unrestrict_response.ok:
        unrestrict_response_json = unrestrict_response.json()
        print(json.dumps(unrestrict_response_json, indent = 2))

        # All files uploaded or generated are automatically deleted based on the
        # File Retention Period as shown on https://pdfrest.com/pricing.
        # For immediate deletion of files, particularly when sensitive data
        # is involved, an explicit delete call can be made to the API.
        #
        # Deletes all files in the workflow, including outputs. Save all desired files before enabling this step.

        if DELETE_SENSITIVE_FILES:
            result_id = unrestrict_response_json['outputId']
            delete_data = { "ids": f"{uploaded_id}, {result_id}" }
            delete_response = requests.post(url=api_url+'/delete',
                                data=json.dumps(delete_data),
                                headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})
            print(json.dumps(delete_response.json(), indent = 2))

    else:
        print(unrestrict_response.text)
else:
    print(upload_response.text)

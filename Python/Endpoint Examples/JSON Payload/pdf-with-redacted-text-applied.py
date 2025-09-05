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
    pdf_with_redacted_text_data = { "id" : uploaded_id }
    print(json.dumps(pdf_with_redacted_text_data, indent = 2))


    print("Processing file...")
    pdf_with_redacted_text_response = requests.post(url='https://api.pdfrest.com/pdf-with-redacted-text-applied',
                        data=json.dumps(pdf_with_redacted_text_data),
                        headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})



    print("Processing response status code: " + str(pdf_with_redacted_text_response.status_code))
    if pdf_with_redacted_text_response.ok:
        pdf_with_redacted_text_response_json = pdf_with_redacted_text_response.json()
        print(json.dumps(pdf_with_redacted_text_response_json, indent = 2))

        # All files uploaded or generated are automatically deleted based on the 
        # File Retention Period as shown on https://pdfrest.com/pricing. 
        # For immediate deletion of files, particularly when sensitive data 
        # is involved, an explicit delete call can be made to the API.
        #
        # The following code is an optional step to delete sensitive files
        # (unredacted, unencrypted, unrestricted, or unwatermarked) from pdfRest servers.

        if DELETE_SENSITIVE_FILES:
            delete_data = { "ids": uploaded_id }
            delete_response = requests.post(url='https://api.pdfrest.com/delete',
                                data=json.dumps(delete_data),
                                headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})
            if delete_response.ok:
                print(json.dumps(delete_response.json(), indent = 2))
            else:
                print(delete_response.text)

    else:
        print(pdf_with_redacted_text_response.text)
else:
    print(upload_response.text)

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
    redact_text_data = { "id" : uploaded_id, "redactions": json.dumps(redaction_options) }

    print(json.dumps(redact_text_data, indent = 2))


    print("Processing file...")
    redact_text_response = requests.post(url='https://api.pdfrest.com/pdf-with-redacted-text-preview',
                        data=json.dumps(redact_text_data),
                        headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})



    print("Processing response status code: " + str(redact_text_response.status_code))
    if redact_text_response.ok:
        redact_text_response_json = redact_text_response.json()
        print(json.dumps(redact_text_response_json, indent = 2))

        # All files uploaded or generated are automatically deleted based on the 
        # File Retention Period as shown on https://pdfrest.com/pricing. 
        # For immediate deletion of files, particularly when sensitive data 
        # is involved, an explicit delete call can be made to the API.
        #
# Deletes all files in the workflow, including outputs. Save all desired files before enabling this step.
        # IMPORTANT: Do not delete the preview_id (the preview PDF) file until after the redaction is applied
        # with the /pdf-with-redacted-text-applied endpoint.

        preview_id = redact_text_response_json['outputId']
        if DELETE_SENSITIVE_FILES:
            delete_data = { "ids": uploaded_id + ", " + preview_id }
            delete_response = requests.post(url='https://api.pdfrest.com/delete',
                                data=json.dumps(delete_data),
                                headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})
            if delete_response.ok:
                print(json.dumps(delete_response.json(), indent = 2))
            else:
                print(delete_response.text)

    else:
        print(redact_text_response.text)
else:
    print(upload_response.text)

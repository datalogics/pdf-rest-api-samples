import requests
import json

# By default, we use the US-based API service. This is the primary endpoint for global use.
api_url = "https://api.pdfrest.com"

# For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#api_url = "https://eu-api.pdfrest.com"

with open('/path/to/file', 'rb') as f:
    upload_data = f.read()

print("Uploading file...")
upload_response = requests.post(
    url=api_url+'/upload',
    data=upload_data,
    headers={
        'Content-Type': 'application/octet-stream',
        'Content-Filename': 'file.pdf',
        'API-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'
    }
)

print("Upload response status code: " + str(upload_response.status_code))

if upload_response.ok:
    upload_response_json = upload_response.json()
    print(json.dumps(upload_response_json, indent=2))

    uploaded_id = upload_response_json['files'][0]['id']
    translate_data = {
        "id": uploaded_id,
        # Translates text to American English. Format the output_language as a 2-3 character ISO 639 code, optionally with a region/script (e.g., 'en', 'es', 'zh-Hant', 'eng-US').
        "output_language": "en-US",
    }
    print(json.dumps(translate_data, indent=2))

    print("Processing file...")
    translate_response = requests.post(
        url=api_url+'/translated-pdf-text',
        data=json.dumps(translate_data),
        headers={
            'Content-Type': 'application/json',
            'API-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'
        }
    )

    print("Processing response status code: " + str(translate_response.status_code))
    if translate_response.ok:
        translate_response_json = translate_response.json()
        print(json.dumps(translate_response_json, indent=2))
    else:
        print(translate_response.text)
else:
    print(upload_response.text)


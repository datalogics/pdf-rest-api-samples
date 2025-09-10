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
upload_response = requests.post(url=api_url+'/upload',
                    data=upload_data,
                    headers={'Content-Type': 'application/octet-stream', 'Content-Filename': 'file.pdf', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})

print("Upload response status code: " + str(upload_response.status_code))

if upload_response.ok:
    upload_response_json = upload_response.json()
    print(json.dumps(upload_response_json, indent = 2))


    uploaded_id = upload_response_json['files'][0]['id']
    gif_data = { "id" : uploaded_id }
    print(json.dumps(gif_data, indent = 2))


    print("Processing file...")
    gif_response = requests.post(url=api_url+'/gif',
                        data=json.dumps(gif_data),
                        headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})



    print("Processing response status code: " + str(gif_response.status_code))
    if gif_response.ok:
        gif_response_json = gif_response.json()
        print(json.dumps(gif_response_json, indent = 2))

    else:
        print(gif_response.text)
else:
    print(upload_response.text)

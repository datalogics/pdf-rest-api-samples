import requests
import json

# By default, we use the US-based API service. This is the primary endpoint for global use.
api_url = "https://api.pdfrest.com"

# For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#api_url = "https://eu-api.pdfrest.com"

delete_data = { "ids" : "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx, xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" }


print("Processing files...")
delete_response = requests.post(url=api_url+'/delete',
                data=json.dumps(delete_data),
                headers={'Content-Type': 'application/json', "API-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"})



print("Processing response status code: " + str(delete_response.status_code))
if delete_response.ok:
    delete_response_json = delete_response.json()
    print(json.dumps(delete_response_json, indent = 2))

else:
    print(delete_response.text)

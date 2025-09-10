import requests
import json

# By default, we use the US-based API service. This is the primary endpoint for global use.
up_url = f"https://api.pdfrest.com/up-toolkit"

# For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#up_url = f"https://eu-api.pdfrest.com/up-toolkit"

# up-forms and up-office can be used to query the other tools
up_url = f"https://api.pdfrest.com/up-toolkit"

print("Sending GET request to /up-toolkit endpoint...")
response = requests.get(up_url)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

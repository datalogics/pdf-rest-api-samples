import requests

# By default, we use the US-based API service. This is the primary endpoint for global use.
api_url = "https://api.pdfrest.com"

# For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#api_url = "https://eu-api.pdfrest.com"

url = f"{api_url}/resource/xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"

payload = {}
headers = {
  'api-key': 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'
}

response = requests.request("DELETE", url, headers=headers, data=payload)

print(response.text)

import requests

url = "https://api.pdfrest.com/resource/xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"

payload = {}
headers = {
  'api-key': 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'
}

response = requests.request("DELETE", url, headers=headers, data=payload)

print(response.text)

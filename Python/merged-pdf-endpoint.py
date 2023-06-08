import requests

# Set the API endpoint URL
url = "https://api.pdfrest.com/merged-pdf"

# Define the payload data for the request
payload = {
    'pages[]': '1-last',
    'type[]': 'file',
    'pages[]': '1-last',
    'type[]': 'file',
    'output': 'pdfrest_merged_pdf'
}

# Specify the files to be merged
files = [
    ('file', ('file', open('/path/to/file', 'rb'), 'application/octet-stream')),
    ('file', ('file', open('/path/to/file', 'rb'), 'application/octet-stream'))
]

# Set the headers, including the API key
headers = {
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'  # Replace with your API key
}

# Make a POST request to the API endpoint
response = requests.request(
    "POST", url, headers=headers, data=payload, files=files)

# Print the response content
print(response.text)

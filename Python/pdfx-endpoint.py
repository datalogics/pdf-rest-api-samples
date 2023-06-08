import requests

# Set the API endpoint URL
url = "https://api.pdfrest.com/pdfx"

# Define the payload data for the request
payload = {
    'output_type': 'PDF/X-4',
    'output': 'pdfrest_pdfx'
}

# Specify the PDF file to convert to PDF/X format
files = [
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

import requests

# Set the API endpoint URL
url = "https://api.pdfrest.com/flattened-transparencies-pdf"

# Define the payload data for the request
payload = {
    'quality': 'medium',
    'output': 'pdfrest_flattened_transparencies_pdf'
}

# Specify the PDF file to have transparencies flattened
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

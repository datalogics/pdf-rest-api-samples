import requests

# Set the API endpoint URL
url = "https://api.pdfrest.com/pdf-with-added-image"

# Define the payload data for the request
payload = {
    'x': '0',
    'y': '0',
    'page': '1',
    'output': 'pdfrest_pdf_with_added_image'
}

# Specify the PDF file and image file to be combined
files = [
    ('file', ('file', open('/path/to/file', 'rb'), 'application/octet-stream')),
    ('image_file', ('file', open('/path/to/file', 'rb'), 'application/octet-stream'))
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

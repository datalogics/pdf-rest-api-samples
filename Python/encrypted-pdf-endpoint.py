import requests

# Set the API endpoint URL
url = "https://api.pdfrest.com/encrypted-pdf"

# Define the payload data for the request
payload = {
    'new_open_password': 'new_example_pw',
    'output': 'pdfrest_encrypted_pdf'
}

# Specify the file to be encrypted
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

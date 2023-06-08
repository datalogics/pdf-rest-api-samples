import requests

# Set the API endpoint URL
url = "https://api.pdfrest.com/restricted-pdf"

# Define the payload data for the request
payload = {
    'new_permissions_password': 'new_example_pw',
    'restrictions[]': 'print_low',
    'restrictions[]': 'accessibility_off',
    'restrictions[]': 'edit_content',
    'output': 'pdfrest_restricted_pdf'
}

# Specify the file to apply restrictions
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

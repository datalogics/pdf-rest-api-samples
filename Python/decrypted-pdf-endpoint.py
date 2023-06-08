import requests

# Set the API endpoint URL
url = "https://api.pdfrest.com/decrypted-pdf"

# Define the payload data for the request
payload = {
    'current_open_password': 'current_example_pw',
    'output': 'pdfrest_decrypted_pdf'
}

# Specify the PDF file to be decrypted
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

import requests

# Set the API endpoint URL
url = "https://api.pdfrest.com/split-pdf"

# Define the payload data for the request
payload = {
    'pages[]': 'even',
    'pages[]': 'odd',
    'pages[]': '1,3,4-6',
    'output': 'pdfrest_split_pdf'
}

# Specify the file to be split
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

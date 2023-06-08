import requests

# Set the API endpoint URL
url = "https://api.pdfrest.com/jpg"

# Define the payload data for the request
payload = {
    'pages': '1-last',
    'resolution': '300',
    'color_model': 'rgb',
    'jpeg_quality': '75',
    'output': 'pdfrest_jpg'
}

# Specify the file to be converted
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

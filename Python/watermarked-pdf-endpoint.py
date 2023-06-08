import requests

# Set the API endpoint URL
url = "https://api.pdfrest.com/watermarked-pdf"

# Define the payload data for the request
payload = {
    'watermark_text': 'Hello, watermarked world!',
    'font': 'Arial',
    'text_size': '72',
    'text_color_rgb': '255,0,0',
    'opacity': '0.5',
    'x': '0',
    'y': '0',
    'rotation': '0',
    'output': 'pdfrest_watermarked_pdf'
}

# Specify the file to be watermarked
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

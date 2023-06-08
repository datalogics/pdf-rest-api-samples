import requests

# Set the API endpoint URL
url = "https://api.pdfrest.com/pdf-info"

# Define the payload data for the request
payload = {
    'queries': 'tagged,image_only,title,subject,author,producer,creator,creation_date,modified_date,keywords,doc_language,page_count,contains_annotations,contains_signature,pdf_version,file_size,filename,restrict_permissions_set,contains_xfa,contains_acroforms,contains_javascript,contains_transparency,contains_embedded_file,uses_embedded_fonts,uses_nonembedded_fonts,pdfa,requires_password_to_open'
}

# Specify the PDF file to get information about
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

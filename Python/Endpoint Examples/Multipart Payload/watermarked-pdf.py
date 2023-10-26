from requests_toolbelt import MultipartEncoder
import requests
import json

watermarked_pdf_endpoint_url = 'https://api.pdfrest.com/watermarked-pdf'

mp_encoder_watermarkedPDF = MultipartEncoder(
    fields={
        'file': ('file_name', open('/path/to/file', 'rb'), 'application/pdf'),
        'watermark_text': 'watermark',
        'output' : 'example_out'
    }
)

headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_watermarkedPDF.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to watermarked-pdf endpoint...")
response = requests.post(watermarked_pdf_endpoint_url, data=mp_encoder_watermarkedPDF, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
else:
    print(response.text)

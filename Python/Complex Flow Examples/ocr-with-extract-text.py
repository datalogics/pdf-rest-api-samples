from requests_toolbelt import MultipartEncoder
import requests


# In this sample, we will show how to convert a scanned document into a PDF with
# searchable and extractable text using Optical Character Recognition (OCR), and then
# extract that text from the newly created document.
#
# First, we will upload a scanned PDF to the /pdf-with-ocr-text route and capture the
# output ID. Then, we will send the output ID to the /extracted-text route, which will
# return the newly added text.

api_key = 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here

ocr_endpoint_url = 'https://api.pdfrest.com/pdf-with-ocr-text'
mp_encoder_pdf = MultipartEncoder(
    fields={
        'file': ('file_name.pdf', open('/path/to/file.pdf', 'rb'), 'application/pdf'),
        'output': 'example_pdf-with-ocr-text_out',
    }
)

image_headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_pdf.content_type,
    'Api-Key': api_key
}

print("Sending POST request to OCR endpoint...")
response = requests.post(ocr_endpoint_url, data=mp_encoder_pdf, headers=image_headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    ocr_pdf_id = response_json["outputId"]
    print("Got the output ID: " + ocr_pdf_id)

    extract_endpoint_url = 'https://api.pdfrest.com/extracted-text'

    mp_encoder_extract_text = MultipartEncoder(
        fields={
            'id': ocr_pdf_id
        }
    )

    extract_text_headers = {
        'Accept': 'application/json',
        'Content-Type': mp_encoder_extract_text.content_type,
        'Api-Key': api_key
    }

    print("Sending POST request to extract text endpoint...")
    extract_response = requests.post(extract_endpoint_url, data=mp_encoder_extract_text, headers=extract_text_headers)

    print("Response status code: " + str(extract_response.status_code))

    if extract_response.ok:
        extract_json = extract_response.json()
        print(extract_json["fullText"])

    else:
        print(extract_response.text)


else:
    print(response.text)
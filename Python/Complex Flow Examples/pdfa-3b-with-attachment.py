from requests_toolbelt import MultipartEncoder
import requests
import json

# In this sample, we will show how to attach an xml document to a PDF file and then
# convert the file with the attachment to conform to the PDF/A standard, which
# can be useful for invoicing and standards compliance. We will be running the
# input document through /pdf-with-added-attachment to add the attachment and
# then /pdfa to do the PDF/A conversion.

# Note that there is nothing special about attaching an xml file, and any appropriate
# file may be attached and wrapped into the PDF/A conversion.

api_key = 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here

pdf_with_added_attachment_endpoint_url = 'https://api.pdfrest.com/pdf-with-added-attachment'

mp_encoder_pdfWithAddedAttachment = MultipartEncoder(
    fields={
        'file': ('file_name.pdf', open('/path/to/file.pdf', 'rb'), 'application/pdf'),
        'file_to_attach': ('file_name.xml', open('/path/to/file.xml', 'rb'), 'application/xml'),
        'output' : 'example_out',
    }
)

attachment_headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_pdfWithAddedAttachment.content_type,
    'Api-Key': api_key
}

print("Sending POST request to pdf-with-added-attachment endpoint...")
response = requests.post(pdf_with_added_attachment_endpoint_url, data=mp_encoder_pdfWithAddedAttachment, headers=attachment_headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    pdfWithAddedAttachment_id = response_json["outputId"]

    pdfa_endpoint_url = 'https://api.pdfrest.com/pdfa'

    mp_encoder_pdfa = MultipartEncoder(
        fields={
            'id': pdfWithAddedAttachment_id,
            'output_type': 'PDF/A-3b',
            'rasterize_if_errors_encountered': 'on',
            'output' : '3b_with_attachment',
        }
    )

    headers = {
        'Accept': 'application/json',
        'Content-Type': mp_encoder_pdfa.content_type,
        'Api-Key': api_key
    }

    print("Sending POST request to pdfa endpoint...")
    response = requests.post(pdfa_endpoint_url, data=mp_encoder_pdfa, headers=headers)

    print("Response status code: " + str(response.status_code))

    if response.ok:
        response_json = response.json()
        print(json.dumps(response_json, indent = 2))
    else:
        print(response.text)
else:
    print(response.text)

# If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.py' sample.

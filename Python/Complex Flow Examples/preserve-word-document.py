from requests_toolbelt import MultipartEncoder
import requests
import json

# In this sample, we will show how to optimize a Word file for long-term preservation
# as discussed in https://pdfrest.com/solutions/optimize-word-excel-and-powerpoint-files-for-long-term-preservation/
# We will take our Word (or Excel or PowerPoint) document and first convert it to
# a PDF with a call to the /pdf route. Then, we will take that converted PDF
# and convert it to the PDF/A format for long-term storage.


pdf_endpoint_url = 'https://api.pdfrest.com/pdf'

mp_encoder_pdf = MultipartEncoder(
    fields={
        'file': ('file_name.docx', open('/path/to/file.doc', 'rb'), 'application/msword'),
    }
)

pdf_headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_pdf.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to pdf endpoint...")
response = requests.post(pdf_endpoint_url, data=mp_encoder_pdf, headers=pdf_headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    pdf_id = response_json["outputId"]


    pdfa_endpoint_url = 'https://api.pdfrest.com/pdfa'

    mp_encoder_pdfa = MultipartEncoder(
        fields={
            'id': pdf_id,
            'output_type': 'PDF/A-1b',
            'rasterize_if_errors_encountered': 'on',
            'output' : 'preserved',
        }
    )

    headers = {
        'Accept': 'application/json',
        'Content-Type': mp_encoder_pdfa.content_type,
        'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
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

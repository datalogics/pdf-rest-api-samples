from requests_toolbelt import MultipartEncoder
import requests
import json


# In this sample, we will show how to watermark a PDF document and then restrict
# editing on the document so that the watermark cannot be removed, as discussed in
# https://pdfrest.com/solutions/add-pdf-watermarks-that-cannot-be-removed/.
# We will be running the input file through /watermarked-pdf to apply the watermark
# and then /restricted-pdf to lock the watermark in.

watermarked_pdf_endpoint_url = 'https://api.pdfrest.com/watermarked-pdf'

mp_encoder_watermarkedPDF = MultipartEncoder(
    fields={
        'file': ('file_name.pdf', open('/path/to/file.pdf', 'rb'), 'application/pdf'),
        'watermark_text': 'watermark',
    }
)

watermark_headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_watermarkedPDF.content_type,
    'Api-Key': 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to watermarked-pdf endpoint...")
response = requests.post(watermarked_pdf_endpoint_url, data=mp_encoder_watermarkedPDF, headers=watermark_headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    watermarked_id = response_json["outputId"]


    restricted_pdf_endpoint_url = 'https://api.pdfrest.com/restricted-pdf'

    mp_encoder_restrictedPdf = MultipartEncoder(
        fields=[
            ('id', watermarked_id),
            ('output', 'secured_watermark'),
            ('new_permissions_password', 'password'),
            ('restrictions', 'edit_annotations'),
            ('restrictions', 'copy_content'),
            ('restrictions', 'edit_content')
        ]
    )

    restrict_headers = {
        'Accept': 'application/json',
        'Content-Type': mp_encoder_restrictedPdf.content_type,
        'Api-Key': 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
    }

    print("Sending POST request to restricted-pdf endpoint...")
    response = requests.post(restricted_pdf_endpoint_url, data=mp_encoder_restrictedPdf, headers=restrict_headers)

    print("Response status code: " + str(response.status_code))

    if response.ok:
        response_json = response.json()
        print(json.dumps(response_json, indent = 2))
    else:
        print(response.text)
else:
    print(response.text)

# If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.py' sample.

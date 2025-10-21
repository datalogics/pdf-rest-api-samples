from requests_toolbelt import MultipartEncoder
import requests
import json

# This sample demonstrates a workflow to prepare form and image-only documents
# for conversion to Markdown. This process uses the Query PDF tool to determine
# whether the document contains forms or contains only images. Form documents
# have their forms flattened, and image-only documents are processed through
# the OCR PDF tool.
#
# To get started, configure the following constants.

# By default, we use the US-based API service. This is the primary endpoint for global use.
API_URL = "https://api.pdfrest.com"

# For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#API_URL = "https://eu-api.pdfrest.com"

API_KEY = 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # Place your api key here.

OCR_LANGUAGES = "English" # a comma-separated list of languages for the OCR tool to use

INPUT_FILE_LOCATION = '/path/to/file/location/' # Replace this value with the home directory of your input PDF.

INPUT_FILE_NAME = 'myFile.pdf' # Replace this value with the filename of your input PDF.

DO_OCR = True # If True, this sample will perform OCR on PDFs that are image-only.

DO_FORM_FLATTENING = True # If True, this sample will flatten forms in form documents.

pdf_endpoint_url = f"{API_URL}/pdf-info"

mp_encoder_query = MultipartEncoder(
    fields={
        'file': (INPUT_FILE_NAME, open(f"{INPUT_FILE_LOCATION}{INPUT_FILE_NAME}", 'rb'), 'application/pdf'),
        'queries': "image_only,contains_xfa,contains_acroforms",
    }
)

headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_query.content_type,
    'Api-Key': API_KEY
}

print("Sending POST request to Query PDF endpoint...")
response = requests.post(pdf_endpoint_url, data=mp_encoder_query, headers=headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    query_response = response.json()
    if query_response["allQueriesProcessed"]:
        pdf_id = query_response["inputId"]
        pdf_contains_forms = query_response["contains_xfa"] or query_response["contains_acroforms"]
        image_only_pdf = query_response["image_only"]
        if pdf_contains_forms and DO_FORM_FLATTENING:
            mp_encoder_flatten = MultipartEncoder(
                fields={
                    'id': pdf_id,
                }
            )
            headers = {
                'Accept': 'application/json',
                'Content-Type': mp_encoder_flatten.content_type,
                'Api-Key': API_KEY
            }
            flatten_endpoint_url = f"{API_URL}/flattened-forms-pdf"
            print("Sending POST request to Flatten Forms endpoint...")
            response = requests.post(flatten_endpoint_url, data=mp_encoder_flatten, headers=headers)
            if response.ok:
                flatten_response = response.json()
                pdf_id = flatten_response["outputId"]
            else:
                print(response.text)
        elif image_only_pdf and DO_OCR:
            mp_encoder_ocr = MultipartEncoder(
                fields={
                    'id': pdf_id,
                    'languages': OCR_LANGUAGES,
                }
            )
            headers = {
                'Accept': 'application/json',
                'Content-Type': mp_encoder_ocr.content_type,
                'Api-Key': API_KEY
            }
            ocr_endpoint_url = f"{API_URL}/pdf-with-ocr-text"
            print("Sending POST request to OCR PDF endpoint...")
            response = requests.post(ocr_endpoint_url, data=mp_encoder_ocr, headers=headers)
            if response.ok:
                ocr_response = response.json()
                pdf_id = ocr_response["outputId"]
            else:
                print(response.text)

        mp_encoder_markdown = MultipartEncoder(
            fields={
                'id': pdf_id,
                'output_type': "file", # Set to 'file' to get a download link to the .MD file.
            }
        )

        headers = {
            'Accept': 'application/json',
            'Content-Type': mp_encoder_markdown.content_type,
            'Api-Key': API_KEY
        }

        print("Sending POST request to Markdown endpoint...")
        markdown_endpoint_url = f"{API_URL}/markdown"
        response = requests.post(markdown_endpoint_url, data=mp_encoder_markdown, headers=headers)

        print("Response status code: " + str(response.status_code))

        if response.ok:
            query_response = response.json()
            print(json.dumps(query_response, indent = 2))
        else:
            print(response.text)
    else:
        print(response.text)
else:
    print(response.text)

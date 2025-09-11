from requests_toolbelt import MultipartEncoder
import requests
import json

# This sample demonstrates the workflow from unredacted document to fully
# redacted document. The output file from the preview tool is immediately
# forwarded to the finalization stage. We recommend inspecting the output from
# the preview stage before utilizing this workflow to ensure that content is
# redacted as intended.

# By default, we use the US-based API service. This is the primary endpoint for global use.
api_url = "https://api.pdfrest.com"

# For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#api_url = "https://eu-api.pdfrest.com"

api_key = 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here

pdf_endpoint_url = api_url+'/pdf-with-redacted-text-preview'

redaction_options = [
    {
        "type": "regex",
        "value": "[Tt]he",
    }
]

mp_encoder_preview = MultipartEncoder(
    fields={
        'file': ('file.pdf', open('/path/to/file.pdf', 'rb'), 'application/pdf'),
        'redactions': json.dumps(redaction_options),
        'output' : 'example_out'
    }
)

pdf_headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_preview.content_type,
    'Api-Key': api_key
}

print("Sending POST request to redaction preview endpoint...")
response = requests.post(pdf_endpoint_url, data=mp_encoder_preview, headers=pdf_headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()
    pdf_id = response_json["outputId"]


    applied_endpoint_url = api_url+'/pdf-with-redacted-text-applied'

    mp_encoder_applied = MultipartEncoder(
        fields={
            'id': pdf_id,
            'output' : 'redacted_final',
        }
    )

    headers = {
        'Accept': 'application/json',
        'Content-Type': mp_encoder_applied.content_type,
        'Api-Key': api_key
    }

    print("Sending POST request to applied redaction endpoint...")
    response = requests.post(applied_endpoint_url, data=mp_encoder_applied, headers=headers)

    print("Response status code: " + str(response.status_code))

    if response.ok:
        response_json = response.json()
        print(json.dumps(response_json, indent = 2))
    else:
        print(response.text)
else:
    print(response.text)

# If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.py' sample.

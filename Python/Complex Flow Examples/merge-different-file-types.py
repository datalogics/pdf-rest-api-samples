from requests_toolbelt import MultipartEncoder
import requests
import json


# In this sample, we will show how to merge different file types together as
# discussed in https://pdfrest.com/solutions/merge-multiple-types-of-files-together/.
# First, we will upload an image file to the /pdf route and capture the output ID.
# Next, we will upload a PowerPoint file to the /pdf route and capture its output
# ID. Finally, we will pass both IDs to the /merged-pdf route to combine both inputs
# into a single PDF.
#
# Note that there is nothing special about an image and a PowerPoint file, and
# this sample could be easily used to convert and combine any two file types
# that the /pdf route takes as inputs.

# By default, we use the US-based API service. This is the primary endpoint for global use.
api_url = "https://api.pdfrest.com"

# For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#api_url = "https://eu-api.pdfrest.com"

api_key = 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here

pdf_endpoint_url = api_url+'/pdf'
mp_encoder_image_pdf = MultipartEncoder(
    fields={
        'file': ('file_name.png', open('/path/to/file.png', 'rb'), 'image/png'),
    }
)

image_headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_image_pdf.content_type,
    'Api-Key': api_key
}

print("Sending POST request to pdf endpoint...")
response = requests.post(pdf_endpoint_url, data=mp_encoder_image_pdf, headers=image_headers)

print("Response status code: " + str(response.status_code))

if response.ok:

    response_json = response.json()
    image_id = response_json["outputId"]
    print("Got the first output ID: " + image_id)


    mp_encoder_ppt_pdf = MultipartEncoder(
        fields={
            'file': ('file_name.ppt', open('/path/to/file.ppt', 'rb'), 'application/vnd.ms-powerpoint'),

        }
    )

    ppt_headers = {
        'Accept': 'application/json',
        'Content-Type': mp_encoder_ppt_pdf.content_type,
        'Api-Key': api_key
    }

    print("Sending POST request to pdf endpoint...")
    response = requests.post(pdf_endpoint_url, data=mp_encoder_ppt_pdf, headers=ppt_headers)

    print("Response status code: " + str(response.status_code))

    if response.ok:
        response_json = response.json()

        ppt_id = response_json["outputId"]
        print("Got the second output ID: " + image_id)

        merged_pdf_endpoint_url = api_url+'/merged-pdf'

        merge_request_data = [('id', image_id), ('pages', '1-last'), ('type', 'id'), ('id', ppt_id), ('pages', '1-last'), ('type', 'id'), ('output', 'multiple_file_types')]
        mp_encoder_merge = MultipartEncoder(
            fields=merge_request_data
        )

        merge_headers = {
            'Accept': 'application/json',
            'Content-Type': mp_encoder_merge.content_type,
            'Api-Key': api_key
        }

        print("Sending POST request to merged-pdf endpoint...")
        response = requests.post(merged_pdf_endpoint_url, data=mp_encoder_merge, headers=merge_headers)

        print("Merge response status code: " + str(response.status_code))

        if response.ok:
            response_json = response.json()
            print(json.dumps(response_json, indent = 2))
        else:
            print(response.text)

    else:
        print(response.text)


else:
    print(response.text)

# If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.py' sample.

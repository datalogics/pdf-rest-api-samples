from requests_toolbelt import MultipartEncoder
import requests
import json

# In this sample we will show how to take an encrypted file and decrypt, modify
# and re-encrypt it to create an encryption-at-rest solution as discussed in
# https://pdfrest.com/solutions/create-secure-document-workflows-with-pdf-password-protection/
# We will be running the document through /decrypted-pdf to open the document
# to modification, running the decrypted result through /pdf-with-added-image,
# and then sending the output with the new image through /encrypted-pdf to
# lock it up again.

decrypted_pdf_endpoint_url = 'https://api.pdfrest.com/decrypted-pdf'

mp_encoder_decryptedPdf = MultipartEncoder(
    fields={
        'file': ('file_name.pdf', open('/path/to/encrypted_file.pdf', 'rb'), 'application/pdf'),
        'current_open_password': 'password',
    }
)

decrypt_headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_decryptedPdf.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

print("Sending POST request to decrypted-pdf endpoint...")
response = requests.post(decrypted_pdf_endpoint_url, data=mp_encoder_decryptedPdf, headers=decrypt_headers)

print("Response status code: " + str(response.status_code))

if response.ok:
    response_json = response.json()

    decrypted_id = response_json["outputId"]

    pdf_with_added_image_endpoint_url = 'https://api.pdfrest.com/pdf-with-added-image'

    mp_encoder_pdfWithAddedImage = MultipartEncoder(
        fields={
            'id': decrypted_id,
            'image_file': ('file_name.jpg', open('/path/to/image_jpg.jpg', 'rb'), 'image/jpeg'),
            'x' : '10',
            'y' : '10',
            'page' : '1',
        }
    )

    add_image_headers = {
        'Accept': 'application/json',
        'Content-Type': mp_encoder_pdfWithAddedImage.content_type,
        'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
    }

    print("Sending POST request to pdf-with-added-image endpoint...")
    response = requests.post(pdf_with_added_image_endpoint_url, data=mp_encoder_pdfWithAddedImage, headers=add_image_headers)

    print("Response status code: " + str(response.status_code))

    if response.ok:
        response_json = response.json()
        added_image_id = response_json["outputId"]

        encrypted_pdf_endpoint_url = 'https://api.pdfrest.com/encrypted-pdf'

        mp_encoder_encryptedPdf = MultipartEncoder(
            fields={
                'id': added_image_id,
                'output': 'encrypted',
                'new_open_password': 'password',
            }
        )

        encrypt_headers = {
            'Accept': 'application/json',
            'Content-Type': mp_encoder_encryptedPdf.content_type,
            'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
        }

        print("Sending POST request to encrypted-pdf endpoint...")
        response = requests.post(encrypted_pdf_endpoint_url, data=mp_encoder_encryptedPdf, headers=encrypt_headers)

        print("Response status code: " + str(response.status_code))

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

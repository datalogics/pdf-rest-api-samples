import requests
import json

# Resource UUIDs can be found in the JSON response of POST requests as "outputId". Resource UUIDs usually look like this: '0950b9bdf-0465-4d3f-8ea3-d2894f1ae839'.
id = 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place resource uuid here

# The response format can be 'file' or 'url'. 
# If 'url', then JSON containing the url of the resource file is returned.
# If 'file', then the file itself is returned.
format = 'file'

resource_url = f"https://api.pdfrest.com/resource/{id}?format={format}"

print("Sending GET request to /resource/{id} endpoint...")
response = requests.get(resource_url)

print("Response status code: " + str(response.status_code))

if response.ok and format == 'url':
    response_json = response.json()
    print(json.dumps(response_json, indent = 2))
elif response.ok and format == 'file':
    # You will find a file (associated with the resource UUID above) in the same folder as the sample when the sample executes successfully.
    output_file_name = response.headers.get("Content-Disposition").split("filename=")[1]

    with open(output_file_name, 'wb') as f:
        f.write(response.content)

    print(f"The file {output_file_name} was created.")
else:
    print(response.text)

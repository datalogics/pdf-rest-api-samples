import openai
from requests_toolbelt import MultipartEncoder
import requests

openai.api_key = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" # place your OpenAI API Key here

pdf_info_endpoint_url = 'https://api.pdfrest.com/pdf-info'
query_prompt = 'From a standard JSON input, turn the following data into a one paragraph summary assuming it 
  describes a PDF Document. \n\n' # default prompt to get info from the /pdf-info output
file_name = 'some_file.pdf' # file name of the PDF to send to /pdf-info
 
# The /pdf-info endpoint can take a single PDF file or id as input.
mp_encoder_pdfInfo = MultipartEncoder(
    fields={
        'file': (
            file_name, 
            open('/PATH/TO/FILE/'+file_name, 'rb'), 
            'application/pdf'
        ),
        'queries': 'tagged,image_only,title,subject,author,producer,creator,creation_date,modified_date,
        keywords,doc_language,page_count,contains_annotations,contains_signature,pdf_version,file_size,
        filename',
    }
)
 
# Let's set the headers that the pdf-info endpoint expects.
# Since MultipartEncoder is used, the 'Content-Type' header gets set to 'multipart/form-data' via the 
# content_type attribute below.
headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_pdfInfo.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' # place your api key here
}

response = requests.post(pdf_info_endpoint_url, data=mp_encoder_pdfInfo, headers=headers)

if response.ok:
    response_json = response.json()

    # We can remove the unnecessary fields returned by /pdf-info, such as inputId and 
    # allQueriesProcessed since they don't describe information about the document, just the request
    del response_json['allQueriesProcessed'] 
    del response_json['inputId']
    
    # Append the query_prompt to the beginning of our JSON output returned from /pdf-info
    query_string = query_prompt + str(response_json)

    # Send the query off to ChatGPT using the gpt-3.5-turbo model
    chat_completion = openai.ChatCompletion.create(model="gpt-3.5-turbo", 
      messages=[{"role": "user", "content": query_string}])

    # Print the output to console on returning successfully
    print(chat_completion.choices[0].message.content)
    
else:
    print(response.text)

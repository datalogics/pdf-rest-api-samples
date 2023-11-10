import openai
from requests_toolbelt import MultipartEncoder
import requests
import tiktoken
from tenacity import (
    retry,
    stop_after_attempt,
    wait_random_exponential,
)  # for exponential backoff

# Configurations


openai.api_key = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"  # place your OpenAI API Key here
extract_text_endpoint_url = 'https://api.pdfrest.com/extracted-text'
query_prompt = ('Assuming the following text is from a PDF Document, derive a relatively short summary of the '
                'contents. \n\n')
file_name = 'FileName.pdf'  # file name of the PDF to send to /extracted-text
file_path = '/PATH/TO/FILE/'
MAX_CONTEXT_SIZE = 127000

# This is used for determining the length of a given string in tokens, so that we don't try sending
# too much data to ChatGPT
enc = tiktoken.get_encoding("cl100k_base")


# Helper function that allows larger documents to process without hitting the rate limits of ChatGPT
@retry(wait=wait_random_exponential(min=1, max=60), stop=stop_after_attempt(6))
def completion_with_backoff(**kwargs):
    return openai.ChatCompletion.create(**kwargs)


# The /extracted-text endpoint can take a single PDF file or id as input.
mp_encoder_extractText = MultipartEncoder(
    fields={
        'file': (
            file_name,
            open(file_path + file_name, 'rb'),
            'application/pdf'
        )
    }
)

# Let's set the headers that the /extracted-text endpoint expects. Since MultipartEncoder is used, the 'Content-Type'
# header gets set to 'multipart/form-data' via the content_type attribute below.
headers = {
    'Accept': 'application/json',
    'Content-Type': mp_encoder_extractText.content_type,
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'  # place your api key here
}

# Send the POST request to the /extracted-text endpoint
response = requests.post(extract_text_endpoint_url, data=mp_encoder_extractText, headers=headers)

if response.ok:
    print("Building prompt...")
    response_json = response.json()

    # To get the full text of the document, we grab the fullText attribute out of the resulting JSON
    fullText = response_json["fullText"]

    # In order to keep track of where we are in the document, we're going to split the resulting string into a list,
    # delimited by spaces.
    fullTextArray = fullText.split()

    # Append the query_prompt to the beginning of our JSON output returned from /extracted-text
    query_string = query_prompt

    # This logic sets up a loop that will continue until all the contents of the document have been processed,
    # keeping track of any summaries returned by ChatGPT.
    shouldLoop = True
    summaryList = []
    i = 0
    while shouldLoop:
        shouldLoop = False
        while len(enc.encode(query_string)) < MAX_CONTEXT_SIZE and i < len(fullTextArray):
            query_string += fullTextArray[i] + " "
            i += 1
            shouldLoop = True

        # For visual feedback, just printing out how much of the document has been processed by each request being sent
        print(f"Got to element #{i} out of {len(fullTextArray)}. \n")

        # Send the query off to ChatGPT using the gpt-4-1106-preview (also known as the GPT 4 turbo) model
        chat_completion = completion_with_backoff(model="gpt-4-1106-preview",
                                                  messages=[{"role": "user", "content": query_string}])

        # Reset query_string back to the default value of query_prompt
        query_string = query_prompt

        # Add the newly returned summary to the summaryList
        summaryList.append(chat_completion.choices[0].message.content)

        # If either of these conditions happen, we should break from the loop
        if len(enc.encode(query_string)) > MAX_CONTEXT_SIZE or i >= len(fullTextArray):
            break

    # If it took multiple summaries to process the entire document, compile the summaries and summarize them again
    # into a more cohesive singular summary.
    if len(summaryList) > 1:
        summary_string = ""
        for summary in summaryList:
            summary_string += " " + summary

        summary_query = ("Assuming the following text is a compilation of summaries about the contents of a single PDF "
                         "document, create a detailed comprehensive summary of the given text. \n\n") + summary_string
        final_chat_completion = completion_with_backoff(model="gpt-4-1106-preview",
                                                        messages=[{"role": "user", "content": summary_query}])

        print("\n" + final_chat_completion.choices[0].message.content + "\n")
    elif len(summaryList) == 1:
        print("\n" + summaryList[0] + "\n")

else:
    print(response.text)

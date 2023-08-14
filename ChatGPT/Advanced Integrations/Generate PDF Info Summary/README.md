<img src='https://cms.pdfrest.com/content/images/2023/07/Solution-Integrate-pdfRest-with-ChatGPT-to-Generate-PDF-Info-Summary.png' alt='Integration with ChatGPT'>
<br/>

# Integrate pdfRest with ChatGPT to Generate PDF Info Summary
### Learn how to leverage pdfRest Query PDF with OpenAI's ChatGPT LLM to easily create human-readable summaries of PDF metadata and document conditions
<br/>

Along with most developers and technology enthusiasts, we've tracked the progress of AI as it has taken leaps forward over the last few years. The technology has reached an exciting and critical point where it can now provide significant functional value to organizations. While services like OpenAI's [ChatGPT](https://platform.openai.com/overview) are now available to solve complex problems using the power of Large Language Models (LLMs), this only covers one side of any major problem-solving equation, as these services require input to be provided in the form of text prompts. This leads to a fundamental question, "Where does the input text come from?"

At pdfRest, we're excited to dive into this question and share some creative ideas for pairing a document processing toolkit, like pdfRest, with an LLM AI service, like ChatGPT, to unlock data from digital documents and create powerful synergy between these services. Join us on an inspirational journey below as we take you step-by-step through a functional solution to integrate the pdfRest [Query PDF](https://pdfrest.com/apitools/query-pdf/) API Tool with OpenAI's ChatGPT API to generate simple, human-readable summaries of complex PDF document metadata and conditional information!
<br/><br/>

## Testing ChatGPT
Before jumping straight into development, we manually passed some input along to ChatGPT's web interface. We weren't initially sure about its ability to read and parse meaning from the JSON format returned by our Query PDF tool. After sending a quick test document to the Query PDF `/pdf-info` endpoint, we passed the JSON output over to ChatGPT. The first test looked something like this:

    From a standard JSON input, turn the following data into a short summary assuming it describes a PDF 
    Document.

    {
        "tagged": false,
        "image_only": false,
        "title": "Character Sheet",
        "subject": "What is a subject anyway",
        "author": "One of us",
        "producer": "Adobe PDF Library 8.0",
        "creator": "Adobe InDesign CS3 (5.0.4)",
        "creation_date": "2011-06-03T18:22:56",
        "modified_date": "2022-12-01T15:30:27",
        "keywords": "fantasy, character, people",
        "doc_language": "None",
        "page_count": 2,
        "contains_annotations": true,
        "contains_signature": false,
        "pdf_version": "1.6.0",
        "file_size": 684444,
        "filename": "color_sheet.pdf",
    }
    
<br/>

ChatGPT responded with the following:
<br/>

    The JSON input provides details about a PDF document titled "Character Sheet" with 2 pages. The document is 
    not tagged, does not contain only images, and has a file size of 684444 bytes. It was authored by "One of us" 
    and created using "Adobe InDesign CS3 (5.0.4)" on June 3, 2011, at 18:22:56. The most recent modification was 
    on December 1, 2022, at 15:30:27. The document is not limited to a specific language, but its subject is 
    "What is a subject anyway." It is related to keywords such as "fantasy," "character," and "people."
    
    Additionally, the PDF contains annotations and does not have any digital signatures. The PDF version used is 
    "1.6.0." The filename associated with the PDF is "color_sheet.pdf," and it was produced using "Adobe PDF 
    Library 8.0."

<br/>

To our great excitement, it worked! You may notice that along with the JSON output from Query PDF, we also included a prompt to tell ChatGPT what to expect from the input and what to deliver in its output. This "base" prompt looked like this:

    From a standard JSON input, turn the following data into a short summary assuming it describes a PDF Document.

ChatGPT has been trained on so much information that it can recognize structure and infer meaning, even from data formats such as JSON. Because of this, it has no problem synthesizing a coherent, helpful summary of the detailed PDF metadata and conditional info. The value of this capability is clear - it is now possible to automatically abstract away the complexities of parsing JSON output for meaning and construct a summarized paragraph that a general audience can understand.
<br/><br/>

## Proof of Concept
With promising results from our manual testing, we moved on to develop a programmatic proof of concept using both the pdfRest [Cloud API](https://pdfrest.com/products/cloud-api/) and the OpenAI API. pdfRest is compatible with all major programming languages, and [OpenAI's documentation](https://platform.openai.com/docs/libraries/python-library) provides packages for both Python and JavaScript, so for this example we decided to work with Python.

To get started, we signed up for an API Key with each service:
- [pdfRest Get Started](https://pdfrest.com/getstarted/)
- [OpenAI Sign Up](https://platform.openai.com/signup?launch)
  
Then we installed the ChatGPT Python package, ensuring it worked as expected. Next, we found pdfRest code samples for the `/pdf-info` endpoint from the official [pdfRest API Samples GitHub Repository](https://github.com/datalogics/pdf-rest-api-samples). With very little modification, we ended up with the following code:
<br/>


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
<br/>

## Breaking Down the Code
If you take a quick glance at the pdfRest Python sample code for the [pdf-info endpoint](https://github.com/datalogics/pdf-rest-api-samples/blob/main/Python/pdf-info-endpoint.py), you'll see that we've preserved the basic structure of the sample code with only minor changes. As this code is self-documented with clear comments, we won't dive into all of the implementation details here.

Taking a look at the ChatGPT component, you'll notice the "base" prompt that we referenced earlier. Through trial and error, we found the input that gave us the most consistent formatting results is as follows:

    From a standard JSON input, turn the following data into a one paragraph summary assuming it describes a PDF 
    Document.

There are several important words and phrases in this prompt. First, we specify that the input data the AI can expect is `formatted according to standard JSON specifications`. Next, we specify to output `a one paragraph summary`. Without this, we found that ChatGPT produces inconsistent formatting, sometimes presented as a paragraph and other times a bullet point list. Finally, we specify that ChatGPT can assume the data `describes a PDF Document`. We have to remember that the only context ChatGPT has is what we provide, and since it doesn't have the actual PDF document, we need to let it know that's what this JSON is referencing.

Another notable section of code appears here:
<br/>


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

<br/>

As noted in the code's comments, we remove two fields returned by the Query PDF tool that are unnecessary for file summarization: `allQueriesProcessed` and `inputId`. Next, we simply append the previously covered "base" prompt, stored in the `query_prompt` variable, to the `response_json`, that's been converted to a string.

All that's left is to send our prepared text to OpenAI's API, specifying that we want to use the `gpt-3.5-turbo` model (since it's quicker, cheaper, and specializes in dialogue, which is perfect for our application). After a moment, we receive the output back from ChatGPT, and we access the string output by navigating down the returned object to the `message.content`. For our purpose, that's all we care about. Finally, we print the result to the command line. That's it - in about 50 lines of code, our end-to-end proof of concept is complete!
<br/><br/>

## What's Next?
ChatGPT has plenty of tricks up its sleeve, and we are not limited to generating paragraph summaries of PDF details. We can ask ChatGPT to do all sorts of things with our Query PDF results. As a quick example, we asked it instead to generate a limerick based on the same JSON. This is what ChatGPT gave us:
<br/>

    There once was a PDF so fine,
    Called "Character Sheet", it did shine.
    Created by InDesign,
    With keywords divine,
    Two pages, annotations it did entwine.

<br/>

pdfRest is also working to support more advanced AI-based digital document workflows - check back soon for announcements about new text extraction solutions that will support extracting PDF text content to send straight to ChatGPT or other AI services for summarization, interpretation, and analysis.

Do you have other ideas or requirements for integrating digitial document workflows with AI technologies? [Let us know](https://pdfrest.com/support/), and we'll be happy to help you on your journey!

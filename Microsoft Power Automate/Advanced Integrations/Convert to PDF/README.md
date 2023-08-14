<img src='https://cms.pdfrest.com/content/images/2023/08/Solution-Integrate-pdfRest-with-Microsoft-Power-Automate.png' alt='Integration with Microsoft Power Automate'>
<br/>

# Integrate pdfRest with Microsoft Power Automate
### Step-by-step instructions for setting up a low-code PDF processing workflow using pdfRest with Microsoft Power Automate
<br/>

## Overview
[Microsoft Power Automate](https://powerautomate.microsoft.com/en-us/) is a convenient tool for streamlining the creation of automated workflows. Its web interface supports the connection of hundreds of services with minimal coding requirements. While pdfRest is not currently listed within the Power Automate marketplace, pdfRest is callable as a REST API service, allowing for easy integration with Power Automate workflows and other low/no-code services.

Take a minute to [sign up for a free Starter account](https://pdfrest.com/getstarted) with pdfRest to generate a dedicated API Key, which will be required for sending calls to the service.

Watch the [video](https://youtu.be/HTaDbvh_KVE) for a quick guided tour then follow along with complete step-by-step instructions below.

## Tutorial
When working with pdfRest in Power Automate, there will always be one or more initial steps that precede the document processing steps. In other words, something has to happen first that triggers pdfRest to kick into action. These preceding hooks/steps/flows can be customized to meet your specific workflow needs. All that really matters is that at some point, you end up with one or more files that are ready to be processed using any of the [API Tools](https://pdfrest.com/apitools/) within the pdfRest toolkit. The following tutorial will demonstrate a flow that is triggered when an image file is uploaded to a OneDrive folder. This image file will automatically be processed with the pdfRest [Convert to PDF](https://pdfrest.com/apitools/convert-to-pdf/) tool, and the resulting PDF will be downloaded back into another folder in OneDrive.

Once you've set up your initial steps that produce one or more files, the first action required for integration with pdfRest is `HTTP`, which is a Power Automate premium offering and will require a license:

<img src='https://cms.pdfrest.com/content/images/2023/08/b0fd4b6f-f4f2-4c29-91f0-be6f5cd34a1f.png' alt='HTTP Premium Offering'>

With a premium license setup, type ‘http’ in the search box, and select the basic `HTTP` action as shown below:

<img src='https://cms.pdfrest.com/content/images/2023/08/2ceab802-4070-45b7-a3ca-ff5d701e764c.png' alt='Select HTTP action'>

Now you're ready to send your first call to pdfRest, which will use the [Upload Files](https://pdfrest.com/apitools/upload-files/) tool to prepare files for subsequent processing steps.

- For `Method`, select `POST`
- For `URI`, enter the endpoint `https://api.pdfrest.com/upload`
- Under `Headers`, you will need to add `Api-Key` followed by your dedicated API Key (shown here as XXXXXXXXXXX)
- Under `Body`, you will need to construct the request. This can be done by copying the template below and changing just two parts:
- Insert the content of the file you are uploading into the `“body”:` section (shown here pulling from the preceding OneDrive step)
- Insert the name of the file you are uploading into the `filename=` section of the `Content-Disposition`. NOTE: This can be done dynamically if you are uploading a wide variety of files and/or want their precise name(s) preserved after upload or by just entering in a static, generic name like `testfile.png` if you know you are only going to be processing one type of file or plan to name the output file with a different name.

<b>Body</b>:

    {
      "$content-type": "multipart/form-data",
      "$multipart": [
        {
          "body": CONTENTS_OF_FILE,
          "headers": {
            "Content-Disposition": "form-data; name=file; filename=NAME_OF_FILE"
          }
        }
      ]
    }
<img src='https://cms.pdfrest.com/content/images/2023/08/1ca98e5e-68ae-4b0c-8197-3ad9d99bf642.png' alt='POST set up'>

At this point, you can test the flow, and if this step succeeds, you will see a success with a return that looks similar to the following response:

    {
      "files": [
        {
          "name": "testfile.png",
          "id": "2bcfb3082-2701-4de2-af69-b6e128559eee"
        }
      ]
    }
<img src='https://cms.pdfrest.com/content/images/2023/08/f55909f8-5a41-4c58-8c50-2a925ed7d3f3.png' alt='POST Body'>

Now that you have a step that is uploading a file and returning a JSON response, you'll need to parse the JSON to retrieve the resource ID so that you can pass this on to the next processing step. Search for "json" to find the `Parse JSON` action.

<img src='https://cms.pdfrest.com/content/images/2023/08/bb4b6b3e-a478-433b-ac38-2ffbf8f6d3f7.png' alt='Parse Body'>

Within this step you will need to pipe the `Body` of the previous `Upload File` step into the `Content` field:

<img src='https://cms.pdfrest.com/content/images/2023/08/84048463-0e7d-4ee6-a527-5d476df01859.png' alt='Parse JSON Conent'>

You can then copy the output JSON from the previous step’s test run, press the `Generate from sample` button, paste the copied sample output, and it will automatically detect the schema for you:

<img src='https://cms.pdfrest.com/content/images/2023/08/1b4147e4-3186-4ab8-8858-3ebde8a98504.png' alt='Parse JSON Generate from Sample'>

If everything is successful, your `Parse JSON` step will look something like this:

<img src='https://cms.pdfrest.com/content/images/2023/08/9c6675aa-2768-42f4-a6fe-aa6f1885fa64.png' alt='Parse JSON Schema'>

Next, you will create another `HTTP` step, just the same as you did before, with the only differences being the `URI` (this will be the endpoint of the API Tool you select for processing - https://api.pdfrest.com/pdf for this example), and the `Body`, which will be JSON containing the ID from the previous step and any other required or optional parameters for the selected endpoint. Please consult the [API Reference Guide](https://pdfrest.com/documentation.html) for complete documentation details.

For this example, you will be sending a call to the `/pdf` endpoint with only the `id` that you parsed out of the Upload step:

<img src='https://cms.pdfrest.com/content/images/2023/08/5931d5da-42d8-4c90-94fa-8cb313926879.png' alt='Convert to PDF setup'>

Note that when you put the dynamic `id` into the file processing step it will immediately and automatically create an `Apply to each` wrapper around the step. This is because the upload functionality of the API actually allows both single and multiple file uploads in a single action and thus returns an array of uploaded files. In this example, you are only uploading and using one file, but Power Automate still noticed that there could be multiple. This does not change anything, aside from the fact that the rest of your process will be built within this `Apply to each` wrapper.

<img src='https://cms.pdfrest.com/content/images/2023/08/e1bbcee9-5ed2-481b-bbdb-cfd4ed7b3cc5.png' alt='Apply to each'>

When this `Convert to PDF` call (or any other pdfRest call) succeeds, it will return a JSON output in the form:

    {
       "outputUrl": "https://api.pdfrest.com/resource/XXXXXXXXXXX?format=file"
    
       "outputId": "XXXXXXXXXXX",
    
       "inputId": "YYYYYYYYYY"
    }

Once again, you will need to parse the JSON response using the `Parse JSON` action by piping the `Body` into the `Content` field and copy/pasting a test response into the `Schema`.

<img src='https://cms.pdfrest.com/content/images/2023/08/895fbb64-8234-4b0a-9f97-884f054de86e.png' alt='Parse JSON Schema for PDF Conversion Response'>

At this point, you can continue chaining calls by adding more pairs of `HTTP` and `Parse JSON` steps, passing the ID from a previous step into a new processing step.

To complete this example, you can pipe the `outputUrl` from the previous Parse JSON step directly into an `Upload file from URL` action to upload the output PDF file back into OneDrive for storage.

<img src='https://cms.pdfrest.com/content/images/2023/08/7d2fdf03-1b53-4e2a-9421-595130b7219f.png' alt='Upload file from URL'>

Of course, you can customize the end step to pass pdfRest output files anywhere you need to send them for the next steps in their journey.


## Support
If you have any trouble getting this set up or would like more information about how pdfRest can solve your PDF processing challenges, please [let us know](https://pdfrest.com/support/) how we can help!

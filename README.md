![pdfRest](https://cms.pdfrest.com/content/images/2022/11/pdfRest_logo_tag_750_275_light_bg.png)

[pdfRest.com](https://pdfrest.com/)&nbsp;|&nbsp; [Get Started](https://pdfrest.com/getstarted/) &nbsp;|&nbsp; [API Lab](https://pdfrest.com/apilab/)&nbsp;|&nbsp; [Explore the Toolkit](https://pdfrest.com/apitools/) &nbsp;|&nbsp; [Solutions](https://pdfrest.com/learning/solutions/) &nbsp;|&nbsp; [Pricing](https://pdfrest.com/pricing/) &nbsp;|&nbsp; [Documentation](https://docs.pdfrest.com/) &nbsp;|&nbsp; [Support](https://pdfrest.com/support/)

<br>

## pdfRest API Toolkit

pdfRest is a REST API Toolkit for developers with all of the API Tools you'll need to power your PDF processing requirements, designed and documented by PDF experts to make your development work easier than ever. Rapidly integrate PDF capabilities into web applications in minutes, not days.

| [Compress PDF](https://pdfrest.com/apitools/compress-pdf/) | [Convert to PDF](https://pdfrest.com/apitools/convert-to-pdf/) | **[Convert to PDF/A](https://pdfrest.com/apitools/convert-to-pdfa/)** | [Convert to PDF/X](https://pdfrest.com/apitools/convert-to-pdfx/) |
| :--- | :--- | :--- | :--- |
| **[Encrypt PDF](https://pdfrest.com/apitools/encrypt-pdf/)** | **[Restrict PDF](https://pdfrest.com/apitools/restrict-pdf/)** | **[Merge PDFs](https://pdfrest.com/apitools/merge-pdfs/)** | **[Split PDF](https://pdfrest.com/apitools/split-pdf/)** |
| **[Decrypt PDF](https://pdfrest.com/apitools/encrypt-pdf/)** | **[Unrestrict PDF](https://pdfrest.com/apitools/restrict-pdf/)** | **[Add to PDF](https://pdfrest.com/apitools/add-to-pdf/)** | **[PDF to Images](https://pdfrest.com/apitools/pdf-to-images/)** |
| **[Watermark PDF](https://pdfrest.com/apitools/watermark-pdf/)** | **[Flatten Transparencies](https://pdfrest.com/apitools/flatten-transparencies/)** | **[Flatten Annotations](https://pdfrest.com/apitools/flatten-annotations/)** | **[Flatten Layers](https://pdfrest.com/apitools/flatten-layers/)** |
| **[Query PDF](https://pdfrest.com/apitools/query-pdf/)** | **[Linearize PDF](https://pdfrest.com/apitools/linearize-pdf/)** | **[Upload Files](https://pdfrest.com/apitools/upload-files/)** | **[Zip Files](https://pdfrest.com/apitools/zip-files/)** |
| **[Flatten Forms](https://pdfrest.com/apitools/flatten-forms/)** | **[Import Form Data](https://pdfrest.com/apitools/import-form-data/)** | **[Export Form Data](https://pdfrest.com/apitools/export-form-data/)** | **[Extract Text](https://pdfrest.com/apitools/extract-text/)** |
| **[PDF to Word](https://pdfrest.com/apitools/pdf-to-word/)** | **[PDF to Excel](https://pdfrest.com/apitools/pdf-to-excel/)** | **[PDF to PowerPoint](https://pdfrest.com/apitools/pdf-to-powerpoint/)** | **[Extract Images](https://pdfrest.com/apitools/extract-images/)** |
| **[OCR to PDF](https://pdfrest.com/apitools/ocr-pdf/)** | **[API Polling](https://pdfrest.com/apitools/api-polling/)** | **[Rasterize PDF](https://pdfrest.com/apitools/rasterize-pdf/)** | **[Convert PDF Colors](https://pdfrest.com/apitools/convert-pdf-colors/)** |
| **[Redact PDF](https://pdfrest.com/apitools/redact-pdf/)** | **[PDF to Markdown](https://pdfrest.com/apitools/pdf-to-markdown/)** | **[Sign PDF](https://pdfrest.com/apitools/sign-pdf/)** | |
<br>

Get started quickly with our [API Lab](https://pdfrest.com/apilab/) web app, trust in high quality Adobe® technology, and keep costs to a minimum with the best fit plan for every business application (including a generous free plan).

<br>

## Integrate Advanced PDF Solutions

- **Automatic Document Processes:** Improve business workflows by automatically converting received documents to PDF, pulling critical data, managing records for adherence, and beyond.

- **Implement PDF Features in Applications:** Easily integrate pdfRest's abilities within your existing software to provide strong PDF processing features such as optimizing file submissions, obtaining form values, or protecting documents.

- **Artificial Intelligence for PDF Automation:** Connect with AI solutions to streamline and automate sophisticated PDF operations. For instance, [pdfAssistant.ai](https://pdfassistant.ai/) leverages pdfRest's powerful API to deliver an intelligent assistant that can perform a range of PDF processing actions from a simple, chat-based interface.

<br>

## Getting Started with Code Samples

This GitHub repository provides public access to code examples that demonstrate how to programmatically submit requests to the [pdfRest API Toolkit](https://pdfrest.com/) service.

Start by [generating a free API Key](https://pdfrest.com/getstarted/), required to run these samples. Create an account with a Starter plan for 100 free API Calls per month. [Plans](https://pdfrest.com/pricing/) scale up from there to support all projects and applications, big and small.

<br>

## Instructions for Running Code Samples

### Uploading an input file

POST requests require an API Key. To apply your API Key, you must include it in your requests as a header called `Api-Key`.

Before running each sample program, look for a comment that reads:

> `Place your api key here`

and replace `xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx` with your API Key.

### Downloading your output file(s)

Each subdirectory includes a `get-resource` sample that demonstrates how to download output files.

When you make a POST call to one of the API endpoints, you will receive back a response that includes an ID reference to each resource, including newly uploaded input files and newly generated output files. These IDs are in the form of a universally unique identifier (UUID).

Before running this sample program, look for a comment that reads:

> `place resource uuid here`

and replace `xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx` with the resource UUID you received back from a previous POST request. You may also wish to update the variable containing the output file name before sending your GET request.

### Optional deletion of sensitive files

Many language samples include an optional step to delete uploaded or generated files from pdfRest servers (e.g., unredacted, unencrypted, unrestricted, or unwatermarked files). Deletion is off by default. To enable it, edit the sample file and set its local deletion flag to true for that language, for example:

- JavaScript: `const DELETE_SENSITIVE_FILES = false;` → `true`
- Python: `DELETE_SENSITIVE_FILES = False` → `True`
- PHP: `$DELETE_SENSITIVE_FILES = false;` → `true`
- .NET (C#): `var deleteSensitiveFiles = false;` → `true`
- Java: `final boolean DELETE_SENSITIVE_FILES = false;` → `true`
- cURL: `# DELETE_SENSITIVE_FILES=true` → `DELETE_SENSITIVE_FILES=true`

Refer to each language README for details.


## API Documentation

After you've successfully sent an API Call using these examples, take a look at the [API Reference Guide](https://docs.pdfrest.com/cloud-api-reference/) for a full description of each endpoint and parameters you can adjust to customize your solution.

<br>

## Postman Collection

Use Postman to fast-track your API testing! Check out the pdfRest [Postman Collection](https://www.postman.com/pdfrest) to start sending POST requests with just a few clicks from pre-configured POST requests.

<br>

## Support

If you have any trouble getting started, please reach out to us through our [Support](https://pdfrest.com/support) form, and a member of the pdfRest team will be in touch as soon as possible.

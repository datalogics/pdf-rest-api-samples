![pdfRest](https://cms.pdfrest.com/content/images/2022/11/pdfRest_logo_tag_750_275_light_bg.png)

[pdfRest.com](https://pdfrest.com)&nbsp;|&nbsp; [Get Started](https://pdfrest.com/getstarted) &nbsp;|&nbsp; [Explore the Toolkit](https://pdfrest.com/apitoolkit) &nbsp;|&nbsp; [Solutions](https://pdfrest.com/solutions/) &nbsp;|&nbsp; [Pricing](https://pdfrest.com/pricing) &nbsp;|&nbsp; [Documentation](https://pdfrest.com/documentation.html) &nbsp;|&nbsp; [Support](https://pdfrest.com/support)

<br>

## pdfRest API Toolkit

pdfRest is a REST API Toolkit for developers with all of the API Tools you'll need to power your PDF processing requirements, designed and documented by PDF experts to make your development work easier than ever. Rapidly integrate PDF capabilities into web applications in minutes, not days.

| [Compress PDF](https://pdfrest.com/apitoolkit/compress-pdf/)       | [Convert to PDF](https://pdfrest.com/apitoolkit/convert-to-pdf/)                     | [Merge PDFs](https://pdfrest.com/apitoolkit/merge-pdfs/)           | [Split PDF](https://pdfrest.com/apitoolkit/split-pdf/)                  |
| :----------------------------------------------------------------- | :----------------------------------------------------------------------------------- | :----------------------------------------------------------------- | :---------------------------------------------------------------------- |
| **[Encrypt PDF](https://pdfrest.com/apitoolkit/encrypt-pdf/)**     | **[Restrict PDF](https://pdfrest.com/apitoolkit/restrict-pdf/)**                     | **[PDF to Images](https://pdfrest.com/apitoolkit/pdf-to-images/)** | **[Convert to PDF/A](https://pdfrest.com/apitoolkit/convert-to-pdfa/)** |
| **[Decrypt PDF](https://pdfrest.com/apitoolkit/encrypt-pdf/)**     | **[Unrestrict PDF](https://pdfrest.com/apitoolkit/restrict-pdf/)**                   | **[Add to PDF](https://pdfrest.com/apitoolkit/add-to-pdf/)**       | **[Query PDF](https://pdfrest.com/apitoolkit/query-pdf/)**              |
| **[Watermark PDF](https://pdfrest.com/apitoolkit/watermark-pdf/)** | **[Flatten Transparencies](https://pdfrest.com/apitoolkit/flatten-transparencies/)** | **[Linearize PDF](https://pdfrest.com/apitoolkit/linearize-pdf/)** | **[Zip Files](https://pdfrest.com/apitoolkit/zip-files/)**              |

<br>

Get started quickly with our "1 Minute to REST, Easy" guarantee, trust in high quality Adobe® technology, and keep costs to a minimum with the best fit plan for every business application (including a generous free plan).

<br>

## Getting Started with Code Samples

This GitHub repository provides public access to code examples that demonstrate how to programmatically submit requests to the [pdfRest API Toolkit](https://pdfrest.com) service.

Start by [generating a free API Key](https://pdfrest.com/getstarted), required to run these samples. Choose between a Guest API Key for 50 free API Calls before signing up, or create an account with a Starter plan for 300 free API Calls per month. [Plans](https://pdfrest.com/pricing) scale up from there to support all projects and applications, big and small.

<br>

## Instructions for Running Code Samples

### Uploading an input file

POST requests require an API Key. To apply your API Key, you must include it in your requests as a header called `Api-Key`.

Before running each sample program, look for a comment that reads:

> `Place your api key here`

and replace `xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx` with your API Key.

### Downloading your output file(s)

Each subdirectory includes a `get-resource-id-endpoint` sample that demonstrates how to download output files.

When you make a POST call to one of the API endpoints, you will receive back a response that includes an ID reference to each resource, including newly uploaded input files and newly generated output files. These IDs are in the form of a universally unique identifier (UUID).

Before running this sample program, look for a comment that reads:

> `place resource uuid here`

and replace `xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx` with the resource UUID you received back from a previous POST request. You may also wish to update the variable containing the output file name before sending your GET request.

### Running a sample program

Below are instructions for running a sample in each language included in this repository.

#### How to run Node samples

Using Node 16 or greater:

1. `cd Node/`
2. `npm install node-fetch`
3. `node name-of-sample-program.js`

#### How to run PHP samples

1. `cd PHP/`
2. `php name-of-sample-program.php`

#### How to run Python samples

Using Python3:

1. `cd Python/`
2. `python name-of-sample-program.py`

<br>

## API Documentation

After you've successfully sent an API Call using these examples, take a look at the [Documentation](https://pdfrest.com/documentation.html) for each API endpoint for a full description of parameters you can adjust to customize your solution.

<br>

## Postman Collection

Use Postman to fast-track your API testing! Download the pdfRest [Postman Collection](https://github.com/datalogics/pdf-rest-postman-collection) to load all of the pdfRest API Tools directly into Postman to start sending POST requests with just a few clicks.

<br>

## Support

If you have any trouble getting started, please reach out to us through our [Support](https://pdfrest.com/support) form, and a member of the pdfRest team will be in touch as soon as possible.

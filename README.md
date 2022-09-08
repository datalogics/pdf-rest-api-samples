# pdf-rest-api-samples by pdfRest

## About pdfRest API Toolkit code samples
This GitHub repository provides public access to sample scripts that demonstrate how to programmatically submit requests to the pdfRest API Toolkit service.

Start by [generating a free API Key](https://pdfrest.com/getstarted/), required to run these samples.  Choose between a Guest API Key for 50 free API Calls before signing up, or create an account with a Starter plan for 300 free API Calls per month. Plans scale up from there to support all projects and applications, big and small.

## Instructions

### Uploading an input file

POST requests require an API Key. To apply your API Key, you must include it in your requests as a header called `Api-Key`.

Before running each sample program, look for a comment that reads:
> `Place your api key here`

and replace `xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx` with your API Key.

### Downloading your output file(s)

Each subdirectory includes a `get-resource-id-endpoint` sample that demonstrates how to download output files.

When you make a POST call to one of the API endpoints, you will receive back a response that includes an ID reference to each resource, including newly uploaded input files and newly generated output files.  These IDs are in the form of a universally unique identifier (UUID).

Before running this sample program, look for a comment that reads:
> `place resource uuid here`

and replace `xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx` with the resource UUID you received back from a previous POST request. You may also wish to update the variable containing the output file name before sending your GET request.

### Running a sample program

Below are instructions for running a sample in each language included in this repository.

#### How to run Node samples
Using Node 14 or greater:
1. `cd Node/`
2. `npm install node-fetch` (need only run once)
3. `node name-of-sample-program.js`

#### How to run PHP samples
1. `cd PHP/`
2. `php name-of-sample-program.php`

#### How to run Python samples
Using Python3:
1. `cd Python/`
2. `python name-of-sample-program.py`

# pdf-rest-api-samples

## About our PDF REST API code samples
This GitHub repository provides public access to sample scripts that demonstrate how to programmatically submit requests to the PDF REST APIs service.

Start by [requesting an API Key](https://www.datalogics.com/rest-apis-form/), which provides free access to up to 250 API calls per month. This is required to run these samples.  After filling out the form, you will immediately receive an email with your API Key.

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

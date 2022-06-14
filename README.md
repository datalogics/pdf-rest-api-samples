# pdf-rest-api-samples

## About our PDF REST API sample programs
This GitHub repository provides public access to sample programs that demonstrate how you can programmatically submit requests to our PDF Rest API.

An API key is required to run these samples. For more information, including how to obtain a free API key, please visit [our website](https://www.datalogics.com/products/cloud/pdf-rest-apis/).

## Instructions

### Uploading an input file

POST requests require an API key. To apply your API key, you must include it in your requests as a header called `Api-Key`.

Before running each sample program, look for a comment that reads:
> `Place your api key here`

and replace `xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx` with your API key.

### Downloading your output file(s)

Each subdirectory includes a `get-resource-id-endpoint` sample program that you can run to retrieve your output files.

Before running the sample program, look for a comment that reads:
> `place resource uuid here`

and replace `xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx` with the resource UUID. You may also wish to update the variable containing the output file name before sending your GET request.

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

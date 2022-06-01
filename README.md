# pdf-rest-api-samples

## About our PDF REST API sample programs
This GitHub repository provides public access to sample programs that demonstrate how you can programmatically submit requests to our PDF Rest API.

An API key is required to run these samples. For more information, including how to obtain a free API key, please visit [our website](https://www.datalogics.com/products/cloud/pdf-rest-apis/).

## Running a sample program

To apply your API key, you must include it in your requests as a header called `Api-Key`. Look for a comment that reads:
> `Place your api key here`

and replace `xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx` with your API key.

Below are instructions for running a sample in each language included in this repository.

### How to run Node samples
Using Node 13 or greater:
1. `cd Node/`
2. `npm install node-fetch` (need only run once)
3. `node name-of-sample-program.js`

### How to run PHP samples
1. `cd PHP/`
2. `php name-of-sample-program.php`

### How to run Python samples
Using Python3:
1. `cd Python/`
2. `python name-of-sample-program.py`

// This request demonstrates how to decrypt a password-protected PDF by removing the password and requires that the current password be provided.
var axios = require('axios');
var FormData = require('form-data');
var fs = require('fs');

// Create a new form data instance and append the PDF file and parameters to it
var data = new FormData();
data.append('file', fs.createReadStream('/path/to/file'));
data.append('current_open_password', 'current_example_pw');
data.append('output', 'pdfrest_decrypted_pdf'); 

// Define configuration options for axios request
var config = {
  method: 'post',
  maxBodyLength: Infinity, // Set maximum length of the request body
  url: 'https://api.pdfrest.com/decrypted-pdf',
  headers: { 
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx', // Replace with your API key
    ...data.getHeaders() // Set headers with form data headers
  },
  data : data // Add headers to the request
};

// Send request and handle response or error
axios(config)
  .then(function (response) {
    console.log(JSON.stringify(response.data));
  })
  .catch(function (error) {
    console.log(error);
  });

// If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.js' sample

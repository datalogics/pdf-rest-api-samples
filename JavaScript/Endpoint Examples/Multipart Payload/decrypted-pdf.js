// This request demonstrates how to decrypt a password-protected PDF by removing the password and requires that the current password be provided.
var axios = require('axios');
var FormData = require('form-data');
var fs = require('fs');

// Toggle deletion of sensitive files (default: false)
const DELETE_SENSITIVE_FILES = false;

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

  // All files uploaded or generated are automatically deleted based on the 
  // File Retention Period as shown on https://pdfrest.com/pricing. 
  // For immediate deletion of files, particularly when sensitive data 
  // is involved, an explicit delete call can be made to the API.
  //
  // The following code is an optional step to delete sensitive files
  // (unredacted, unencrypted, unrestricted, or unwatermarked) from pdfRest servers.

  var body = response.data;
  var result_id = body.outputId;
    var delete_config = {
      method: 'post',
      maxBodyLength: Infinity,
      url: 'https://api.pdfrest.com/delete',
      headers: {
        'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
        'Content-Type': 'application/json'
      },
      data: { ids: result_id }
    };

  if (DELETE_SENSITIVE_FILES) {
    axios(delete_config)
      .then(function (delete_response) { console.log(JSON.stringify(delete_response.data)); })
      .catch(function (error) { console.log(error); });
  }
  
})
  .catch(function (error) {
    console.log(error);
  });

// If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.js' sample

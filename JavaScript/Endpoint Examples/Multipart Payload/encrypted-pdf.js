// This request demonstrates how to apply 256-bit AES encryption with a 32-bit key. Encrypted PDFs cannot be viewed without first providing the open password.
var axios = require('axios');
var FormData = require('form-data');
var fs = require('fs');

// Toggle deletion of sensitive files (default: false)
const DELETE_SENSITIVE_FILES = false;

// Create a new form data instance and append the file and parameters to it
var data = new FormData();
data.append('file', fs.createReadStream('/path/to/file'));
data.append('new_open_password', 'new_example_pw');
data.append('output', 'pdfrest_encrypted_pdf');

// define configuration options for axios request
var config = {
  method: 'post',
maxBodyLength: Infinity, // set maximum length of the request body
  url: 'https://api.pdfrest.com/encrypted-pdf',
  headers: { 
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx', // Replace with your API key
    ...data.getHeaders() // set headers for the request
  },
  data : data // set the data to be sent with the request
};

// send request and handle response or error
axios(config)
.then(function (response) {
  console.log(JSON.stringify(response.data));
  var output_id = response.data.outputId;

  // All files uploaded or generated are automatically deleted based on the 
  // File Retention Period as shown on https://pdfrest.com/pricing. 
  // For immediate deletion of files, particularly when sensitive data 
  // is involved, an explicit delete call can be made to the API.
  //
  // Deletes all files in the workflow, including outputs. Save all desired files before enabling this step.

  var body = response.data;
  var input_id = body.inputId;
  var delete_config = {
    method: 'post',
    maxBodyLength: Infinity,
    url: 'https://api.pdfrest.com/delete',
    headers: {
      'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
      'Content-Type': 'application/json'
    },
    data: { ids: `${input_id}, ${output_id}` }
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

// If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.js' sample.

/**
 * This request demonstrates how to apply text redaction previews to a PDF.
 * This is step 1 of 2 toward applying redactions to text.
 */
var axios = require('axios');
var FormData = require('form-data');
var fs = require('fs');

// Toggle deletion of sensitive files (default: false)
const DELETE_SENSITIVE_FILES = false;

// Create a new form data instance and append the PDF file and parameters to it
var data = new FormData();
data.append('file', fs.createReadStream('/path/to/file'));
var redaction_option_array = [];
var redaction_options1 = {
    "type": "preset",
    "value": "email",
};
var redaction_options2 = {
    "type": "regex",
    "value": "(\\+\\d{1,2}\\s)?\\(?\\d{3}\\)?[\\s.-]\\d{3}[\\s.-]\\d{4}",
};
var redaction_options3 = {
    "type": "literal",
    "value": "word",
};
redaction_option_array.push(redaction_options1);
redaction_option_array.push(redaction_options2);
redaction_option_array.push(redaction_options3);
data.append('redactions', JSON.stringify(redaction_option_array));
data.append('output', 'pdfrest_pdf_with_redacted_text_preview');

// define configuration options for axios request
var config = {
  method: 'post',
  maxBodyLength: Infinity, // set maximum length of the request body
  url: 'https://api.pdfrest.com/pdf-with-redacted-text-preview', 
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

  // All files uploaded or generated are automatically deleted based on the 
  // File Retention Period as shown on https://pdfrest.com/pricing. 
  // For immediate deletion of files, particularly when sensitive data 
  // is involved, an explicit delete call can be made to the API.
  //
  // The following code is an optional step to delete sensitive files
  // (unredacted, unencrypted, unrestricted, or unwatermarked) from pdfRest servers.

  var body = response.data;
  var input_id = body.inputId;
  var preview_id = body.outputId;
  // IMPORTANT: Do not delete the preview_id (the preview PDF) file until after the redaction is applied
  // with the /pdf-with-redacted-text-applied endpoint.
  var delete_config = {
    method: 'post',
    maxBodyLength: Infinity,
    url: 'https://api.pdfrest.com/delete',
    headers: {
      'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
      'Content-Type': 'application/json'
    },
    data: { ids: input_id + ', ' + preview_id }
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

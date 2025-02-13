/**
 * This request demonstrates how to apply text redaction previews to a PDF.
 * This is step 1 of 2 toward applying redactions to text.
 */
var axios = require('axios');
var FormData = require('form-data');
var fs = require('fs');

// Create a new form data instance and append the PDF file and parameters to it
var data = new FormData();
data.append('file', fs.createReadStream('/path/to/file'));
var redaction_option_array = [];
var redaction_options1 = {
    "type": "preset",
    "value": "uuid",
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
})
.catch(function (error) {
  console.log(error); 
});

// If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.js' sample.
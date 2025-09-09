/**
 * This request demonstrates how to apply a text watermark to a PDF.
 * Horizontal and vertical offsets of the watermark are measured in PDF units. (1 inch = 72 PDF units)
 */
var axios = require('axios');
var FormData = require('form-data');
var fs = require('fs');

// By default, we use the US-based API service. This is the primary endpoint for global use.
var apiUrl = "https://api.pdfrest.com";

/* For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
 * For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 */
//var apiUrl "https://eu-api.pdfrest.com";

// Toggle deletion of sensitive files (default: false)
const DELETE_SENSITIVE_FILES = false;

// Create a new form data instance and append the PDF file and parameters to it
var data = new FormData();
data.append('file', fs.createReadStream('/path/to/file'));
data.append('watermark_text', 'Hello, watermarked world!');
data.append('font', 'Arial');
data.append('text_size', '72');
data.append('text_color_rgb', '255,0,0');
data.append('opacity', '0.5');
data.append('x', '0');
data.append('y', '0');
data.append('rotation', '0');
data.append('output', 'pdfrest_watermarked_pdf');

// define configuration options for axios request
var config = {
  method: 'post',
  maxBodyLength: Infinity, // set maximum length of the request body
  url: apiUrl + '/watermarked-pdf',
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
  var input_id = body.inputId[0];
  var delete_config = {
    method: 'post',
    maxBodyLength: Infinity,
    url: apiUrl + '/delete',
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

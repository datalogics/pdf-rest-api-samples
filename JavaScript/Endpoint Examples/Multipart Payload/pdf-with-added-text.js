/**
 * This request demonstrates how to add text to a PDF.
 * Horizontal and vertical offsets of the text are measured in PDF units. (1 inch = 72 PDF units)
 */
var axios = require('axios');
var FormData = require('form-data');
var fs = require('fs');

// By default, we use the US-based API service. This is the primary endpoint for global use.
var apiUrl = "https://api.pdfrest.com";

/* For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
 * For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 */
//var apiUrl = "https://eu-api.pdfrest.com";

// Create a new form data instance and append the PDF file and parameters to it
var data = new FormData();
data.append('file', fs.createReadStream('/path/to/file'));
var text_option_array = [];
var text_options = {
    "font":"Times New Roman",
    "max_width":"175",
    "opacity":"1",
    "page":"1",
    "rotation":"0",
    "text":"sample text in PDF",
    "text_color_rgb":"0,0,0",
    "text_size":"30",
    "x":"72",
    "y":"144"
};
text_option_array.push(text_options);
data.append('text_objects', JSON.stringify(text_option_array));
data.append('output', 'pdfrest_pdf_with_added_text');

// define configuration options for axios request
var config = {
  method: 'post',
  maxBodyLength: Infinity, // set maximum length of the request body
  url: apiUrl + '/pdf-with-added-text',
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

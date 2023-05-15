/**
 * This request demonstrates how to apply a file watermark to a PDF. This kind of watermark uses another PDF (using either watermark_file or watermark_file_id as a form-data parameter) and applies it as a watermark to the primary input document (file or id).
 * Horizontal and vertical offsets of the watermark are measured in PDF units. (1 inch = 72 PDF units)
 */
var axios = require('axios');
var FormData = require('form-data');
var fs = require('fs');

// Create a new form data instance and append the PDF file and parameters to it
var data = new FormData();
data.append('file', fs.createReadStream('/path/to/file'));
data.append('watermark_file', fs.createReadStream('/path/to/file'));
data.append('watermark_file_scale', '0.5');
data.append('opacity', '0.5');
data.append('x', '0');
data.append('y', '0');
data.append('rotation', '0');
data.append('output', 'pdfrest_watermarked_pdf');

// define configuration options for axios request
var config = {
  method: 'post',
  maxBodyLength: Infinity, // set maximum length of the request body
  url: 'https://api.pdfrest.com/pdf-with-added-image', 
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

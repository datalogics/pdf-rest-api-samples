/**
 * This request demonstrates how to split a PDF document into three files by specifying the pages that will be included in each.
 * pages[] is an optional parameter that accepts page numbers and/or ranges separated by commas. In addition, pages[] supports several keywords: even, odd, and last.
 */
var axios = require('axios');
var FormData = require('form-data');
var fs = require('fs');

// Create a new form data instance and append the PDF file and parameters to it
var data = new FormData();
data.append('file', fs.createReadStream('/path/to/file'));
data.append('pages[]', 'even');
data.append('pages[]', 'odd');
data.append('pages[]', '1,3,4-6');
data.append('output', 'pdfrest_split_pdf');

// define configuration options for axios request
var config = {
  method: 'post',
  maxBodyLength: Infinity, // set maximum length of the request body
  url: 'https://api.pdfrest.com/split-pdf', 
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

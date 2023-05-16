// This request demonstrates how to convert a PDF into PNG image files, one per PDF page.
var axios = require('axios');
var FormData = require('form-data');
var fs = require('fs');

// Create a new form data instance and append the file and parameters to it
var data = new FormData();
data.append('file', fs.createReadStream('/path/to/file')); 
data.append('pages', '1-last');
data.append('resolution', '300');
data.append('color_model', 'rgb');
data.append('output', 'pdfrest_png');

// define configuration options for axios request
var config = {
  method: 'post',
  maxBodyLength: Infinity, // set maximum length of the request body
  url: 'https://api.pdfrest.com/png', 
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
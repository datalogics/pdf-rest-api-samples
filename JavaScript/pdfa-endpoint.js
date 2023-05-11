var axios = require('axios');
var FormData = require('form-data');
var fs = require('fs');

// Create a new form data instance and append the PDF file and parameters to it
var data = new FormData();
data.append('file', fs.createReadStream('/path/to/file'));
data.append('output_type', 'PDF/A-2b'); 
data.append('rasterize_if_errors_encountered', 'off');
data.append('output', 'pdfrest_pdfa');

// Set the configuration options for the request
var config = {
  method: 'post',
  maxBodyLength: Infinity, 
  url: 'https://api.pdfrest.com/pdfa',
  headers: { 
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx', // Replace with your API key
    ...data.getHeaders()
  },
  data : data
};

// Send the request and handle the response or error
axios(config)
.then(function (response) {
  console.log(JSON.stringify(response.data));
})
.catch(function (error) {
  console.log(error);
});


// If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.js' sample.

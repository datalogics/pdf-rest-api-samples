var axios = require('axios');
var FormData = require('form-data');
var fs = require('fs');

// Create a new form data instance and append the PDF file and parameters to it
var data = new FormData();
data.append('file', fs.createReadStream('/path/to/file'));
data.append('pages[]', '1-last');
data.append('type[]', 'file');
data.append('file', fs.createReadStream('/path/to/file'));
data.append('pages[]', '1-last');
data.append('type[]', 'file');
data.append('output', 'pdfrest_merged_pdf');

// set request configuration
var config = {
  method: 'post',
maxBodyLength: Infinity,
  url: 'https://api.pdfrest.com/merged-pdf',
  headers: { 
    'Api-Key': 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx', // Replace with your API key
    ...data.getHeaders()
  },
  data : data
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

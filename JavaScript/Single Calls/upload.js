// This request demonstrates how to upload . 
var axios = require('axios');
var FormData = require('form-data');
var fs = require('fs');

// Create a new form data object and append the PDF file and parameters to it
var data = new FormData();
data.append("file", fs.createReadStream("/path/to/file"));
data.append("file", fs.createReadStream("/path/to/file"));
data.append("file", fs.createReadStream("/path/to/file"));

// define configuration options for axios request
var config = {
  method: 'post',
  maxBodyLength: Infinity, // set maximum length of the request body
  url: 'https://api.pdfrest.com/upload', 
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

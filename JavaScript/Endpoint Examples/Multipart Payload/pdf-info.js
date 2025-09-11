// This request demonstrates how to get detailed information about a PDF document and its contents to assess the current state of the file and drive conditional processing.
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
data.append('queries', 'tagged,image_only,title,subject,author,producer,creator,creation_date,modified_date,keywords,doc_language,page_count,contains_annotations,contains_signature,pdf_version,file_size,filename,restrict_permissions_set,contains_xfa,contains_acroforms,contains_javascript,contains_transparency,contains_embedded_file,uses_embedded_fonts,uses_nonembedded_fonts,pdfa,requires_password_to_open');

// define configuration options for axios request
var config = {
  method: 'post',
  maxBodyLength: Infinity, // set maximum length of the request body
  url: apiUrl + '/pdf-info',
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

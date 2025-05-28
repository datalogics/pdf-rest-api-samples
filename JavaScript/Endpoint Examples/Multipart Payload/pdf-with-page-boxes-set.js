/**
 * This request demonstrates how to alter the margins of the page boxes in a PDF.
 */
var axios = require("axios");
var FormData = require("form-data");
var fs = require("fs");

// Create a new form data instance and append the PDF file and parameters to it
var data = new FormData();
data.append("file", fs.createReadStream("/path/to/file"));
const boxOptions = {
  boxes: [
    {
      box: "media",
      pages: [
        {
          range: "1",
          left: 100,
          top: 100,
          bottom: 100,
          right: 100,
        },
      ],
    },
  ],
};
data.append("boxes", JSON.stringify(boxOptions));
data.append("output", "example_out");

// define configuration options for axios request
var config = {
  method: "post",
  maxBodyLength: Infinity, // set maximum length of the request body
  url: "https://api.pdfrest.com/pdf-with-page-boxes-set",
  headers: {
    "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
    ...data.getHeaders(), // set headers for the request
  },
  data: data, // set the data to be sent with the request
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

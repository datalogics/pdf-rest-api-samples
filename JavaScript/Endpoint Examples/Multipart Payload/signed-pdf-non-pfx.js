/**
 * This request demonstrates how to apply a digital signature to a PDF using PFX credentials.
 */
var axios = require("axios");
var FormData = require("form-data");
var fs = require("fs");

// Create a new form data instance and append the PDF file and parameters to it
var data = new FormData();
data.append("file", fs.createReadStream("/path/to/file"));
data.append("certificate_file", fs.createReadStream("/path/to/certificate.pem"));
data.append("private_key_file", fs.createReadStream("/path/to/private_key.pem"));
const signature_config = {
    type: "new",
    name: "esignature",
    location: {
        bottom_left: { x: "0", y: "0" },
        top_right: { x: "216", y: "72" },
        page: 1
    },
    display: {
        include_datetime: "true"
    }
};
data.append("signature_configuration", JSON.stringify(signature_config));
data.append("output", "example_out");

var signed_pdf_config = {
    method: "post",
    maxBodyLength: Infinity,
    url: "https://api.pdfrest.com/signed-pdf",
    headers: {
        "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
        ...data.getHeaders(), // set headers for the request
    },
    data: data,
};

// send request and handle response or error
axios(signed_pdf_config)
  .then(function (response) {
    console.log(JSON.stringify(response.data));
  })
  .catch(function (error) {
    console.log(error);
  });

// If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.js' sample.

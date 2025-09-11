var axios = require("axios");
var FormData = require("form-data");
var fs = require("fs");

// By default, we use the US-based API service. This is the primary endpoint for global use.
var apiUrl = "https://api.pdfrest.com";

/* For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
 * For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 */
//var apiUrl = "https://eu-api.pdfrest.com";

var pdf_upload_data = fs.createReadStream("/path/to/pdf_file");

var pdf_upload_config = {
  method: "post",
  maxBodyLength: Infinity,
  url: apiUrl + "/upload",
  headers: {
    "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
    "Content-Filename": "pdf_filename.pdf",
    "Content-Type": "application/octet-stream",
  },
  data: pdf_upload_data, // set the data to be sent with the request
};

// send request and handle response or error
axios(pdf_upload_config)
  .then(function (pdf_upload_response) {
    var pdf_uploaded_id = pdf_upload_response.data.files[0].id;

    var data_upload_data = fs.createReadStream("/path/to/data_file.xml");

    var data_upload_config = {
      method: "post",
      maxBodyLength: Infinity,
      url: apiUrl + "/upload",
      headers: {
        "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
        "Content-Filename": "data_filename.xml",
        "Content-Type": "application/octet-stream",
      },
      data: data_upload_data, // set the data to be sent with the request
    };

    axios(data_upload_config)
      .then(function (data_upload_response) {
        console.log(JSON.stringify(data_upload_response.data));
        var data_uploaded_id = data_upload_response.data.files[0].id;

        var import_config = {
          method: "post",
          maxBodyLength: Infinity,
          url: apiUrl + "/pdf-with-imported-form-data",
          headers: {
            "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
            "Content-Type": "application/json",
          },
          data: {
            id: pdf_uploaded_id,
            data_file_id: data_uploaded_id,
          }, // set the data to be sent with the request
        };

        // send request and handle response or error
        axios(import_config)
          .then(function (import_response) {
            console.log(JSON.stringify(import_response.data));
          })
          .catch(function (error) {
            console.log(error);
          });
      })
      .catch(function (error) {
        console.log(error);
      });
  })
  .catch(function (error) {
    console.log(error);
  });

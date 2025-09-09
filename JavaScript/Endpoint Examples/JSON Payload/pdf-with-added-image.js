var axios = require("axios");
var FormData = require("form-data");
var fs = require("fs");

// By default, we use the US-based API service. This is the primary endpoint for global use.
var apiUrl = "https://api.pdfrest.com";

/* For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
 * For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 */
//var apiUrl "https://eu-api.pdfrest.com";

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

    var image_upload_data = fs.createReadStream("/path/to/image_file");

    var image_upload_config = {
      method: "post",
      maxBodyLength: Infinity,
      url: apiUrl + "/upload",
      headers: {
        "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
        "Content-Filename": "image_filename.png",
        "Content-Type": "application/octet-stream",
      },
      data: image_upload_data, // set the data to be sent with the request
    };

    axios(image_upload_config)
      .then(function (image_upload_response) {
        console.log(JSON.stringify(image_upload_response.data));
        var image_uploaded_id = image_upload_response.data.files[0].id;

        var add_image_config = {
          method: "post",
          maxBodyLength: Infinity,
          url: apiUrl + "/pdf-with-added-image",
          headers: {
            "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
            "Content-Type": "application/json",
          },
          data: {
            id: pdf_uploaded_id,
            image_id: image_uploaded_id,
            x: 0,
            y: 0,
            page: 1,
          }, // set the data to be sent with the request
        };

        // send request and handle response or error
        axios(add_image_config)
          .then(function (add_image_response) {
            console.log(JSON.stringify(add_image_response.data));
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

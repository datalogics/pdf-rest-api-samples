var axios = require("axios");
var FormData = require("form-data");
var fs = require("fs");

var first_upload_data = fs.createReadStream("/path/to/first_file");

var first_upload_config = {
  method: "post",
  maxBodyLength: Infinity,
  url: "https://api.pdfrest.com/upload",
  headers: {
    "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
    "Content-Filename": "first_filename.pdf",
    "Content-Type": "application/octet-stream",
  },
  data: first_upload_data, // set the data to be sent with the request
};

// send request and handle response or error
axios(first_upload_config)
  .then(function (first_upload_response) {
    var first_uploaded_id = first_upload_response.data.files[0].id;

    var second_upload_data = fs.createReadStream("/path/to/second_file");

    var second_upload_config = {
      method: "post",
      maxBodyLength: Infinity,
      url: "https://api.pdfrest.com/upload",
      headers: {
        "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
        "Content-Filename": "second_filename.pdf",
        "Content-Type": "application/octet-stream",
      },
      data: second_upload_data, // set the data to be sent with the request
    };

    axios(second_upload_config)
      .then(function (second_upload_response) {
        console.log(JSON.stringify(second_upload_response.data));
        var second_uploaded_id = second_upload_response.data.files[0].id;

        var zip_config = {
          method: "post",
          maxBodyLength: Infinity,
          url: "https://api.pdfrest.com/zip",
          headers: {
            "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
            "Content-Type": "application/json",
          },
          data: {
            id: [first_uploaded_id, second_uploaded_id],
          }, // set the data to be sent with the request
        };

        // send request and handle response or error
        axios(zip_config)
          .then(function (zip_response) {
            console.log(JSON.stringify(zip_response.data));
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

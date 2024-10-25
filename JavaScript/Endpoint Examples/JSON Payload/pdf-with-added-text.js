var axios = require("axios");
var FormData = require("form-data");
var fs = require("fs");

var upload_data = fs.createReadStream("/path/to/file");

var upload_config = {
  method: "post",
  maxBodyLength: Infinity,
  url: "https://api.pdfrest.com/upload",
  headers: {
    "Api-Key": "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
    "Content-Filename": "filename.pdf",
    "Content-Type": "application/octet-stream",
  },
  data: upload_data, // set the data to be sent with the request
};

// send request and handle response or error
axios(upload_config)
  .then(function (upload_response) {
    console.log(JSON.stringify(upload_response.data));
    var uploaded_id = upload_response.data.files[0].id;

    var text_option_array = [];
    var text_options = {
        "font":"Times New Roman",
        "max_width":"175",
        "opacity":"1",
        "page":"1",
        "rotation":"0",
        "text":"sample text in PDF",
        "text_color_rgb":"0,0,0",
        "text_size":"30",
        "x":"72",
        "y":"144"
    };
    text_option_array.push(text_options);
    var add_text_config = {
      method: "post",
      maxBodyLength: Infinity,
      url: "https://api.pdfrest.com/pdf-with-added-text",
      headers: {
        "Api-Key": "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
        "Content-Type": "application/json",
      },
      data: {
        id: uploaded_id,
        text_objects: JSON.stringify(text_option_array),
      }, // set the data to be sent with the request
    };

    // send request and handle response or error
    axios(add_text_config)
      .then(function (add_text_response) {
        console.log(JSON.stringify(add_text_response.data));
      })
      .catch(function (error) {
        console.log(error);
      });
  })
  .catch(function (error) {
    console.log(error);
  });

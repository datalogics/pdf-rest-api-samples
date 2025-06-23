var axios = require("axios");
var fs = require("fs");

var upload_data = fs.createReadStream("/path/to/file.pdf");

var upload_config = {
  method: "post",
  maxBodyLength: Infinity,
  url: "https://api.pdfrest.com/upload",
  headers: {
    "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
    "Content-Filename": "filename.pdf",
    "Content-Type": "application/octet-stream",
  },
  data: upload_data,
};

// Send upload request
axios(upload_config)
  .then(function (upload_response) {
    console.log("Upload response:");
    console.log(JSON.stringify(upload_response.data, null, 2));

    var uploaded_id = upload_response.data.files[0].id;

    var markdown_config = {
      method: "post",
      maxBodyLength: Infinity,
      url: "https://api.pdfrest.com/markdown",
      headers: {
        "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
        "Content-Type": "application/json",
      },
      data: {
        id: uploaded_id,
        page_break_comments: "on"
      },
    };

    // Send markdown request
    axios(markdown_config)
      .then(function (markdown_response) {
        console.log("Markdown response:");
        console.log(JSON.stringify(markdown_response.data, null, 2));
      })
      .catch(function (error) {
        console.error("Markdown request error:");
        console.error(error.response?.data || error.message);
      });
  })
  .catch(function (error) {
    console.error("Upload request error:");
    console.error(error.response?.data || error.message);
  });
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

    var boxes_config = {
      method: "post",
      maxBodyLength: Infinity,
      url: "https://api.pdfrest.com/pdf-with-page-boxes-set",
      headers: {
        "Api-Key": "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
        "Content-Type": "application/json",
      },
      data: {
        id: uploaded_id,
        boxes: JSON.stringify(boxOptions),
      }, // set the data to be sent with the request
    };

    // send request and handle response or error
    axios(boxes_config)
      .then(function (boxes_response) {
        console.log(JSON.stringify(boxes_response.data));
      })
      .catch(function (error) {
        console.log(error);
      });
  })
  .catch(function (error) {
    console.log(error);
  });

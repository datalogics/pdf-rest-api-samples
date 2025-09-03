var axios = require("axios");
var FormData = require("form-data");
var fs = require("fs");

var upload_data = fs.createReadStream("/path/to/file");

var upload_config = {
  method: "post",
  maxBodyLength: Infinity,
  url: "https://api.pdfrest.com/upload",
  headers: {
    "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
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

    var unrestrict_config = {
      method: "post",
      maxBodyLength: Infinity,
      url: "https://api.pdfrest.com/unrestricted-pdf",
      headers: {
        "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
        "Content-Type": "application/json",
      },
      data: {
        id: uploaded_id,
        current_permissions_password: "current_password",
      }, // set the data to be sent with the request
    };

    // send request and handle response or error
    axios(unrestrict_config)
      .then(function (unrestrict_response) {
        console.log(JSON.stringify(unrestrict_response.data));

        // All files uploaded or generated are automatically deleted based on the 
        // File Retention Period as shown on https://pdfrest.com/pricing. 
        // For immediate deletion of files, particularly when sensitive data 
        // is involved, an explicit delete call can be made to the API.
        //
        // The following code is an optional step to delete sensitive files
        // (unredacted, unencrypted, unrestricted, or unwatermarked) from pdfRest servers.

        var body = unrestrict_response.data;
        var result_id = body.outputId;
          var delete_config = {
            method: "post",
            maxBodyLength: Infinity,
            url: "https://api.pdfrest.com/delete",
            headers: {
              "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
              "Content-Type": "application/json",
            },
            data: { ids: result_id },
          };

        axios(delete_config)
          .then(function (delete_response) {
            console.log(JSON.stringify(delete_response.data));
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

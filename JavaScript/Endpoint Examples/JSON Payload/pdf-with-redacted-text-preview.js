var axios = require("axios");
var FormData = require("form-data");
var fs = require("fs");

// Toggle deletion of sensitive files (default: false)
const DELETE_SENSITIVE_FILES = false;

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

    var redaction_option_array = [];
    var redaction_options1 = {
        "type": "preset",
        "value": "email",
    };
    var redaction_options2 = {
        "type": "regex",
        "value": "(\\+\\d{1,2}\\s)?\\(?\\d{3}\\)?[\\s.-]\\d{3}[\\s.-]\\d{4}",
    };
    var redaction_options3 = {
        "type": "literal",
        "value": "word",
    };
    redaction_option_array.push(redaction_options1);
    redaction_option_array.push(redaction_options2);
    redaction_option_array.push(redaction_options3);

    var redact_text_config = {
      method: "post",
      maxBodyLength: Infinity,
      url: "https://api.pdfrest.com/pdf-with-redacted-text-preview",
      headers: {
        "Api-Key": "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
        "Content-Type": "application/json",
      },
      data: {
        id: uploaded_id,
        redactions: JSON.stringify(redaction_option_array),
      }, // set the data to be sent with the request
    };

    // send request and handle response or error
    axios(redact_text_config)
      .then(function (redact_text_response) {
        console.log(JSON.stringify(redact_text_response.data));

        // All files uploaded or generated are automatically deleted based on the 
        // File Retention Period as shown on https://pdfrest.com/pricing. 
        // For immediate deletion of files, particularly when sensitive data 
        // is involved, an explicit delete call can be made to the API.
        //
        // The following code is an optional step to delete sensitive files
        // (unredacted, unencrypted, unrestricted, or unwatermarked) from pdfRest servers.
        // IMPORTANT: Do not delete the preview_id (the preview PDF) file until after the redaction is applied
        // with the /pdf-with-redacted-text-applied endpoint.

        var preview_id = redact_text_response.data.outputId;
        var delete_config = {
          method: "post",
          maxBodyLength: Infinity,
          url: "https://api.pdfrest.com/delete",
          headers: {
            "Api-Key": "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
            "Content-Type": "application/json",
          },
          data: { ids: uploaded_id + ', ' + preview_id },
        };

        if (DELETE_SENSITIVE_FILES) {
          axios(delete_config)
            .then(function (delete_response) {
              console.log(JSON.stringify(delete_response.data));
            })
            .catch(function (error) {
              console.log(error);
            });
        }
      })
      .catch(function (error) {
        console.log(error);
      });
  })
  .catch(function (error) {
    console.log(error);
  });

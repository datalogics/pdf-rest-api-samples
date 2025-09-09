var axios = require("axios");
var FormData = require("form-data");
var fs = require("fs");

// By default, we use the US-based API service. This is the primary endpoint for global use.
var apiUrl = "https://api.pdfrest.com";

/* For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
 * For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 */
//var apiUrl "https://eu-api.pdfrest.com";

// Toggle deletion of sensitive files (default: false)
const DELETE_SENSITIVE_FILES = false;

var upload_data = fs.createReadStream("/path/to/file");

var upload_config = {
  method: "post",
  maxBodyLength: Infinity,
  url: apiUrl + "/upload",
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
      url: apiUrl + "/pdf-with-redacted-text-preview",
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
        // Deletes all files in the workflow, including outputs. Save all desired files before enabling this step.
        // IMPORTANT: Do not delete the preview_id (the preview PDF) file until after the redaction is applied
        // with the /pdf-with-redacted-text-applied endpoint.

        var preview_id = redact_text_response.data.outputId;
        var delete_config = {
          method: "post",
          maxBodyLength: Infinity,
          url: apiUrl + "/delete",
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

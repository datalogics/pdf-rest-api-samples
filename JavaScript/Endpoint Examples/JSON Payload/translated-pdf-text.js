var axios = require("axios");
var fs = require("fs");

// By default, we use the US-based API service. This is the primary endpoint for global use.
var apiUrl = "https://api.pdfrest.com";

/* For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
 * For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 */
//var apiUrl = "https://eu-api.pdfrest.com";

var upload_data = fs.createReadStream("/path/to/file");

var upload_config = {
  method: "post",
  maxBodyLength: Infinity,
  url: apiUrl + "/upload",
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

    var translate_config = {
      method: "post",
      maxBodyLength: Infinity,
      url: apiUrl + "/translated-pdf-text",
      headers: {
        "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
        "Content-Type": "application/json",
      },
      data: {
        id: uploaded_id,
        // Translates text to American English. Format the output_language as a 2-3 character ISO 639 code, optionally with a region/script (e.g., 'en', 'es', 'zh-Hant', 'eng-US').
        output_language: "en-US",
      },
    };

    // Send translated-pdf-text request
    axios(translate_config)
      .then(function (translate_response) {
        console.log("Translate response:");
        console.log(JSON.stringify(translate_response.data, null, 2));
      })
      .catch(function (error) {
        console.error("Translate request error:");
        console.error(error.response?.data || error.message);
      });
  })
  .catch(function (error) {
    console.error("Upload request error:");
    console.error(error.response?.data || error.message);
  });


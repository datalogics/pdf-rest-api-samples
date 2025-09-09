var axios = require("axios");
var FormData = require("form-data");
var fs = require("fs");

/*
* This sample demonstrates the workflow from unredacted document to fully
* redacted document. The output file from the preview tool is immediately
* forwarded to the finalization stage. We recommend inspecting the output from
* the preview stage before utilizing this workflow to ensure that content is
* redacted as intended.
*/

// By default, we use the US-based API service. This is the primary endpoint for global use.
var apiUrl = "https://api.pdfrest.com";

/* For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
 * For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 */
//var apiUrl "https://eu-api.pdfrest.com";

var apiKey = "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"; // Replace with your API key

var previewData = new FormData();
previewData.append("file", fs.createReadStream("/path/to/file.pdf"));

var redaction_option_array = [];
var redaction_options = {
    "type": "regex",
    "value": "[Tt]he",
};
redaction_option_array.push(redaction_options);
previewData.append('redactions', JSON.stringify(redaction_option_array));

var previewConfig = {
  method: "post",
  maxBodyLength: Infinity,
  url: apiUrl + "/pdf-with-redacted-text-preview",
  headers: {
    "Api-Key": apiKey,
    ...previewData.getHeaders(),
  },
  data: previewData,
};

axios(previewConfig)
  .then(function (response) {
    var pdfID = response.data.outputId;

    var appliedData = new FormData();
    appliedData.append("id", pdfID);
    appliedData.append("output", "pdfrest_applied_redaction");

    var appliedConfig = {
      method: "post",
      maxBodyLength: Infinity,
      url: apiUrl + "/pdf-with-redacted-text-applied",
      headers: {
        "Api-Key": apiKey,
        ...appliedData.getHeaders(),
      },
      data: appliedData,
    };

    axios(appliedConfig)
      .then(function (response) {
        console.log(JSON.stringify(response.data)); // If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.js' sample.
      })
      .catch(function (error) {
        console.log(error);
      });
  })
  .catch(function (error) {
    console.log(error);
  });

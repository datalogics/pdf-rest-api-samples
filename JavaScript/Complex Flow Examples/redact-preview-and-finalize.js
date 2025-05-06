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
  url: "https://api.pdfrest.com/pdf-with-redacted-text-preview",
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
      url: "https://api.pdfrest.com/pdf-with-redacted-text-applied",
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

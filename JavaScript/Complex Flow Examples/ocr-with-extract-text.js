var axios = require("axios");
var FormData = require("form-data");
var fs = require("fs");

/* In this sample, we will show how to convert a scanned document into a PDF with
* searchable and extractable text using Optical Character Recognition (OCR), and then
* extract that text from the newly created document.
*
* First, we will upload a scanned PDF to the /pdf-with-ocr-text route and capture the
* output ID. Then, we will send the output ID to the /extracted-text route, which will
* return the newly added text.
*/

var apiKey = "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"; // Replace with your API key

var ocrData = new FormData();
ocrData.append("file", fs.createReadStream("/path/to/file.pdf"), "file_name.pdf");
ocrData.append("output", "example_pdf-with-ocr-text_out");

var ocrConfig = {
  method: "post",
  maxBodyLength: Infinity,
  url: "https://api.pdfrest.com/pdf-with-ocr-text",
  headers: {
    "Api-Key": apiKey,
    ...ocrData.getHeaders(),
  },
  data: ocrData,
};

console.log("Sending POST request to OCR endpoint...");
axios(ocrConfig)
  .then(function (response) {
    console.log("Response status code: " + response.status);

    if (response.status === 200) {
      var ocrPDFID = response.data.outputId;
      console.log("Got the output ID: " + ocrPDFID);

      var extractData = new FormData();
      extractData.append("id", ocrPDFID);

      var extractConfig = {
        method: "post",
        maxBodyLength: Infinity,
        url: "https://api.pdfrest.com/extracted-text",
        headers: {
          "Api-Key": apiKey,
          ...extractData.getHeaders(),
        },
        data: extractData,
      };

      console.log("Sending POST request to extract text endpoint...");
      axios(extractConfig)
        .then(function (extractResponse) {
          console.log("Response status code: " + extractResponse.status);

          if (extractResponse.status === 200) {
            console.log(extractResponse.data.fullText);
          } else {
            console.log(extractResponse.data);
          }
        })
        .catch(function (error) {
          console.log(error.response ? error.response.data : error.message);
        });
    } else {
      console.log(response.data);
    }
  })
  .catch(function (error) {
    console.log(error.response ? error.response.data : error.message);
  });
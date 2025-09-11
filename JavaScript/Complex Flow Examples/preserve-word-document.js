var axios = require("axios");
var FormData = require("form-data");
var fs = require("fs");

/* In this sample, we will show how to optimize a Word file for long-term preservation
 * as discussed in https://pdfrest.com/solutions/optimize-word-excel-and-powerpoint-files-for-long-term-preservation/
 * We will take our Word (or Excel or PowerPoint) document and first convert it to
 * a PDF with a call to the /pdf route. Then, we will take that converted PDF
 * and convert it to the PDF/A format for long-term storage.
 */

 // By default, we use the US-based API service. This is the primary endpoint for global use.
 var apiUrl = "https://api.pdfrest.com";

 /* For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
  * For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
  */
 //var apiUrl = "https://eu-api.pdfrest.com";

var apiKey = "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"; // Replace with your API key

var pdfData = new FormData();
pdfData.append("file", fs.createReadStream("/path/to/word.doc"));

var pdfConfig = {
  method: "post",
  maxBodyLength: Infinity,
  url: apiUrl + "/pdf",
  headers: {
    "Api-Key": apiKey,
    ...pdfData.getHeaders(),
  },
  data: pdfData,
};

axios(pdfConfig)
  .then(function (response) {
    var pdfID = response.data.outputId;

    var pdfaData = new FormData();
    pdfaData.append("id", pdfID);
    pdfaData.append("output_type", "PDF/A-3b");
    pdfaData.append("output", "pdfrest_pdfa");

    var pdfaConfig = {
      method: "post",
      maxBodyLength: Infinity,
      url: apiUrl + "/pdfa",
      headers: {
        "Api-Key": apiKey,
        ...pdfaData.getHeaders(),
      },
      data: pdfaData,
    };

    axios(pdfaConfig)
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

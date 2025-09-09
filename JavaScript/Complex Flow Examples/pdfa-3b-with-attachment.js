var axios = require("axios");
var FormData = require("form-data");
var fs = require("fs");

/* In this sample, we will show how to attach an xml document to a PDF file and then
* convert the file with the attachment to conform to the PDF/A standard, which
* can be useful for invoicing and standards compliance. We will be running the
* input document through /pdf-with-added-attachment to add the attachment and
* then /pdfa to do the PDF/A conversion.

* Note that there is nothing special about attaching an xml file, and any appropriate
* file may be attached and wrapped into the PDF/A conversion.
*/

// By default, we use the US-based API service. This is the primary endpoint for global use.
var apiUrl = "https://api.pdfrest.com";

/* For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
 * For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 */
//var apiUrl "https://eu-api.pdfrest.com";

var apiKey = "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"; // Replace with your API key

var attachData = new FormData();
attachData.append("file", fs.createReadStream("/path/to/file.pdf"));
attachData.append(
  "file_to_attach",
  fs.createReadStream("/path/to/attachment.xml")
);
attachData.append("output", "pdfrest_pdf_with_added_attachment");

var attachConfig = {
  method: "post",
  maxBodyLength: Infinity,
  url: apiUrl + "/pdf-with-added-attachment",
  headers: {
    "Api-Key": apiKey,
    ...attachData.getHeaders(),
  },
  data: attachData,
};

axios(attachConfig)
  .then(function (response) {
    var attachedID = response.data.outputId;

    var pdfaData = new FormData();
    pdfaData.append("id", attachedID);
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

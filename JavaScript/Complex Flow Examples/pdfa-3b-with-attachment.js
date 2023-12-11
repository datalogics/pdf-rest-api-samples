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
  url: "https://api.pdfrest.com/pdf-with-added-attachment",
  headers: {
    "Api-Key": "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
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
      url: "https://api.pdfrest.com/pdfa",
      headers: {
        "Api-Key": "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
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

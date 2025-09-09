var axios = require("axios");
var FormData = require("form-data");
var fs = require("fs");

/* In this sample, we will show how to watermark a PDF document and then restrict
 * editing on the document so that the watermark cannot be removed, as discussed in
 * https://pdfrest.com/solutions/add-pdf-watermarks-that-cannot-be-removed/.
 * We will be running the input file through /watermarked-pdf to apply the watermark
 * and then /restricted-pdf to lock the watermark in.
 */

 // By default, we use the US-based API service. This is the primary endpoint for global use.
 var apiUrl = "https://api.pdfrest.com";

 /* For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
  * For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
  */
 //var apiUrl "https://eu-api.pdfrest.com";

var apiKey = "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"; // Replace with your API key

var watermarkData = new FormData();
watermarkData.append("file", fs.createReadStream("/path/to/file.pdf"));
watermarkData.append("watermark_text", "Watermarked");

var watermarkConfig = {
  method: "post",
  maxBodyLength: Infinity,
  url: apiUrl + "/watermarked-pdf",
  headers: {
    "Api-Key": apiKey,
    ...watermarkData.getHeaders(),
  },
  data: watermarkData,
};

axios(watermarkConfig)
  .then(function (response) {
    var data = new FormData();
    data.append("id", response.data.outputId);
    data.append("new_permissions_password", "new_example_pw");

    data.append("restrictions[]", "edit_annotations");
    data.append("restrictions[]", "edit_content");
    data.append("restrictions[]", "copy_content");

    data.append("output", "pdfrest_restricted_pdf");
    var restrictConfig = {
      method: "post",
      maxBodyLength: Infinity,
      url: apiUrl + "/restricted-pdf",
      headers: {
        "Api-Key": apiKey,
        ...data.getHeaders(),
      },
      data: data,
    };

    axios(restrictConfig)
      .then(function (response) {
        console.log(JSON.stringify(response.data)); // If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.js' sample
      })
      .catch(function (error) {
        console.log(error);
      });
  })
  .catch(function (error) {
    console.log(error);
  });

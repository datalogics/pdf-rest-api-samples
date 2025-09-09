var axios = require("axios");
var FormData = require("form-data");
var fs = require("fs");

/* In this sample, we will show how to merge different file types together as
* discussed in https://pdfrest.com/solutions/merge-multiple-types-of-files-together/.
First, we will upload an image file to the /pdf route and capture the output ID.
* Next, we will upload a PowerPoint file to the /pdf route and capture its output
* ID. Finally, we will pass both IDs to the /merged-pdf route to combine both inputs
* into a single PDF.
*
* Note that there is nothing special about an image and a PowerPoint file, and
* this sample could be easily used to convert and combine any two file types
* that the /pdf route takes as inputs.
*/

// By default, we use the US-based API service. This is the primary endpoint for global use.
var apiUrl = "https://api.pdfrest.com";

/* For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
 * For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 */
//var apiUrl "https://eu-api.pdfrest.com";

var apiKey = "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"; // Replace with your API key

var imageData = new FormData();
imageData.append("file", fs.createReadStream("/path/to/image.png"));

var imageConfig = {
  method: "post",
  maxBodyLength: Infinity,
  url: apiUrl + "/pdf",
  headers: {
    "Api-Key": apiKey,
    ...imageData.getHeaders(),
  },
  data: imageData,
};

axios(imageConfig)
  .then(function (response) {
    var imagePDFID = response.data.outputId;

    var pptData = new FormData();
    pptData.append("file", fs.createReadStream("/path/to/powerpoint.ppt"));

    var pptConfig = {
      method: "post",
      maxBodyLength: Infinity,
      url: apiUrl + "/pdf",
      headers: {
        "Api-Key": apiKey,
        ...pptData.getHeaders(),
      },
      data: pptData,
    };

    axios(pptConfig)
      .then(function (response) {
        var pptPDFID = response.data.outputId;

        var mergeData = new FormData();
        mergeData.append("id", imagePDFID);
        mergeData.append("pages[]", "1-last");
        mergeData.append("type[]", "id");
        mergeData.append("id", pptPDFID);
        mergeData.append("pages[]", "1-last");
        mergeData.append("type[]", "id");
        mergeData.append("output", "pdfrest_merged_pdf");

        var mergeConfig = {
          method: "post",
          maxBodyLength: Infinity,
          url: apiUrl + "/merged-pdf",
          headers: {
            "Api-Key": apiKey,
            ...mergeData.getHeaders(),
          },
          data: mergeData,
        };

        axios(mergeConfig)
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
  })
  .catch(function (error) {
    console.log(error);
  });

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

var imageData = new FormData();
imageData.append("file", fs.createReadStream("/path/to/image.png"));

var imageConfig = {
  method: "post",
  maxBodyLength: Infinity,
  url: "https://api.pdfrest.com/pdf",
  headers: {
    "Api-Key": "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
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
      url: "https://api.pdfrest.com/pdf",
      headers: {
        "Api-Key": "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
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
          url: "https://api.pdfrest.com/merged-pdf",
          headers: {
            "Api-Key": "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
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

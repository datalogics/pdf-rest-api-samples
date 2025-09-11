var axios = require("axios");
var FormData = require("form-data");
var fs = require("fs");

/* In this sample, we will show how to take an encrypted file and decrypt, modify
 * and re-encrypt it to create an encryption-at-rest solution as discussed in
 * https://pdfrest.com/solutions/create-secure-document-workflows-with-pdf-password-protection/
 * We will be running the document through /decrypted-pdf to open the document
 * to modification, running the decrypted result through /pdf-with-added-image,
 * and then sending the output with the new image through /encrypted-pdf to
 * lock it up again.
 */

// By default, we use the US-based API service. This is the primary endpoint for global use.
var apiUrl = "https://api.pdfrest.com";

/* For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
 * For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 */
//var apiUrl = "https://eu-api.pdfrest.com";

var apiKey = "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"; // Replace with your API key

var decryptRequestData = new FormData();
decryptRequestData.append("file", fs.createReadStream("/path/to/file.pdf"));
decryptRequestData.append("current_open_password", "current_example_pw");

var decryptConfig = {
  method: "post",
  maxBodyLength: Infinity,
  url: apiUrl + "/decrypted-pdf",
  headers: {
    "Api-Key": apiKey,
    ...decryptRequestData.getHeaders(),
  },
  data: decryptRequestData,
};

axios(decryptConfig)
  .then(function (response) {
    var data = new FormData();
    data.append("id", response.data.outputId);
    data.append("image_file", fs.createReadStream("/path/to/image.png"));
    data.append("x", "0");
    data.append("y", "0");
    data.append("page", "1");
    data.append("output", "pdfrest_pdf_with_added_image");

    var addImageConfig = {
      method: "post",
      maxBodyLength: Infinity,
      url: apiUrl + "/pdf-with-added-image",
      headers: {
        "Api-Key": apiKey,
        ...data.getHeaders(),
      },
      data: data,
    };

    axios(addImageConfig)
      .then(function (response) {
        var data = new FormData();
        data.append("id", response.data.outputId);
        data.append("new_open_password", "new_example_pw");
        data.append("output", "pdfrest_encrypted_pdf");
        var encryptConfig = {
          method: "post",
          maxBodyLength: Infinity,
          url: apiUrl + "/encrypted-pdf",
          headers: {
            "Api-Key": apiKey,
            ...data.getHeaders(),
          },
          data: data,
        };

        axios(encryptConfig)
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

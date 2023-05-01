/**
The /decrypted-pdf endpoint can take a single PDF file or id as input.

This sample demonstrates removing security restrictions from a PDF.

Import fetch
 */
// Importing the required modules
import https from "follow-redirects/https.js";
import fs from "fs";

// Setting the options for the HTTPS POST request
var options = {
  method: "POST",
  hostname: "api.pdfrest.com",
  path: "/restricted-pdf",
  headers: {
    "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Place your API key here
  },
  maxRedirects: 20,
};

// Sending the HTTPS POST request
var req = https.request(options, function (res) {
  var chunks = [];

  // Receiving data from the response
  res.on("data", function (chunk) {
    chunks.push(chunk);
  });

  // Handling the end of the response
  res.on("end", function (chunk) {
    var body = Buffer.concat(chunks);
    console.log(body.toString());
  });

  // Handling errors in the response
  res.on("error", function (error) {
    console.error(error);
  });
});

// Preparing the data to send in the request
var postData =
  '------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name="file"; filename="file"\r\nContent-Type: "{Insert_File_Content_Type}"\r\n\r\n' +
  fs.readFileSync("/path/to/file") +
  '\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name="output"\r\n\r\nunrestrict_out\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name="current_permissions_password"\r\n\r\npassword\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--';

// Setting the request header
req.setHeader(
  "content-type",
  "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW"
);

// Writing the data to the request
req.write(postData);

// Ending the request
req.end();

// If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.js' sample.

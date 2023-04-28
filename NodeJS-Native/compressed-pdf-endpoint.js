/**
The /compressed-pdf endpoint can take a single PDF file or id as input.

This sample demonstrates setting compression_level to 'medium'.

We have preset 'high', 'medium', and 'low' compression levels available for use. These preset levels do not require the 'profile' parameter.
 */
// Importing the required modules
var https = require("follow-redirects").https;
var fs = require("fs");

// Set the request options
var options = {
  method: "POST",
  hostname: "api.pdfrest.com",
  path: "/compressed-pdf",
  headers: {
    "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Place your API key here
  },
  maxRedirects: 20,
};

// Create a request
var req = https.request(options, function (res) {
  // Create an array to store the response chunks
  var chunks = [];

  // Add data to the response array when the 'data' event is emitted
  res.on("data", function (chunk) {
    chunks.push(chunk);
  });

  // Concatenate the response chunks and log the resulting compressed file when the 'end' event is emitted
  res.on("end", function (chunk) {
    var body = Buffer.concat(chunks);
    console.log(body.toString());
  });

  // Log any errors that occur when the 'error' event is emitted
  res.on("error", function (error) {
    console.error(error);
  });
});

// Create the form data to send in the request
var postData =
  '------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name="file"; filename="file"\r\nContent-Type: "{Insert_File_Content_Type}"\r\n\r\n' +
  fs.readFileSync("/path/to/file") +
  '\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name="output"\r\n\r\ncompressed_out\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name="compression_level"\r\n\r\nmedium\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--';

// Set the 'content-type' header and write the form data to the request
req.setHeader(
  "content-type",
  "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW"
);
req.write(postData);

// Send the request
req.end();

// If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.js' sample.

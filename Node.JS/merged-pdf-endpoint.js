/**
The /merged-pdf endpoint can take one or more PDF files or ids as input.

This sample takes 2 PDF files and merges all the pages in the document into a single document.
 */
// Importing the required modules
import https from "follow-redirects/https.js";
import fs from "fs";

// Defining the request options
var options = {
  method: "POST",
  hostname: "api.pdfrest.com",
  path: "/merged-pdf",
  headers: {
    // Adding the API key to the headers
    "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Place your API key here
  },
  maxRedirects: 20,
};

// Making the request to the PDFRest API
var req = https.request(options, function (res) {
  var chunks = [];

  // Collecting the response data in chunks
  res.on("data", function (chunk) {
    chunks.push(chunk);
  });

  // Parsing and printing the response data on 'end' event
  res.on("end", function (chunk) {
    var body = Buffer.concat(chunks);
    console.log(body.toString());
  });

  // Handling any errors that occur during the request
  res.on("error", function (error) {
    console.error(error);
  });
});

// Creating the form data to be sent in the request body
var postData =
  '------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name="file"; filename="file"\r\nContent-Type: "{Insert_File_Content_Type}"\r\n\r\n' +
  fs.readFileSync("/path/to/file") +
  '\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name="pages[]"\r\n\r\n1-last\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name="type[]"\r\n\r\nfile\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name="file"; filename="file"\r\nContent-Type: "{Insert_File_Content_Type}"\r\n\r\n' +
  fs.readFileSync("/path/to/file") +
  '\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name="pages[]"\r\n\r\n1-last\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name="type[]"\r\n\r\nfile\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name="output"\r\n\r\nmerged_out\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--';

// Setting the content type in the request header
req.setHeader(
  "content-type",
  "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW"
);

// Writing the form data to the request body
req.write(postData);

// Ending the request
req.end();

// If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.js' sample.

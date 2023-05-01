/**
The /pdf-info endpoint can take a single PDF file or id as input.

This sample demonstrates querying the title, page count, document language and author

 */
// Importing the required modules
import https from "follow-redirects/https.js";
import fs from "fs";

// Setting the options for the HTTPS POST request
var options = {
  method: "POST",
  hostname: "api.pdfrest.com",
  path: "/pdf-info",
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
  '\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name="queries"\r\n\r\ntitle,page_count,doc_language,tagged,image_only,author,creation_date,modified_date,producer\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--';

// Setting the request header
req.setHeader(
  "content-type",
  "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW"
);

// Writing the data to the request
req.write(postData);

// Ending the request
req.end();

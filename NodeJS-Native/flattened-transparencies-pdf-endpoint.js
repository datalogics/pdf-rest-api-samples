/**
The /flattened-transparencies-pdf endpoint can take a single PDF file or id as input.

This sample demonstrates setting quality to 'medium'.

We have preset 'high', 'medium', and 'low' quality levels available for use. These preset levels do not require the 'profile' parameter.
 */
// Importing the required modules
var https = require("follow-redirects").https;
var fs = require("fs");

// Set the API endpoint options
var options = {
  method: "POST",
  hostname: "api.pdfrest.com",
  path: "/flattened-transparencies-pdf",
  headers: {
    "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Place your API key here
  },
  maxRedirects: 20,
};

// Send the API request
var req = https.request(options, function (res) {
  var chunks = [];

  res.on("data", function (chunk) {
    chunks.push(chunk);
  });

  res.on("end", function (chunk) {
    var body = Buffer.concat(chunks);
    console.log(body.toString());
  });

  res.on("error", function (error) {
    console.error(error);
  });
});

// Set the data to send in the request
var postData =
  '------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name="file"; filename="file"\r\nContent-Type: "{Insert_File_Content_Type}"\r\n\r\n' +
  fs.readFileSync("/path/to/file") +
  '\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name="output"\r\n\r\nflattened_transparencies_out\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name="quality"\r\n\r\nmedium\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--';

// Set the request headers
req.setHeader(
  "content-type",
  "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW"
);

// Send the data in the request
req.write(postData);

// End the request
req.end();

// If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.js' sample.

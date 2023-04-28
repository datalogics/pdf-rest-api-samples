/**
 * The /pdf endpoint can take a single file, id, or url as input.
 * This sample passes a jpeg file to the endpoint, but there's a variety of input file types that are accepted by this endpoint.
 */

// Import the necessary modules
const https = require("follow-redirects").https;
const fs = require("fs");

// Set the options for the HTTPS request
const options = {
  method: "POST",
  hostname: "api.pdfrest.com",
  path: "/pdf",
  headers: {
    "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
  },
  maxRedirects: 20,
};

// Make the HTTPS request
const req = https.request(options, function (res) {
  // Collect the response data in chunks
  const chunks = [];

  res.on("data", function (chunk) {
    chunks.push(chunk);
  });

  // When the response is complete, assemble the chunks into a buffer and log it
  res.on("end", function (chunk) {
    const body = Buffer.concat(chunks);
    console.log(body.toString());
  });

  // If there's an error, log it
  res.on("error", function (error) {
    console.error(error);
  });
});

// Set up the request body
const postData =
  '------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name="file"; filename="file"\r\nContent-Type: "application/pdf"\r\n\r\n' +
  fs.readFileSync("path/to/file") +
  '\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name="output"\r\n\r\npdf_out\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--';

// Set the content type and write the request body
req.setHeader(
  "content-type",
  "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW"
);

req.write(postData);

// End the request
req.end();

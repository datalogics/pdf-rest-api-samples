/**
The /html endpoint can take a string of HTML content and convert it to a HTML (.html) file.

This sample takes in a string of HTML content that displays "Hello World!" and turns it into a HTML file.

Import fetch
 */
import fetch, { FormData } from "node-fetch";

let htmlContent =
  "<html><head><title>Web Page</title></head><body>Hello World!</body></html>";

let formdata = new FormData();
formdata.append("content", htmlContent);
formdata.append("output", "example_html_out");

let requestOptions = {
  method: "POST",
  headers: {
    "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Place your api key here
  },
  body: formdata,
  redirect: "follow",
};

fetch("https://cloud-api.datalogics.com/html", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));

// If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.js' sample.

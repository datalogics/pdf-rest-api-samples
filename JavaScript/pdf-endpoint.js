/**
The /pdf endpoint can take a single file, id, or url as input. 

This sample passes a jpeg file to the endpoint, but there's a variety of input file types that are accepted by this endpoint.
Import fetch
*/
import fetch, { FormData, fileFromSync } from "node-fetch";

// Append formdata here
let formdata = new FormData();
formdata.append("file", fileFromSync("../Sample_Input/rainbow.tif"));
formdata.append("output", "example_pdf_out");

let requestOptions = {
  method: "POST",
  headers: {
    "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  },
  body: formdata,
  redirect: "follow",
};

// Define URL and submit request
fetch("https://api.pdfrest.com/pdf", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));

// If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.js' sample.

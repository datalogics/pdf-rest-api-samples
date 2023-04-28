/**
The /flattened-transparencies-pdf endpoint can take a single PDF file or id as input.

This sample demonstrates setting quality to 'medium'.

We have preset 'high', 'medium', and 'low' quality levels available for use. These preset levels do not require the 'profile' parameter.

Import fetch
 */
import fetch, { FormData, fileFromSync } from "node-fetch";

// Append formdata here
let formdata = new FormData();
formdata.append("file", fileFromSync("../Sample_Input/toFlatten.pdf"));
formdata.append("quality", "medium");
formdata.append("output", "example_flattenedPdf_out");

let requestOptions = {
  method: "POST",
  headers: {
    "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Place your api key here
  },
  body: formdata,
  redirect: "follow",
};

// Define URL and submit request
fetch("https://api.pdfrest.com/flattened-transparencies-pdf", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));

// If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.js' sample.

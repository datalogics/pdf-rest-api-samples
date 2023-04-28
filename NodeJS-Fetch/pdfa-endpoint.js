/**
The /pdfa endpoint can take a single PDF file or id as input.

Import fetch
*/
import fetch, { FormData, fileFromSync } from "node-fetch";

// Append formdata here
let formdata = new FormData();
formdata.append("file", fileFromSync("../Sample_Input/ducky.pdf"));
formdata.append("output_type", "PDF/A-2b");
formdata.append("output", "example_pdfa_out");

let requestOptions = {
  method: "POST",
  headers: {
    "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Place your api key here
  },
  body: formdata,
  redirect: "follow",
};

// Define URL and submit request
fetch("https://api.pdfrest.com/pdfa", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));

// If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.js' sample.

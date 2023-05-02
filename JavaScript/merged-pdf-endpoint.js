/**
The /merged-pdf endpoint can take one or more PDF files or ids as input.

This sample takes 2 PDF files and merges all the pages in the document into a single document.

Import fetch
 */
import fetch, { FormData, fileFromSync } from "node-fetch";

// Append formdata here
let formdata = new FormData();
formdata.append("file", fileFromSync("../Sample_Input/merge1.pdf"));
formdata.append("pages[]", "1-last");
formdata.append("type[]", "file");
formdata.append("file", fileFromSync("../Sample_Input/merge2.pdf"));
formdata.append("pages[]", "1-last");
formdata.append("type[]", "file");
formdata.append("output", "example_mergedPdf_out");

let requestOptions = {
  method: "POST",
  headers: {
    "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Place your api key here
  },
  body: formdata,
  redirect: "follow",
};

// Define URL and submit request
fetch("https://api.pdfrest.com/merged-pdf", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));

// If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.js' sample.

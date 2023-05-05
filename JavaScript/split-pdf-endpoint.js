/**
The /split-pdf endpoint can take one PDF file or id as input.

This sample takes one PDF file that has at least 5 pages and splits it into two documents when given two page ranges.

Import fetch 
*/
import fetch, { FormData, fileFromSync } from 'node-fetch';


// Append formdata here
let formdata = new FormData();
formdata.append("file", fileFromSync("../Sample_Input/PDFToBeSplit.pdf"));
formdata.append("pages[]", "1,2,5");
formdata.append("pages[]", "3,4");
formdata.append("output", "example_splitPdf_out");

let requestOptions = {
  method: "POST",
  headers: {
    "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Place your api key here
  },
  body: formdata,
  redirect: "follow",
};

// Define URL and submit request
fetch("https://api.pdfrest.com/split-pdf", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));

  // If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.js' sample.

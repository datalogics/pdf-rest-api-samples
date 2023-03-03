/**
The /pdf-info endpoint can take a single PDF file or id as input.

This sample demonstrates querying the title, page count, document language and author

Import fetch
 */
import fetch, { FormData, fileFromSync } from "node-fetch";

// Append formdata here
let formdata = new FormData();
formdata.append("file", fileFromSync("../Sample_Input/pdfRest.pdf"));
formdata.append("queries", "title,page_count,doc_language,author");

let requestOptions = {
  method: "POST",
  headers: {
    "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Place your api key here
  },
  body: formdata,
  redirect: "follow",
};

// Define URL and submit request
fetch("https://api.pdfrest.com/pdf-info", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));

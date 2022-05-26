/**
The /tif endpoint can take a single PDF file or id as input and turn them into TIF image files. 
 
This sample takes in a PDF and converts all pages into grayscale TIF files. 
 */

// Import fetch
import fetch, { FormData, fileFromSync } from "node-fetch";

// // Append formdata here
let formdata = new FormData();
formdata.append("file", fileFromSync("../Sample_Input/ducky.pdf"));
formdata.append("pages", "1-last");
formdata.append("resolution", "600");
formdata.append("color_model", "gray");
formdata.append("output", "example_tif_out");

let requestOptions = {
  method: "POST",
  headers: {
    "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Place your api key here
  },
  body: formdata,
  redirect: "follow",
};

// Define URL and submit request
fetch("https://cloud-api.datalogics.com/tif", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
  
// If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.js' sample.

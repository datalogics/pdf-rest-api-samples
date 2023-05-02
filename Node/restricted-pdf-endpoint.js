/**
The /restricted-pdf endpoint can take a single PDF file or id as input.

This sample demonstrates setting the permissions password to 'password' and adding restrictions.

Import fetch
 */
import fetch, { FormData, fileFromSync } from "node-fetch";

// Append formdata here
let formdata = new FormData();
formdata.append("file", fileFromSync("../Sample_Input/toRestrict.pdf"));
formdata.append("new_permissions_password", "password");
formdata.append("restrictions[]", "print_low");
formdata.append("restrictions[]", "print_high");
formdata.append("restrictions[]", "edit_content");
formdata.append("output", "example_restrictedPdf_out");

let requestOptions = {
  method: "POST",
  headers: {
    "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Place your api key here
  },
  body: formdata,
  redirect: "follow",
};

// Define URL and submit request
fetch("https://api.pdfrest.com/restricted-pdf", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));

// If you would like to download the file instead of getting the JSON response, please see the 'get-resource-id-endpoint.js' sample.

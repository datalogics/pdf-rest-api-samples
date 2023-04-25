import fetch, { FormData, fileFromSync } from "node-fetch";

let formdata = new FormData();
formdata.append("file", fileFromSync("../Sample_Input/ducky.pdf"));
formdata.append("watermark_text", "watermark");
formdata.append("output", "example_out");

let requestOptions = {
  method: "POST",
  headers: {
    "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Place your api key here
  },
  body: formdata,
  redirect: "follow",
};

fetch("https://api.pdfrest.com/watermarked-pdf", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));

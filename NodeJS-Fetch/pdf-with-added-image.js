import fetch, { FormData, fileFromSync } from "node-fetch";

let formdata = new FormData();
formdata.append("file", fileFromSync("../Sample_Input/ducky.pdf"));
formdata.append("image_file", fileFromSync("../Sample_Input/strawberries.jpg"));
formdata.append("output", "example_out");
formdata.append("x", "10");
formdata.append("y", "10");
formdata.append("page", "1");

let requestOptions = {
  method: "POST",
  headers: {
    "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Place your api key here
  },
  body: formdata,
  redirect: "follow",
};

fetch("https://api.pdfrest.com/pdf-with-added-image", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.log("error", error));
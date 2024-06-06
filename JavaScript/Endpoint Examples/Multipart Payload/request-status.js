const axios = require("axios");
const FormData = require("form-data");
const fs = require("fs");


const apiKey = "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"; // Replace with your API key
const pathToFile = "/path/to/file.pdf";

let bmpRequestData = new FormData();
bmpRequestData.append("file", fs.createReadStream(pathToFile));

let bmpConfig = {
  method: "post",
  maxBodyLength: Infinity,
  url: "https://api.pdfrest.com/bmp",
  headers: {
    "Api-Key": apiKey,
    "Response-Type": "requestId", // Use this header to get a request ID.
    ...bmpRequestData.getHeaders(),
  },
  data: bmpRequestData,
};

axios(bmpConfig)
  .then(bmpResponse => {
    console.log(JSON.stringify(bmpResponse.data));
    const requestId = bmpResponse.data.requestId;
    let config = {
      method: "get",
      maxBodyLength: Infinity, // set maximum length of the request body
      url: `https://api.pdfrest.com/request-status/${requestId}`,
      headers: { "Api-Key": apiKey }
    };
    axios.request(config)
      .then((requestStatusResponse) => {
        console.log(JSON.stringify(requestStatusResponse.data));
      })
      .catch((error) => {
        console.log(error);
    });
  }).catch(error => {
    console.log(error);
  })

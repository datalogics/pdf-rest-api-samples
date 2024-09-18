// This request demonstrates how to check the status of the API servers
const axios = require("axios");

let config = {
  method: "get",
  maxBodyLength: Infinity, // set maximum length of the request body
  url: "https://api.pdfrest.com/up-toolkit", // up-forms and up-office can be used to query the other tools
  headers: {},
};

// define configuration options for axios request
axios
  .request(config)
  .then((response) => {
    console.log(JSON.stringify(response.data));
  })
  .catch((error) => {
    console.log(error);
  });

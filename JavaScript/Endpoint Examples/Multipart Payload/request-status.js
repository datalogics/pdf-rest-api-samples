const axios = require("axios");

let config = {
  method: "get",
  maxBodyLength: Infinity, // set maximum length of the request body
  url: "https://api.pdfrest.com/request-status/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  headers: { "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" }
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

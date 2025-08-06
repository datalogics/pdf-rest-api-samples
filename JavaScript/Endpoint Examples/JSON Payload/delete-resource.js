const axios = require("axios");

let config = {
  method: "delete",
  maxBodyLength: Infinity,
  url: "https://api.pdfrest.com/resource/xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  headers: {
    "api-key": "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  },
};

axios
  .request(config)
  .then((response) => {
    console.log(JSON.stringify(response.data));
  })
  .catch((error) => {
    console.log(error);
  });

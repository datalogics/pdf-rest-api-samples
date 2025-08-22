const axios = require("axios");

let config = {
  method: "post",
  maxBodyLength: Infinity,
  url: "https://api.pdfrest.com/delete",
  headers: {
    "api-key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
    "Content-Type": "application/json",
  },
  data: {
    ids:
      "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx, xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
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

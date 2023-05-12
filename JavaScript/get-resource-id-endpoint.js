// This request demonstrates how to retrieve a resource using an ID.
const axios = require('axios');

let config = {
  method: 'get',
  maxBodyLength: Infinity, // set maximum length of the request body
  url: 'https://api.pdfrest.com/resource/xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx?format=url', // Replace with your API key
  headers: { }
};

// define configuration options for axios request
axios.request(config)
.then((response) => {
  console.log(JSON.stringify(response.data));
})
.catch((error) => {
  console.log(error);
});

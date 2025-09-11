// This request demonstrates how to retrieve a resource using an ID.
const axios = require('axios');

// By default, we use the US-based API service. This is the primary endpoint for global use.
var apiUrl = "https://api.pdfrest.com";

/* For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
 * For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 */
//var apiUrl = "https://eu-api.pdfrest.com";

let config = {
  method: 'get',
  maxBodyLength: Infinity, // set maximum length of the request body
  url: apiUrl + '/resource/xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx?format=url',
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

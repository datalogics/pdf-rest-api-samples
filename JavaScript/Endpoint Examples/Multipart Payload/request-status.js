const axios = require("axios");
const FormData = require("form-data");
const fs = require("fs");

// By default, we use the US-based API service. This is the primary endpoint for global use.
var apiUrl = "https://api.pdfrest.com";

/* For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
 * For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 */
//var apiUrl "https://eu-api.pdfrest.com";

const apiKey = "xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"; // Replace with your API key
const pathToFile = "/path/to/file.pdf";


const sleep = (ms) => {
  return new Promise(resolve => setTimeout(resolve, ms));
}

const sendBmpReq = async (config) => {
  return await axios(config);
};

const pollUntilFulfilled = async (requestId) => {
  const pollConfig = {
    method: "get",
    maxBodyLength: Infinity, // set maximum length of the request body
    url: `${apiUrl}/request-status/${requestId}`,
    headers: { "Api-Key": apiKey }
  };
  requestStatusResponse = await axios(pollConfig);
  let status = requestStatusResponse.data.status;

  // This example will repeat the GET request until the BMP request is completed.
  while (status === "pending") {
    console.log(JSON.stringify(requestStatusResponse.data));
    await sleep(5000);
    requestStatusResponse = await axios(pollConfig);
    status = requestStatusResponse.data.status;
  }
  console.log(JSON.stringify(requestStatusResponse.data));
};

const demoApiPolling = async () => {
  try {
    // Send a request with the Response-Type header (using /bmp as an arbitrary example)
    const bmpRequestData = new FormData();
    bmpRequestData.append("file", fs.createReadStream(pathToFile));

    const bmpConfig = {
      method: "post",
      maxBodyLength: Infinity,
      url: apiUrl + "/bmp",
      headers: {
        "Api-Key": apiKey,
        "Response-Type": "requestId", // Use this header to get a request ID.
        ...bmpRequestData.getHeaders(),
      },
      data: bmpRequestData,
    };
    bmpResponse = await sendBmpReq(bmpConfig);
    console.log(JSON.stringify(bmpResponse.data));

    // Get the request ID from the initial response.
    const requestId = bmpResponse.data.requestId;
    await pollUntilFulfilled(requestId);
  } catch (err) {
    console.error(err);
  }
};

demoApiPolling();

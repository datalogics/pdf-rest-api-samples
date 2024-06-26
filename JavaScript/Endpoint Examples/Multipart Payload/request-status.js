const axios = require("axios");
const FormData = require("form-data");
const fs = require("fs");

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
    url: `https://api.pdfrest.com/request-status/${requestId}`,
    headers: { "Api-Key": apiKey }
  };
  requestStatusResponse = await axios(pollConfig);
  let status = requestStatusResponse.data.status;
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
    const bmpRequestData = new FormData();
    bmpRequestData.append("file", fs.createReadStream(pathToFile));
  
    const bmpConfig = {
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
    bmpResponse = await sendBmpReq(bmpConfig);
    console.log(JSON.stringify(bmpResponse.data));
    const requestId = bmpResponse.data.requestId;
    await pollUntilFulfilled(requestId);
  } catch (err) {
    console.error(err);
  }
};

demoApiPolling();

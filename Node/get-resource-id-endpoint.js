import fetch from 'node-fetch';
import fs from 'fs';

// The response format can be 'file' or 'url'. 
// If 'url', then JSON containing the url of the resource file is returned.
// If 'file', then the file itself is returned.
let format = 'file';

// Resource UUIDs can be found in the JSON response of POST requests as "outputId". Resource UUIDs usually look like this: '0950b9bdf-0465-4d3f-8ea3-d2894f1ae839'.
let id = 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'; // place resource uuid here

let url = `https://cloud-api.datalogics.com/resource/${id}?format=${format}`;
let outputFileName = 'place_output_name_with_extension_here';

fetch(url)
.then(response => {
    // If there is a JSON response, print to the console.
    // Otherwise, save a file response.
    if (format === "file") {
        if (response.headers.get('content-type')?.includes('application/json')) {
            response.text().then(resText => console.log(resText));
        } else {
            response.arrayBuffer().then(resArrBuf => fs.writeFileSync(outputFileName, Buffer.from(resArrBuf)));
        }
    } else {
        response.text().then(resText => console.log(resText));
    }
})
.catch(error => console.log("error", error));

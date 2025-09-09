import axios from "axios";
import fs from "fs";

// By default, we use the US-based API service. This is the primary endpoint for global use.
var apiUrl = "https://api.pdfrest.com";

/* For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
 * For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 */
//var apiUrl "https://eu-api.pdfrest.com";

async function uploadFile(filePath, fileName) {
    var upload_data = fs.createReadStream(filePath);
    var upload_config = {
        method: "post",
        maxBodyLength: Infinity,
        url: apiUrl + "/upload",
        headers: {
            "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
            "Content-Filename": fileName,
            "Content-Type": "application/octet-stream",
        },
        data: upload_data, // set the data to be sent with the request
    };
    try {
        const upload_response = await axios(upload_config);
        console.log(JSON.stringify(upload_response.data));
        return upload_response.data.files[0].id;
    } catch (error) {
        console.log(error);
    }
}

function signPdf(input_id, credential_id, passphrase_id, logo_id) {
    const signature_config = {
        type: "new",
        name: "esignature",
        logo_opacity: "0.5",
        location: {
            bottom_left: { x: "0", y: "0" },
            top_right: { x: "216", y: "72" },
            page: 1
        },
        display: {
            include_distinguished_name: "true",
            include_datetime: "true",
            contact: "My contact information",
            location: "My signing location",
            name: "John Doe",
            reason: "My reason for signing"
        }
    };

    var signed_pdf_config = {
        method: "post",
        maxBodyLength: Infinity,
        url: apiUrl + "/signed-pdf",
        headers: {
            "Api-Key": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", // Replace with your API key
            "Content-Type": "application/json",
        },
        data: {
            id: input_id,
            pfx_credential_id: credential_id,
            pfx_passphrase_id: passphrase_id,
            logo_id: logo_id,
            signature_configuration: JSON.stringify(signature_config),
        }, // set the data to be sent with the request
    };

    // send request and handle response or error
    axios(signed_pdf_config)
        .then(function (signed_pdf_response) {
            console.log(JSON.stringify(signed_pdf_response.data));
        })
        .catch(function (error) {
            console.log(error);
        });
}

const input_id = await uploadFile("/path/to/input.pdf", "input.pdf");
const credential_id = await uploadFile("/path/to/credentials.pfx", "credentials.pfx");
const passphrase_id = await uploadFile("/path/to/passphrase.txt", "passphrase.txt");
const logo_id = await uploadFile("/path/to/logo.png", "logo.png");
signPdf(input_id, credential_id, passphrase_id, logo_id);

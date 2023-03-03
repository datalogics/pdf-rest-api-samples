<?php
require("../Sample_Input/sample_input.php");

// The /zip endpoint can take one or more file or ids as input and compresses them into a .zip.
// This sample takes 2 files and compresses them into a zip file.

$zip_endpoint_url = 'https://api.pdfrest.com/zip';

// Create an array that contains that data that will be passed to the POST request.
$data = array(
    'file' => array(
        SAMPLE_INPUT_DIR . 'pdfRest.pdf',
        SAMPLE_INPUT_DIR . 'Datalogics.png',
        SAMPLE_INPUT_DIR . 'pdfRestApiLab.png'
    ),
    'output' => 'example_zip_out'
);

$headers = array(
    'Accept: application/json',
    'Content-Type: multipart/form-data',
    'Api-Key: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' // place your api key here
);

// Form cURL POST command that will be executed with the zip endpoint.
// NOTE: The '-s' in the cURL command below runs cURL in silent mode, so exec() output is not shown.
$curl_command = 'curl -s -X POST "'.$zip_endpoint_url.'" -H "'.$headers[0].'" -H "'.$headers[1].'" -H "'.$headers[2].'" -F "file=@'.$data['file'][0].'" -F "file=@'.$data['file'][1].'" -F "output='.$data['output'].'"';

print "Sending POST request to zip endpoint...\n";
exec($curl_command, $response);

print json_encode(json_decode($response[0]), JSON_PRETTY_PRINT | JSON_UNESCAPED_SLASHES);
print "\n";
?>

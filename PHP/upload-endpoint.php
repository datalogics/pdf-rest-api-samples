<?php
require("../Sample_Input/sample_input.php");

// The /merged-pdf endpoint can take one or more PDF files or ids as input.
// This sample takes 2 PDF files and merges all the pages in the document into a single document.
$upload_endpoint_url = 'https://api.pdfrest.com/upload';

// Create an array that contains that data that will be passed to the POST request.
$data = array(
    'file' => array(
        SAMPLE_INPUT_DIR . 'ducky.pdf',
        SAMPLE_INPUT_DIR . 'merge1.pdf',
        SAMPLE_INPUT_DIR . 'merge2.pdf'
    ),
);

$headers = array(
    'Accept: application/json',
    'Content-Type: multipart/form-data',
    'Api-Key: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' // place your api key here
);

// Form cURL POST command that will be executed with the merged-pdf endpoint.
// NOTE: The '-s' in the cURL command below runs cURL in silent mode, so exec() output is not shown.
$curl_command = 'curl -s -X POST "'.$upload_endpoint_url.'" -H "'.$headers[0].'" -H "'.$headers[1].'" -H "'.$headers[2].'" -F "file=@'.$data['file'][0].'" -F "file=@'.$data['file'][1].'"  -F "file=@'.$data['file'][2].'"';

print "Sending POST request to upload endpoint...\n";
exec($curl_command, $response);

print json_encode(json_decode($response[0]), JSON_PRETTY_PRINT | JSON_UNESCAPED_SLASHES);
print "\n";
?>

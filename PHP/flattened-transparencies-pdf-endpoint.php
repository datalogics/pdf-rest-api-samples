<?php
require("../Sample_Input/sample_input.php");

// The /flattened-transparencies-pdf endpoint can take a single PDF file or id as input.
// This sample demonstrates setting quality to 'medium'.
// We have preset 'high', 'medium', and 'low' quality levels available for use. These preset levels do not require the 'profile' parameter.
$flattened_transparencies_pdf_endpoint_url = 'https://api.pdfrest.com/flattened-transparencies-pdf';

// Create an array that contains that data that will be passed to the POST request.
$data = array(
    'file' => new CURLFile(SAMPLE_INPUT_DIR . 'toFlattenTransparencies.pdf','application/pdf', 'toFlattenTransparencies.pdf'),
    'output' => 'example_flattenedTransparenciesPdf_out',
    'quality' => 'medium'
);

$headers = array(
    'Accept: application/json',
    'Content-Type: multipart/form-data',
    'Api-Key: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' // place your api key here
);

// Initialize a cURL session.
$ch = curl_init();

// Set the url, headers, and data that will be sent to flattened-transparencies-pdf endpoint.
curl_setopt($ch, CURLOPT_URL, $flattened_transparencies_pdf_endpoint_url);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
curl_setopt($ch, CURLOPT_POST, true);
curl_setopt($ch, CURLOPT_POSTFIELDS, $data);

print "Sending POST request to flattened-transparencies-pdf endpoint...\n";
$response = curl_exec($ch);

print "Response status code: " . curl_getinfo($ch, CURLINFO_HTTP_CODE) . "\n";

if($response === false){
    print 'Error: ' . curl_error($ch) . "\n";
}else{
    print json_encode(json_decode($response), JSON_PRETTY_PRINT | JSON_UNESCAPED_SLASHES);
    print "\n";
}

curl_close($ch);
?>

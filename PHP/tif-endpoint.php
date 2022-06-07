<?php
require("../Sample_Input/sample_input.php");

// The /tif endpoint can take a single PDF file or id as input and turn them into TIFF image files.
// This sample takes in a PDF and converts all pages into grayscale TIFF files.

$tif_endpoint_url = 'https://cloud-api.datalogics.com/tif';

// Create an array that contains that data that will be passed to the POST request.
$data = array(
    'file' => new CURLFile(SAMPLE_INPUT_DIR . 'ducky.pdf', 'application/pdf', 'ducky.pdf'),
    'pages' => '1-last',
    'resolution' => '600',
    'color_model' => 'gray',
    'output' => 'example_tif_out',
);

$headers = array(
    'Accept: application/json',
    'Content-Type: multipart/form-data',
    'Api-Key: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' // place your api key here
);

// Initialize a cURL session.
$ch = curl_init();

// Set the url, headers, and data that will be sent to tif endpoint.
curl_setopt($ch, CURLOPT_URL, $tif_endpoint_url);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
curl_setopt($ch, CURLOPT_POST, true);
curl_setopt($ch, CURLOPT_POSTFIELDS, $data);

print "Sending POST request to tif endpoint...\n";
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

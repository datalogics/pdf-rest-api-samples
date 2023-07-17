<?php
require("../Sample_Input/sample_input.php");

// The /pdf endpoint can take a single file, id, or url as input. 
// This sample passes a jpeg file to the endpoint, but there's a variety of input file types that are accepted by this endpoint.

$pdf_endpoint_url = 'https://api.pdfrest.com/pdf';

// Create an array that contains that data that will be passed to the POST request.
// The 'image/jpeg' string below is known as a MIME type, which is a label used to identify the type of a file so that it is handled properly by software.
// Please see https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types for more information about MIME types.
$data = array(
    'file' => new CURLFile(SAMPLE_INPUT_DIR . 'rainbow.tif', 'image/tiff', 'rainbow.tif'),
    'output' => 'example_pdf_out'
);

$headers = array(
    'Accept: application/json',
    'Content-Type: multipart/form-data',
    'Api-Key: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' // place your api key here
);

// Initialize a cURL session.
$ch = curl_init();

// Set the url, headers, and data that will be sent to pdf endpoint.
curl_setopt($ch, CURLOPT_URL, $pdf_endpoint_url);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
curl_setopt($ch, CURLOPT_POST, true);
curl_setopt($ch, CURLOPT_POSTFIELDS, $data);

print "Sending POST request to pdf endpoint...\n";
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

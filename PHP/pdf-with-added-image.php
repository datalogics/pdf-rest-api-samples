<?php
require("../Sample_Input/sample_input.php");

$pdf_with_added_image_endpoint_url = 'https://api.pdfrest.com/pdf-with-added-image';

$data = array(
    'file' => new CURLFile(SAMPLE_INPUT_DIR . 'ducky.pdf','application/pdf', 'ducky.pdf'),
    'image_file' => new CURLFile(SAMPLE_INPUT_DIR . 'ducky.jpg','image/jpeg', 'strawberries.jpg'),
    'output' => 'example_out',
    'x' => '10',
    'y' => '10',
    'page' => '1'
);

$headers = array(
    'Accept: application/json',
    'Content-Type: multipart/form-data',
    'Api-Key: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' // place your api key here
);

$ch = curl_init();

curl_setopt($ch, CURLOPT_URL, $pdf_with_added_image_endpoint_url);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
curl_setopt($ch, CURLOPT_POST, true);
curl_setopt($ch, CURLOPT_POSTFIELDS, $data);

print "Sending POST request to pdf-with-added-image endpoint...\n";
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
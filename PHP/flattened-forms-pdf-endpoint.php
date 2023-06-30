<?php
require("../Sample_Input/sample_input.php");

$flatten_forms_endpoint_url = 'https://api.pdfrest.com/flattened-forms-pdf';

$data = array(
    'file' => new CURLFile(SAMPLE_INPUT_DIR . 'ducky.pdf','application/pdf', 'ducky.pdf'),
    'output' => 'example_out'
);

$headers = array(
    'Accept: application/json',
    'Content-Type: multipart/form-data',
    'Api-Key: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' // place your api key here
);

$ch = curl_init();

curl_setopt($ch, CURLOPT_URL, $flatten_forms_endpoint_url);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
curl_setopt($ch, CURLOPT_POST, true);
curl_setopt($ch, CURLOPT_POSTFIELDS, $data);

print "Sending POST request to flattened-forms-pdf endpoint...\n";
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

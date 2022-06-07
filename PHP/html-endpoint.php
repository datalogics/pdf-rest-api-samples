<?php
// The /html endpoint can take a string of HTML content and convert it to a HTML (.html) file.
// This sample takes in a string of HTML content that displays "Hello World!" and turns it into a HTML file.

$html_endpoint_url = 'https://cloud-api.datalogics.com/html';

// Create an array that contains that data that will be passed to the POST request.
$data = array(
    'content' => '<html><head><title>Web Page</title></head><body>Hello World!</body></html>',
    'output' => 'example_html_out'
);

$headers = array(
    'Accept: application/json',
    'Content-Type: multipart/form-data',
    'Api-Key: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' // place your api key here
);

// Initialize a cURL session.
$ch = curl_init();

// Set the url, headers, and data that will be sent to html endpoint.
curl_setopt($ch, CURLOPT_URL, $html_endpoint_url);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
curl_setopt($ch, CURLOPT_POST, true);
curl_setopt($ch, CURLOPT_POSTFIELDS, $data);

print "Sending POST request to html endpoint...\n";
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

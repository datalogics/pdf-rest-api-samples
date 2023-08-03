<?php

// The /encrypted-pdf endpoint can take a single PDF file or id as input.
// This sample demonstrates setting the open password to 'password'.
$encrypted_pdf_endpoint_url = 'https://api.pdfrest.com/encrypted-pdf';

// Create an array that contains that data that will be passed to the POST request.
$data = array(
    'file' => new CURLFile('/path/to/file','application/pdf', 'file_name'),
    'output' => 'example_encryptedPdf_out',
    'new_open_password' => 'password'
);

$headers = array(
    'Accept: application/json',
    'Content-Type: multipart/form-data',
    'Api-Key: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' // place your api key here
);

// Initialize a cURL session.
$ch = curl_init();

// Set the url, headers, and data that will be sent to encrypted-pdf endpoint.
curl_setopt($ch, CURLOPT_URL, $encrypted_pdf_endpoint_url);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
curl_setopt($ch, CURLOPT_POST, true);
curl_setopt($ch, CURLOPT_POSTFIELDS, $data);

print "Sending POST request to encrypted-pdf endpoint...\n";
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

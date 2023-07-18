<?php

// The /split-pdf endpoint can take one PDF file or id as input.
// This sample takes one PDF file that has at least 5 pages and splits it into two documents when given two page ranges.
$split_pdf_endpoint_url = 'https://api.pdfrest.com/split-pdf';

// Create an array that contains that data that will be passed to the POST request.
// NOTE: PHP array keys cannot be an array, but the endpoint expects the 'pages[]' field so the page range key must be passed as 'pages[0]', 'pages[1]', etc.
$data = array(
    'file' => new CURLFile('/path/to/file','application/pdf', 'file_name'),
    'pages[0]' => '1,2,5',
    'pages[1]' => '3,4',
    'output' => 'example_splitPdf_out'
);

$headers = array(
    'Accept: application/json',
    'Content-Type: multipart/form-data',
    'Api-Key: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' // place your api key here
);

// Initialize a cURL session.
$ch = curl_init();

// Set the url, headers, and data that will be sent to split-pdf endpoint.
curl_setopt($ch, CURLOPT_URL, $split_pdf_endpoint_url);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
curl_setopt($ch, CURLOPT_POST, true);
curl_setopt($ch, CURLOPT_POSTFIELDS, $data);

print "Sending POST request to split-pdf endpoint...\n";
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

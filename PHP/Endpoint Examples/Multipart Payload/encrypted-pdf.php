<?php
require 'vendor/autoload.php'; // Require the autoload file to load Guzzle HTTP client.

use GuzzleHttp\Client; // Import the Guzzle HTTP client namespace.
use GuzzleHttp\Psr7\Request; // Import the PSR-7 Request class.
use GuzzleHttp\Psr7\Utils; // Import the PSR-7 Utils class for working with streams.

$client = new Client(); // Create a new instance of the Guzzle HTTP client.

$headers = [
  'Api-Key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' // Set the API key in the headers for authentication.
];

$options = [
  'multipart' => [
    [
      'name' => 'file', // Specify the field name for the file.
      'contents' => Utils::tryFopen('/path/to/file', 'r'), // Open the file specified by the '/path/to/file' for reading.
      'filename' => '/path/to/file', // Set the filename for the file to be processed, in this case, '/path/to/file'.
      'headers' => [
        'Content-Type' => '<Content-type header>' // Set the Content-Type header for the file.
      ]
    ],
    [
      'name' => 'new_open_password', // Specify the field name for the new open password.
      'contents' => 'new_example_pw' // Set the value for the new open password (in this case, 'new_example_pw').
    ],
    [
      'name' => 'output', // Specify the field name for the output option.
      'contents' => 'pdfrest_encrypted_pdf' // Set the value for the output option (in this case, 'pdfrest_encrypted_pdf').
    ]
  ]
];

$request = new Request('POST', 'https://api.pdfrest.com/encrypted-pdf', $headers); // Create a new HTTP POST request with the API endpoint and headers.

$res = $client->sendAsync($request, $options)->wait(); // Send the asynchronous request and wait for the response.

$body_str = (string)$res->getBody();
echo $body_str; // Output the response body, which contains the encrypted PDF content.

// All files uploaded or generated are automatically deleted based on the 
// File Retention Period as shown on https://pdfrest.com/pricing. 
// For immediate deletion of files, particularly when sensitive data 
// is involved, an explicit delete call can be made to the API.
//
// The following code is an optional step to delete sensitive files
// (unredacted, unencrypted, unrestricted, or unwatermarked) from pdfRest servers.

$delete_client = new Client(['http_errors' => false]);
$delete_headers = [
  'api-key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'Content-Type' => 'application/json'
];
$parsed = json_decode($body_str, true);
$input_id = isset($parsed['inputId']) ? $parsed['inputId'] : '';
$delete_body = json_encode([ 'ids' => $input_id ]);
$delete_request = new Request('POST', 'https://api.pdfrest.com/delete', $delete_headers, $delete_body);
$delete_res = $delete_client->sendAsync($delete_request)->wait();
echo $delete_res->getBody() . PHP_EOL;

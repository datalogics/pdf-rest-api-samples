<?php
require 'vendor/autoload.php'; // Require the autoload file to load Guzzle HTTP client.

use GuzzleHttp\Client; // Import the Guzzle HTTP client namespace.
use GuzzleHttp\Psr7\Request; // Import the PSR-7 Request class.
use GuzzleHttp\Psr7\Utils; // Import the PSR-7 Utils class for working with streams.

$client = new Client();
$headers = [
  'Api-Key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'
];
$options = [
  'multipart' => [
    [
      'name' => 'file',
      'contents' => Utils::tryFopen('/path/to/file', 'r'), // Provide the path to the PDF file with forms.
      'filename' => '/path/to/file',
      'headers'  => [
        'Content-Type' => '<Content-type header>'
      ]
    ],
    [
      'name' => 'output',
      'contents' => '' // Specify the output type (e.g., 'pdfrest_flattened_form', 'pdfrest_flattened_annotations', etc.)
    ]
]];

$request = new Request('POST', 'https://api.pdfrest.com/flattened-forms-pdf', $headers);  // Create a new HTTP POST request with the API endpoint and headers.

$res = $client->sendAsync($request, $options)->wait(); // Send the asynchronous request and wait for the response.

echo $res->getBody(); // Output the response body, which contains the flattened PDF with forms .

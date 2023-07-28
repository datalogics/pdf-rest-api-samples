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
      'contents' => Utils::tryFopen('/path/to/file1', 'r'), // Provide the path to the first file to be included in the zip.
      'filename' => '/path/to/file1',
      'headers'  => [
        'Content-Type' => '<Content-type header>'
      ]
    ],
    [
      'name' => 'file',
      'contents' => Utils::tryFopen('/path/to/file2', 'r'), // Provide the path to the second file to be included in the zip.
      'filename' => '/path/to/file2',
      'headers'  => [
        'Content-Type' => '<Content-type header>'
      ]
    ],
    [
      'name' => 'id[]',
      'contents' => 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' // Provide the ID of the file to be included in the zip.
    ],
    [
      'name' => 'output',
      'contents' => 'pdfrest_zip' // Specify the output type as zip.
    ]
]];

$request = new Request('POST', 'https://api.pdfrest.com/zip', $headers);  // Create a new HTTP POST request with the API endpoint and headers.

$res = $client->sendAsync($request, $options)->wait(); // Send the asynchronous request and wait for the response.

echo $res->getBody(); // Output the response body, which contains the zipped files.

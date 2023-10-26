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
      'name' => 'file', // Specify the field name for the first file.
      'contents' => Utils::tryFopen('/path/to/file1', 'r'), // Open the first file for reading.
      'filename' => '/path/to/file1', // Set the filename for the first file to be uploaded.
      'headers' => [
        'Content-Type' => '<Content-type header for file1>' // Set the Content-Type header for the first file.
      ]
    ],
    [
      'name' => 'file', // Specify the field name for the second file.
      'contents' => Utils::tryFopen('/path/to/file2', 'r'), // Open the second file for reading.
      'filename' => '/path/to/file2', // Set the filename for the second file to be uploaded.
      'headers' => [
        'Content-Type' => '<Content-type header for file2>' // Set the Content-Type header for the second file.
      ]
    ],
    [
      'name' => 'file', // Specify the field name for the third file.
      'contents' => Utils::tryFopen('/path/to/file3', 'r'), // Open the third file for reading.
      'filename' => '/path/to/file3', // Set the filename for the third file to be uploaded.
      'headers' => [
        'Content-Type' => '<Content-type header for file3>' // Set the Content-Type header for the third file.
      ]
    ]
  ]
];

$request = new Request('POST', 'https://api.pdfrest.com/upload', $headers); // Create a new HTTP POST request with the API endpoint and headers.

$res = $client->sendAsync($request, $options)->wait(); // Send the asynchronous request and wait for the response.

echo $res->getBody(); // Output the response body.

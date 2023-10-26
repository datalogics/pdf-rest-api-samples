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
      'contents' => Utils::tryFopen('/path/to/file', 'r'), // Open the file specified by the '/path/to/file' for reading.
      'filename' => '/path/to/file', // Set the filename for the first file to be merged, in this case, '/path/to/file'.
      'headers' => [
        'Content-Type' => '<Content-type header>' // Set the Content-Type header for the first file.
      ]
    ],
    [
      'name' => 'pages[]', // Specify the field name for the pages option for the first file.
      'contents' => '1-last' // Set the pages value for the first file (in this case, '1-last').
    ],
    [
      'name' => 'type[]', // Specify the field name for the type option for the first file.
      'contents' => 'file' // Set the type value for the first file (in this case, 'file').
    ],
    [
      'name' => 'file', // Specify the field name for the second file.
      'contents' => Utils::tryFopen('/path/to/file', 'r'), // Open the file specified by the '/path/to/file' for reading.
      'filename' => '/path/to/file', // Set the filename for the second file to be merged, in this case, '/path/to/file'.
      'headers' => [
        'Content-Type' => '<Content-type header>' // Set the Content-Type header for the second file.
      ]
    ],
    [
      'name' => 'pages[]', // Specify the field name for the pages option for the second file.
      'contents' => '1-last' // Set the pages value for the second file (in this case, '1-last').
    ],
    [
      'name' => 'type[]', // Specify the field name for the type option for the second file.
      'contents' => 'file' // Set the type value for the second file (in this case, 'file').
    ],
    [
      'name' => 'id[]', // Specify the field name for the ID option.
      'contents' => 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' // Set the value for the ID option (in this case, 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx').
    ],
    [
      'name' => 'pages[]', // Specify the field name for the pages option for the third file (identified by ID).
      'contents' => '1-last' // Set the pages value for the third file (identified by ID) (in this case, '1-last').
    ],
    [
      'name' => 'type[]', // Specify the field name for the type option for the third file (identified by ID).
      'contents' => 'id' // Set the type value for the third file (identified by ID) (in this case, 'id').
    ],
    [
      'name' => 'output', // Specify the field name for the output option.
      'contents' => 'pdfrest_merged_pdf' // Set the value for the output option (in this case, 'pdfrest_merged_pdf').
    ]
  ]
];

$request = new Request('POST', 'https://api.pdfrest.com/merged-pdf', $headers); // Create a new HTTP POST request with the API endpoint and headers.

$res = $client->sendAsync($request, $options)->wait(); // Send the asynchronous request and wait for the response.

echo $res->getBody(); // Output the response body, which contains the merged PDF content.

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
      'contents' => Utils::tryFopen('/path/to/file', 'r'), // Open the file specified by '/path/to/file' for reading.
      'filename' => '/path/to/file', // Set the filename for the file to be processed, in this case, '/path/to/file'.
      'headers' => [
        'Content-Type' => '<Content-type header>' // Set the Content-Type header for the file.
      ]
    ],
    [
      'name' => 'pages[]', // Specify the field name for the pages option as an array.
      'contents' => 'even' // Set the value for the pages option to 'even'.
    ],
    [
      'name' => 'pages[]', // Specify the field name for the pages option as an array.
      'contents' => 'odd' // Set the value for the pages option to 'odd'.
    ],
    [
      'name' => 'pages[]', // Specify the field name for the pages option as an array.
      'contents' => '1,3,4-6' // Set the value for the pages option to '1,3,4-6' (a combination of specific pages and page ranges).
    ],
    [
      'name' => 'output', // Specify the field name for the output option.
      'contents' => 'pdfrest_split_pdf' // Set the value for the output option to generate split PDFs.
    ]
  ]
];

$request = new Request('POST', 'https://api.pdfrest.com/split-pdf', $headers); // Create a new HTTP POST request with the API endpoint and headers.

$res = $client->sendAsync($request, $options)->wait(); // Send the asynchronous request and wait for the response.

echo $res->getBody(); // Output the response body, which contains the split PDFs.

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
      'name' => 'compression', // Specify the field name for the compression option.
      'contents' => 'lossless' // Set the value for the compression option (in this case, 'lossless').
    ],
    [
      'name' => 'downsample', // Specify the field name for the downsample option.
      'contents' => '300' // Set the value for the downsample option (in this case, '300').
    ],
    [
      'name' => 'tagged_pdf', // Specify the field name for the tagged PDF option.
      'contents' => 'off' // Set the value for the tagged PDF option (in this case, 'off').
    ],
    [
      'name' => 'output', // Specify the field name for the output option.
      'contents' => 'pdfrest_pdf' // Set the value for the output option (in this case, 'pdfrest_pdf').
    ]
  ]
];

$request = new Request('POST', 'https://api.pdfrest.com/pdf', $headers); // Create a new HTTP POST request with the API endpoint and headers.

$res = $client->sendAsync($request, $options)->wait(); // Send the asynchronous request and wait for the response.

echo $res->getBody(); // Output the response body, which contains the processed PDF content.

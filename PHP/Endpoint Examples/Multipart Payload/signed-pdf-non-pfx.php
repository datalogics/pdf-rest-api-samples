<?php
require 'vendor/autoload.php'; // Require the autoload file to load Guzzle HTTP client.

use GuzzleHttp\Client; // Import the Guzzle HTTP client namespace.
use GuzzleHttp\Psr7\Request; // Import the PSR-7 Request class.
use GuzzleHttp\Psr7\Utils; // Import the PSR-7 Utils class for working with streams.

$client = new Client(); // Create a new instance of the Guzzle HTTP client.

$headers = [
  'Api-Key' => 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' // Set the API key in the headers for authentication.
];

$options = [
  'multipart' => [
    [
      'name' => 'file', // Specify the field name for the file.
      'contents' => Utils::tryFopen('/path/to/file', 'r'), // Open the file specified by the '/path/to/file' for reading.
      'filename' => 'filename.pdf', // Set the filename for the file to be processed, in this case, '/path/to/file'.
      'headers' => [
        'Content-Type' => 'application/pdf' // Set the Content-Type header for the file.
      ]
    ],
    [
      'name' => 'certificate_file', // Specify the field name for the file.
      'contents' => Utils::tryFopen('/path/to/file', 'r'), // Open the file specified by the '/path/to/file' for reading.
      'filename' => 'filename.pfx', // Set the filename for the file to be processed, in this case, '/path/to/file'.
      'headers' => [
        'Content-Type' => 'application/x-pkcs12' // Set the Content-Type header for the file.
      ]
    ],
    [
      'name' => 'private_key_file', // Specify the field name for the file.
      'contents' => Utils::tryFopen('/path/to/file', 'r'), // Open the file specified by the '/path/to/file' for reading.
      'filename' => 'filename.txt', // Set the filename for the file to be processed, in this case, '/path/to/file'.
      'headers' => [
        'Content-Type' => 'text/plain' // Set the Content-Type header for the file.
      ]
    ],
    [
      'name' => 'signature_configuration', // Specify the field name for the digital signature config.
      'contents' => '{"type":"new","name":"esignature","location":{"bottom_left":{ "x":"0", "y":"0" },"top_right":{ "x":"216", "y":"72" },"page":1},"display":{"include_datetime":"true"}}' // Set the value for the signature configuration. This is a JSON-formatted string.
    ],
    [
      'name' => 'output', // Specify the field name for the output option.
      'contents' => 'pdfrest_signed_pdf' // Set the value for the output option (in this case, 'pdfrest_signed_pdf').
    ]
  ]
];

$request = new Request('POST', 'https://api.pdfrest.com/signed-pdf', $headers); // Create a new HTTP POST request with the API endpoint and headers.

$res = $client->sendAsync($request, $options)->wait(); // Send the asynchronous request and wait for the response.

echo $res->getBody(); // Output the response body, which contains the signed PDF.

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
      'name' => 'output', // Specify the field name for the output option.
      'contents' => 'pdfrest_restricted_pdf' // Set the value for the output option to generate a restricted PDF.
    ],
    [
      'name' => 'new_permissions_password', // Specify the field name for the new_permissions_password option.
      'contents' => 'new_example_pw' // Set the value for the new_permissions_password option to 'new_example_pw'.
    ],
    [
      'name' => 'restrictions[]', // Specify the field name for the restrictions option as an array.
      'contents' => 'print_low' // Add the 'print_low' restriction to the restrictions array.
    ],
    [
      'name' => 'restrictions[]', // Specify the field name for the restrictions option as an array.
      'contents' => 'accessibility_off' // Add the 'accessibility_off' restriction to the restrictions array.
    ],
    [
      'name' => 'restrictions[]', // Specify the field name for the restrictions option as an array.
      'contents' => 'edit_content' // Add the 'edit_content' restriction to the restrictions array.
    ]
  ]
];

$request = new Request('POST', 'https://api.pdfrest.com/restricted-pdf', $headers); // Create a new HTTP POST request with the API endpoint and headers.

$res = $client->sendAsync($request, $options)->wait(); // Send the asynchronous request and wait for the response.

echo $res->getBody(); // Output the response body, which contains the restricted PDF with specified permissions.

<?php
require 'vendor/autoload.php'; // Require the autoload file to load Guzzle HTTP client.

use GuzzleHttp\Client; // Import the Guzzle HTTP client namespace.
use GuzzleHttp\Psr7\Request; // Import the PSR-7 Request class.
use GuzzleHttp\Psr7\Utils; // Import the PSR-7 Utils class for working with streams.

// By default, we use the US-based API service. This is the primary endpoint for global use.
$apiUrl = "https://api.pdfrest.com";

/* For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
 * For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 */
//$apiUrl = "https://eu-api.pdfrest.com";

$client = new Client(); // Create a new instance of the Guzzle HTTP client.

$headers = [
  'Api-Key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' // Set the API key in the headers for authentication.
];

$options = [
  'multipart' => [
    [
      'name' => 'file', // Specify the field name for the file.
      'contents' => Utils::tryFopen('/path/to/file', 'r'), // Open the file specified by the '/path/to/file' for reading.
      'filename' => '/path/to/file', // Set the filename for the file to be converted, in this case, '/path/to/file'.
      'headers' => [
        'Content-Type' => '<Content-type header>' // Set the Content-Type header for the file.
      ]
    ],
    [
      'name' => 'pages', // Specify the field name for the pages option.
      'contents' => '1-last' // Set the value for the pages option (in this case, '1-last').
    ],
    [
      'name' => 'resolution', // Specify the field name for the resolution option.
      'contents' => '300' // Set the value for the resolution option (in this case, '300').
    ],
    [
      'name' => 'color_model', // Specify the field name for the color_model option.
      'contents' => 'rgb' // Set the value for the color_model option (in this case, 'rgb').
    ],
    [
      'name' => 'output', // Specify the field name for the output option.
      'contents' => 'pdfrest_png' // Set the value for the output option (in this case, 'pdfrest_png').
    ]
  ]
];

$request = new Request('POST', $apiUrl.'/png', $headers); // Create a new HTTP POST request with the API endpoint and headers.

$res = $client->sendAsync($request, $options)->wait(); // Send the asynchronous request and wait for the response.

echo $res->getBody(); // Output the response body, which contains the converted PNG image.

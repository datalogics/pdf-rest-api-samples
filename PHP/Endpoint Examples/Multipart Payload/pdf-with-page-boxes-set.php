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
  'Api-Key' => 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' // Set the API key in the headers for authentication.
];

$options = [
  'multipart' => [
    [
      'name' => 'file', // Specify the field name for the file.
      'contents' => Utils::tryFopen('/path/to/file', 'r'), // Open the file specified by the '/path/to/file' for reading.
      'filename' => 'flename.pdf', // Set the filename for the file to be processed, in this case, '/path/to/file'.
      'headers' => [
        'Content-Type' => '<Content-type header>' // Set the Content-Type header for the file.
      ]
    ],
    [
      'name' => 'boxes', // Specify the field name for the text options.
      'contents' => '{"boxes":[{"box":"media","pages":[{"range":"1","left":100,"top":100,"bottom":100,"right":100}]}]}' // Set the value for the boxes option. This is a JSON-formatted string consisting of an array with sets of text options.
    ],
    [
      'name' => 'output', // Specify the field name for the output option.
      'contents' => 'pdfrest_pdf_with_boxes_set' // Set the value for the output option (in this case, 'pdfrest_pdf_with_boxes_set').
    ]
  ]
];

$request = new Request('POST', $apiUrl.'/pdf-with-page-boxes-set', $headers); // Create a new HTTP POST request with the API endpoint and headers.

$res = $client->sendAsync($request, $options)->wait(); // Send the asynchronous request and wait for the response.

echo $res->getBody(); // Output the response body, which contains the PDF with the boxes set.

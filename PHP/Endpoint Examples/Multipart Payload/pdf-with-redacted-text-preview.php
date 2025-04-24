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
      'filename' => '/path/to/file', // Set the filename for the file to be processed, in this case, '/path/to/file'.
      'headers' => [
        'Content-Type' => '<Content-type header>' // Set the Content-Type header for the file.
      ]
    ],
    [
      'name' => 'redactions', // Specify the field name for the text options.
      'contents' => '[{"type":"preset","value":"email"},{"type":"regex","value":"(\\\\+\\\\d{1,2}\\\\s)?\\\\(?\\\\d{3}\\\\)?[\\\\s.-]\\\\d{3}[\\\\s.-]\\\\d{4}"},{"type":"literal","value":"word"}]' // Set the value for the redactions option. This is a JSON-formatted string consisting of an array with sets of text options.
    ],
    [
      'name' => 'output', // Specify the field name for the output option.
      'contents' => 'pdfrest_pdf_with_redacted_text' // Set the value for the output option (in this case, 'pdfrest_pdf_with_redacted_text').
    ]
  ]
];

$request = new Request('POST', 'https://api.pdfrest.com/pdf-with-redacted-text-preview', $headers); // Create a new HTTP POST request with the API endpoint and headers.

$res = $client->sendAsync($request, $options)->wait(); // Send the asynchronous request and wait for the response.

echo $res->getBody(); // Output the response body, which contains the PDF with redaction preview annotations.

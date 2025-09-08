<?php
require 'vendor/autoload.php'; // Require the autoload file to load Guzzle HTTP client.

use GuzzleHttp\Client; // Import the Guzzle HTTP client namespace.
use GuzzleHttp\Psr7\Request; // Import the PSR-7 Request class.
use GuzzleHttp\Psr7\Utils; // Import the PSR-7 Utils class for working with streams.

// Toggle deletion of sensitive files (default: false)
$DELETE_SENSITIVE_FILES = false;

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
      'name' => 'current_open_password', // Specify the field name for the current open password.
      'contents' => 'current_example_pw' // Set the value for the current open password (in this case, 'current_example_pw').
    ],
    [
      'name' => 'output', // Specify the field name for the output option.
      'contents' => 'pdfrest_decrypted_pdf' // Set the value for the output option (in this case, 'pdfrest_decrypted_pdf').
    ]
  ]
];

$request = new Request('POST', 'https://api.pdfrest.com/decrypted-pdf', $headers); // Create a new HTTP POST request with the API endpoint and headers.

$res = $client->sendAsync($request, $options)->wait(); // Send the asynchronous request and wait for the response.

$body = (string) $res->getBody();
echo $body; // Output the response body

// All files uploaded or generated are automatically deleted based on the 
// File Retention Period as shown on https://pdfrest.com/pricing. 
// For immediate deletion of files, particularly when sensitive data 
// is involved, an explicit delete call can be made to the API.
//
// Deletes all files in the workflow, including outputs. Save all desired files before enabling this step.

if ($DELETE_SENSITIVE_FILES) {
  $delete_client = new Client(['http_errors' => false]);
  $delete_headers = [
    'api-key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
    'Content-Type' => 'application/json'
  ];
  $json = json_decode($body, true);
  $input_id = isset($json['inputId']) ? $json['inputId'] : '';
  $output_id = isset($json['outputId']) ? $json['outputId'] : '';
  $delete_body = json_encode([ 'ids' => $input_id . ', ' . $output_id ]);
  $delete_request = new Request('POST', 'https://api.pdfrest.com/delete', $delete_headers, $delete_body);
  $delete_res = $delete_client->sendAsync($delete_request)->wait();
  echo $delete_res->getBody() . PHP_EOL;
}

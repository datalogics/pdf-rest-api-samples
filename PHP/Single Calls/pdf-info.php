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
      'name' => 'queries', // Specify the field name for the queries option.
      'contents' => 'tagged,image_only,title,subject,author,producer,creator,creation_date,modified_date,keywords,doc_language,page_count,contains_annotations,contains_signature,pdf_version,file_size,filename,restrict_permissions_set,contains_xfa,contains_acroforms,contains_javascript,contains_transparency,contains_embedded_file,uses_embedded_fonts,uses_nonembedded_fonts,pdfa,requires_password_to_open,pdfua_claim,pdfe_claim,pdfx_claim' // Set the value for the queries option (a comma-separated list of various queries).
    ]
  ]
];

$request = new Request('POST', 'https://api.pdfrest.com/pdf-info', $headers); // Create a new HTTP POST request with the API endpoint and headers.

$res = $client->sendAsync($request, $options)->wait(); // Send the asynchronous request and wait for the response.

echo $res->getBody(); // Output the response body, which contains the information about the PDF file based on the specified queries.

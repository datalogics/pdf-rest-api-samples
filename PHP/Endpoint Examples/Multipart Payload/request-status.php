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

$client = new Client(); // Create a new instance of the Guzzle HTTP client

$apiKey = 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'; // Your API key goes here.

$headers = [
  'Api-Key' => $apiKey,
  'response-type' => 'requestId' // Use this header to obtain a request status.
];

$pngOptions = [
  'multipart' => [
    [
      'name' => 'file',
      'contents' => Utils::tryFopen('/path/to/file.pdf', 'r'),
      'filename' => 'file.pdf',
      'headers' => [
        'Content-Type' => '<Content-type header>'
      ]
    ]
  ]
];

// Using /png as an arbitrary example, send a request with the Request-Type header.
$pngRequest = new Request('POST', $apiUrl.'/png', $headers);

$pngResponse = $client->sendAsync($pngRequest, $pngOptions)->wait();

echo $pngResponse->getBody();
echo "\r\n";

// Get the request ID from the response.
$requestId = json_decode($pngResponse->getBody())->{'requestId'};

// Get the status of the PNG request by its ID.
$request_status_endpoint_url = $apiUrl.'/request-status/'.$requestId;

$headers = [
  'Api-Key' => $apiKey
];

$request = new Request('GET', $request_status_endpoint_url, $headers);

$res = $client->sendAsync($request)->wait();

$status = json_decode($res->getBody())->{'status'};

// This example repeats the status request until the request is fulfilled.
while (strcmp($status, "pending") == 0):
  echo $res->getBody(); // Output the response body, which contains the status information.
  echo "\r\n";
  sleep(5);
  $res = $client->sendAsync($request)->wait();
  $status = json_decode($res->getBody())->{'status'};
endwhile;

echo $res->getBody();
echo "\r\n";

?>

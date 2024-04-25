<?php
require 'vendor/autoload.php'; // Require the autoload file to load Guzzle HTTP client.

use GuzzleHttp\Client; // Import the Guzzle HTTP client namespace.
use GuzzleHttp\Psr7\Request; // Import the PSR-7 Request class.
use GuzzleHttp\Psr7\Utils; // Import the PSR-7 Utils class for working with streams.

$client = new Client(); // Create a new instance of the Guzzle HTTP client.

$requestId = 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'; // place the requestId returned from a previous POST request here

$request_status_endpoint_url = 'https://api.pdfrest.com/request-status/'.$requestId;

$headers = [
  'Api-Key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' // Set the API key in the headers for authentication.
];

$request = new Request('GET', $request_status_endpoint_url, $headers); // Create a new HTTP GET request with the API endpoint and headers.

$res = $client->sendAsync($request)->wait(); // Send the asynchronous request and wait for the response.

echo $res->getBody(); // Output the response body, which contains the status information.
?>

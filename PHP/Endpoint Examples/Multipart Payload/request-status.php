<?php
require 'vendor/autoload.php'; // Require the autoload file to load Guzzle HTTP client.

use GuzzleHttp\Client; // Import the Guzzle HTTP client namespace.
use GuzzleHttp\Psr7\Request; // Import the PSR-7 Request class.
use GuzzleHttp\Psr7\Utils; // Import the PSR-7 Utils class for working with streams.

$client = new Client(); // Create a new instance of the Guzzle HTTP client

$apiKey = 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'; // Your API key goes here.

$headers = [
  'Api-Key' => $apiKey,
  'response-type' => 'requestId'
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

$pngRequest = new Request('POST', 'https://api.pdfrest.com/png', $headers);

$pngResponse = $client->sendAsync($pngRequest, $pngOptions)->wait();

echo $pngResponse->getBody();
echo "\r\n";

$requestId = json_decode($pngResponse->getBody())->{'requestId'};

$request_status_endpoint_url = 'https://api.pdfrest.com/request-status/'.$requestId;

$headers = [
  'Api-Key' => $apiKey
];

$request = new Request('GET', $request_status_endpoint_url, $headers);

$res = $client->sendAsync($request)->wait();

echo $res->getBody(); // Output the response body, which contains the status information.
echo "\r\n";
?>

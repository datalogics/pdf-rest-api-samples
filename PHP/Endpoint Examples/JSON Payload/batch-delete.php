<?php
require 'vendor/autoload.php'; // Require the autoload file to load Guzzle HTTP client.

use GuzzleHttp\Client; // Import the Guzzle HTTP client namespace.
use GuzzleHttp\Psr7\Request; // Import the PSR-7 Request class.
use GuzzleHttp\Psr7\Utils; // Import the PSR-7 Utils class for working with streams.


$delete_client = new Client(['http_errors' => false]);
$delete_headers = [
  'api-key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'Content-Type' => 'application/json'
];
$delete_body = '{"ids":"xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx, xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"}';
$delete_request = new Request('POST', 'https://api.pdfrest.com/delete', $delete_headers, $delete_body);
$delete_res = $delete_client->sendAsync($delete_request)->wait();
echo $delete_res->getBody() . PHP_EOL;

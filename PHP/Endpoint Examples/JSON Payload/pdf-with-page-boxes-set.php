<?php
require 'vendor/autoload.php'; // Require the autoload file to load Guzzle HTTP client.

use GuzzleHttp\Client; // Import the Guzzle HTTP client namespace.
use GuzzleHttp\Psr7\Request; // Import the PSR-7 Request class.
use GuzzleHttp\Psr7\Utils; // Import the PSR-7 Utils class for working with streams.

$upload_client = new Client(['http_errors' => false]);
$upload_headers = [
  'api-key' => 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'content-filename' => 'filename.pdf',
  'Content-Type' => 'application/octet-stream'
];
$upload_body = file_get_contents('/path/to/file');
$upload_request = new Request('POST', 'https://api.pdfrest.com/upload', $upload_headers, $upload_body);
$upload_res = $upload_client->sendAsync($upload_request)->wait();
echo $upload_res->getBody() . PHP_EOL;

$upload_response_json = json_decode($upload_res->getBody());

$uploaded_id = $upload_response_json->{'files'}[0]->{'id'};

echo "Successfully uploaded with an id of: " . $uploaded_id . PHP_EOL;

$boxes_client = new Client(['http_errors' => false]);
$box_options = '{\"boxes\":[{\"box\":\"media\",\"pages\":[{\"range\":\"1\",\"left\":100,\"top\":100,\"bottom\":100,\"right\":100}]}]}';
$boxes_headers = [
  'api-key' => 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'Content-Type' => 'application/json'
];
$boxes_body = '{"id":"'.$uploaded_id.'", "boxes":"'.$box_options.'"}';
$boxes_request = new Request('POST', 'https://api.pdfrest.com/pdf-with-page-boxes-set', $boxes_headers, $boxes_body);
$boxes_res = $boxes_client->sendAsync($boxes_request)->wait();
echo $boxes_res->getBody() . PHP_EOL;

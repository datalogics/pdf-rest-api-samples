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

$upload_first_file_client = new Client(['http_errors' => false]);
$upload_first_file_headers = [
  'api-key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'content-filename' => 'first_filename.pdf',
  'Content-Type' => 'application/octet-stream'
];
$upload_first_file_body = file_get_contents('/path/to/first_file');
$upload_first_file_request = new Request('POST', $apiUrl.'/upload', $upload_first_file_headers, $upload_first_file_body);
$upload_first_file_res = $upload_first_file_client->sendAsync($upload_first_file_request)->wait();
echo $upload_first_file_res->getBody() . PHP_EOL;

$upload_first_file_response_json = json_decode($upload_first_file_res->getBody());

$uploaded_first_file_id = $upload_first_file_response_json->{'files'}[0]->{'id'};

echo "First file successfully uploaded with an id of: " . $uploaded_first_file_id . PHP_EOL;


$upload_second_file_client = new Client(['http_errors' => false]);
$upload_second_file_headers = [
  'api-key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'content-filename' => 'second_filename.pdf',
  'Content-Type' => 'application/octet-stream'
];
$upload_second_file_body = file_get_contents('/path/to/second_file');
$upload_second_file_request = new Request('POST', $apiUrl.'/upload', $upload_second_file_headers, $upload_second_file_body);
$upload_second_file_res = $upload_second_file_client->sendAsync($upload_second_file_request)->wait();
echo $upload_second_file_res->getBody() . PHP_EOL;

$upload_second_file_response_json = json_decode($upload_second_file_res->getBody());

$uploaded_second_file_id = $upload_second_file_response_json->{'files'}[0]->{'id'};

echo "Second file successfully uploaded with an id of: " . $uploaded_second_file_id . PHP_EOL;



$merge_client = new Client(['http_errors' => false]);
$merge_headers = [
  'api-key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'Content-Type' => 'application/json'
];
$merge_body = '{"id":["'.$uploaded_first_file_id.'", "'.$uploaded_second_file_id.'"], "type":["id","id"], "pages": [1,1]}';
$merge_request = new Request('POST', $apiUrl.'/merged-pdf', $merge_headers, $merge_body);
$merge_res = $merge_client->sendAsync($merge_request)->wait();
echo $merge_res->getBody() . PHP_EOL;

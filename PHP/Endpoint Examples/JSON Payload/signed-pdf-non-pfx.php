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
//$apiUrl "https://eu-api.pdfrest.com";

$upload_client = new Client(['http_errors' => false]);
$input_upload_headers = [
  'api-key' => 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'content-filename' => 'filename.pdf',
  'Content-Type' => 'application/octet-stream'
];
$input_upload_body = file_get_contents('/path/to/file');
$input_upload_request = new Request('POST', $apiUrl.'/upload', $input_upload_headers, $input_upload_body);
$input_upload_res = $upload_client->sendAsync($input_upload_request)->wait();
echo $input_upload_res->getBody() . PHP_EOL;

$input_upload_response_json = json_decode($input_upload_res->getBody());

$uploaded_input_id = $input_upload_response_json->{'files'}[0]->{'id'};

echo "Successfully uploaded with an id of: " . $uploaded_input_id . PHP_EOL;

$certificate_upload_headers = [
  'api-key' => 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'content-filename' => 'filename.pfx',
  'Content-Type' => 'application/octet-stream'
];
$certificate_upload_body = file_get_contents('/path/to/file');
$certificate_upload_request = new Request('POST', $apiUrl.'/upload', $certificate_upload_headers, $certificate_upload_body);
$certificate_upload_res = $upload_client->sendAsync($certificate_upload_request)->wait();
echo $certificate_upload_res->getBody() . PHP_EOL;

$certificate_upload_response_json = json_decode($certificate_upload_res->getBody());

$uploaded_certificate_id = $certificate_upload_response_json->{'files'}[0]->{'id'};

echo "Successfully uploaded with an id of: " . $uploaded_certificate_id . PHP_EOL;

$private_key_upload_headers = [
  'api-key' => 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'content-filename' => 'filename.txt',
  'Content-Type' => 'application/octet-stream'
];
$private_key_upload_body = file_get_contents('/path/to/file');
$private_key_upload_request = new Request('POST', $apiUrl.'/upload', $private_key_upload_headers, $private_key_upload_body);
$private_key_upload_res = $upload_client->sendAsync($private_key_upload_request)->wait();
echo $private_key_upload_res->getBody() . PHP_EOL;

$private_key_upload_response_json = json_decode($private_key_upload_res->getBody());

$uploaded_private_key_id = $private_key_upload_response_json->{'files'}[0]->{'id'};

echo "Successfully uploaded with an id of: " . $uploaded_private_key_id . PHP_EOL;

$signing_client = new Client(['http_errors' => false]);
$signature_config = '{\"type\": \"new\",\"name\": \"esignature\",\"location\": {\"bottom_left\": { \"x\": \"0\", \"y\": \"0\" },\"top_right\": { \"x\": \"216\", \"y\": \"72\" },\"page\": 1},\"display\": {\"include_datetime\": \"true\"}}';
$signing_headers = [
  'api-key' => 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'Content-Type' => 'application/json'
];
$signing_body = '{"id":"'.$uploaded_input_id.'", "certificate_id":"'.$uploaded_certificate_id.'", "private_key_id":"'.$uploaded_private_key_id.'", "signature_configuration":"'.$signature_config.'"}';
$signing_request = new Request('POST', $apiUrl.'/signed-pdf', $signing_headers, $signing_body);
$signing_res = $signing_client->sendAsync($signing_request)->wait();
echo $signing_res->getBody() . PHP_EOL;

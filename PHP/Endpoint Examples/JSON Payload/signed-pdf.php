<?php
require 'vendor/autoload.php'; // Require the autoload file to load Guzzle HTTP client.

use GuzzleHttp\Client; // Import the Guzzle HTTP client namespace.
use GuzzleHttp\Psr7\Request; // Import the PSR-7 Request class.
use GuzzleHttp\Psr7\Utils; // Import the PSR-7 Utils class for working with streams.

$upload_client = new Client(['http_errors' => false]);
$input_upload_headers = [
  'api-key' => 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'content-filename' => 'filename.pdf',
  'Content-Type' => 'application/octet-stream'
];
$input_upload_body = file_get_contents('/path/to/file');
$input_upload_request = new Request('POST', 'https://api.pdfrest.com/upload', $input_upload_headers, $input_upload_body);
$input_upload_res = $upload_client->sendAsync($input_upload_request)->wait();
echo $input_upload_res->getBody() . PHP_EOL;

$input_upload_response_json = json_decode($input_upload_res->getBody());

$uploaded_input_id = $input_upload_response_json->{'files'}[0]->{'id'};

echo "Successfully uploaded with an id of: " . $uploaded_input_id . PHP_EOL;

$credentials_upload_headers = [
  'api-key' => 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'content-filename' => 'filename.pfx',
  'Content-Type' => 'application/octet-stream'
];
$credentials_upload_body = file_get_contents('/path/to/file');
$credentials_upload_request = new Request('POST', 'https://api.pdfrest.com/upload', $credentials_upload_headers, $credentials_upload_body);
$credentials_upload_res = $upload_client->sendAsync($credentials_upload_request)->wait();
echo $credentials_upload_res->getBody() . PHP_EOL;

$credentials_upload_response_json = json_decode($credentials_upload_res->getBody());

$uploaded_credentials_id = $credentials_upload_response_json->{'files'}[0]->{'id'};

echo "Successfully uploaded with an id of: " . $uploaded_credentials_id . PHP_EOL;

$passphrase_upload_headers = [
  'api-key' => 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'content-filename' => 'filename.txt',
  'Content-Type' => 'application/octet-stream'
];
$passphrase_upload_body = file_get_contents('/path/to/file');
$passphrase_upload_request = new Request('POST', 'https://api.pdfrest.com/upload', $passphrase_upload_headers, $passphrase_upload_body);
$passphrase_upload_res = $upload_client->sendAsync($passphrase_upload_request)->wait();
echo $passphrase_upload_res->getBody() . PHP_EOL;

$passphrase_upload_response_json = json_decode($passphrase_upload_res->getBody());

$uploaded_passphrase_id = $passphrase_upload_response_json->{'files'}[0]->{'id'};

echo "Successfully uploaded with an id of: " . $uploaded_passphrase_id . PHP_EOL;

$logo_upload_headers = [
  'api-key' => 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'content-filename' => 'filename.png',
  'Content-Type' => 'application/octet-stream'
];
$logo_upload_body = file_get_contents('/path/to/file');
$logo_upload_request = new Request('POST', 'https://api.pdfrest.com/upload', $logo_upload_headers, $logo_upload_body);
$logo_upload_res = $upload_client->sendAsync($logo_upload_request)->wait();
echo $logo_upload_res->getBody() . PHP_EOL;

$logo_upload_response_json = json_decode($logo_upload_res->getBody());

$uploaded_logo_id = $logo_upload_response_json->{'files'}[0]->{'id'};

echo "Successfully uploaded with an id of: " . $uploaded_logo_id . PHP_EOL;

$signing_client = new Client(['http_errors' => false]);
$signature_config = '{\"type\": \"new\",\"name\": \"esignature\",\"logo_opacity\": \"0.25\",\"location\": {\"bottom_left\": { \"x\": \"0\", \"y\": \"0\" },\"top_right\": { \"x\": \"216\", \"y\": \"72\" },\"page\": 1},\"display\": {\"include_distinguished_name\": \"true\",\"include_datetime\": \"true\",\"contact\": \"My contact info\",\"location\": \"My location\",\"name\": \"John Doe\",\"reason\": \"My reason for signing\"}}';
$signing_headers = [
  'api-key' => 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'Content-Type' => 'application/json'
];
$signing_body = '{"id":"'.$uploaded_input_id.'", "pfx_credential_id":"'.$uploaded_credentials_id.'", "pfx_passphrase_id":"'.$uploaded_passphrase_id.'", "logo_id":"'.$uploaded_logo_id.'", "signature_configuration":"'.$signature_config.'"}';
$signing_request = new Request('POST', 'https://api.pdfrest.com/signed-pdf', $signing_headers, $signing_body);
$signing_res = $signing_client->sendAsync($signing_request)->wait();
echo $signing_res->getBody() . PHP_EOL;

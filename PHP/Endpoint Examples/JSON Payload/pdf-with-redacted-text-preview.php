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

$redact_text_client = new Client(['http_errors' => false]);
$redaction_options = '[{\"type\":\"preset\",\"value\":\"uuid\"},{\"type\":\"regex\",\"value\":\"(\\\\+\\\\d{1,2}\\\\s)?\\\\(?\\\\d{3}\\\\)?[\\\\s.-]\\\\d{3}[\\\\s.-]\\\\d{4}\"},{\"type\":\"literal\",\"value\":\"word\"}]';
$redact_text_headers = [
  'api-key' => 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'Content-Type' => 'application/json'
];
$redact_text_body = '{"id":"'.$uploaded_id.'", "redactions":"'.$redaction_options.'"}';
$redact_text_request = new Request('POST', 'https://api.pdfrest.com/pdf-with-redacted-text-preview', $redact_text_headers, $redact_text_body);
$redact_text_res = $redact_text_client->sendAsync($redact_text_request)->wait();
echo $redact_text_res->getBody() . PHP_EOL;

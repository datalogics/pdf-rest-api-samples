<?php
require 'vendor/autoload.php'; // Require the autoload file to load Guzzle HTTP client.

use GuzzleHttp\Client; // Import the Guzzle HTTP client namespace.
use GuzzleHttp\Psr7\Request; // Import the PSR-7 Request class.
use GuzzleHttp\Psr7\Utils; // Import the PSR-7 Utils class for working with streams.

// Toggle deletion of sensitive files (default: false)
$DELETE_SENSITIVE_FILES = false;

$upload_client = new Client(['http_errors' => false]);
$upload_headers = [
  'api-key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
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

$decrypt_client = new Client(['http_errors' => false]);
$decrypt_headers = [
  'api-key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'Content-Type' => 'application/json'
];
$decrypt_body = '{"id":"'.$uploaded_id.'", "current_open_password": "password"}';
$decrypt_request = new Request('POST', 'https://api.pdfrest.com/decrypted-pdf', $decrypt_headers, $decrypt_body);
$decrypt_res = $decrypt_client->sendAsync($decrypt_request)->wait();
$decrypt_body = (string) $decrypt_res->getBody();
echo $decrypt_body . PHP_EOL;

// All files uploaded or generated are automatically deleted based on the 
// File Retention Period as shown on https://pdfrest.com/pricing. 
// For immediate deletion of files, particularly when sensitive data 
// is involved, an explicit delete call can be made to the API.
//
// Deletes all files in the workflow, including outputs. Save all desired files before enabling this step.

if ($DELETE_SENSITIVE_FILES) {
  $delete_client = new Client(['http_errors' => false]);
  $delete_headers = [
    'api-key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
    'Content-Type' => 'application/json'
  ];
  $decrypt_json = json_decode($decrypt_body, true);
  $delete_body = '{"ids":"'.$uploaded_id.', '.$decrypt_json['outputId'].'"}';
  $delete_request = new Request('POST', 'https://api.pdfrest.com/delete', $delete_headers, $delete_body);
  $delete_res = $delete_client->sendAsync($delete_request)->wait();
  echo $delete_res->getBody() . PHP_EOL;
}

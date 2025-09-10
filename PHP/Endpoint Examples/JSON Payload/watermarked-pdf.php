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

// Toggle deletion of sensitive files (default: false)
$DELETE_SENSITIVE_FILES = false;

$upload_client = new Client(['http_errors' => false]);
$upload_headers = [
  'api-key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'content-filename' => 'filename.pdf',
  'Content-Type' => 'application/octet-stream'
];
$upload_body = file_get_contents('/path/to/file');
$upload_request = new Request('POST', $apiUrl.'/upload', $upload_headers, $upload_body);
$upload_res = $upload_client->sendAsync($upload_request)->wait();
echo $upload_res->getBody() . PHP_EOL;

$upload_response_json = json_decode($upload_res->getBody());

$uploaded_id = $upload_response_json->{'files'}[0]->{'id'};

echo "Successfully uploaded with an id of: " . $uploaded_id . PHP_EOL;

$watermark_client = new Client(['http_errors' => false]);
$watermark_headers = [
  'api-key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'Content-Type' => 'application/json'
];
$watermark_body = '{"id":"'.$uploaded_id.'", "watermark_text":"TEXT"}';
$watermark_request = new Request('POST', $apiUrl.'/watermarked-pdf', $watermark_headers, $watermark_body);
$watermark_res = $watermark_client->sendAsync($watermark_request)->wait();
$watermark_body_str = (string)$watermark_res->getBody();
echo $watermark_body_str . PHP_EOL;
$watermark_response_json = json_decode($watermark_body_str);
$output_id = $watermark_response_json->{'outputId'} ?? '';

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
  $delete_body = '{"ids":"' . $uploaded_id . ', ' . $output_id . '"}';
  $delete_request = new Request('POST', $apiUrl.'/delete', $delete_headers, $delete_body);
  $delete_res = $delete_client->sendAsync($delete_request)->wait();
  echo $delete_res->getBody() . PHP_EOL;
}

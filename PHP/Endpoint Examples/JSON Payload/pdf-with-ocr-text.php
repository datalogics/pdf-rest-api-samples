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

$pdf_with_ocr_text_client = new Client(['http_errors' => false]);
$pdf_with_ocr_text_headers = [
  'api-key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'Content-Type' => 'application/json'
];
$pdf_with_ocr_text_body = '{"id":"'.$uploaded_id.'"}';
$pdf_with_ocr_text_request = new Request('POST', $apiUrl.'/pdf-with-ocr-text', $pdf_with_ocr_text_headers, $pdf_with_ocr_text_body);
$pdf_with_ocr_text_res = $pdf_with_ocr_text_client->sendAsync($pdf_with_ocr_text_request)->wait();
echo $pdf_with_ocr_text_res->getBody() . PHP_EOL;

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

$upload_pdf_client = new Client(['http_errors' => false]);
$upload_pdf_headers = [
  'api-key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'content-filename' => 'filename.pdf',
  'Content-Type' => 'application/octet-stream'
];
$upload_pdf_body = file_get_contents('/path/to/pdf_file');
$upload_pdf_request = new Request('POST', $apiUrl.'/upload', $upload_pdf_headers, $upload_pdf_body);
$upload_pdf_res = $upload_pdf_client->sendAsync($upload_pdf_request)->wait();
echo $upload_pdf_res->getBody() . PHP_EOL;

$upload_pdf_response_json = json_decode($upload_pdf_res->getBody());

$uploaded_pdf_id = $upload_pdf_response_json->{'files'}[0]->{'id'};

echo "PDF file successfully uploaded with an id of: " . $uploaded_pdf_id . PHP_EOL;


$upload_image_client = new Client(['http_errors' => false]);
$upload_image_headers = [
  'api-key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'content-filename' => 'image.png',
  'Content-Type' => 'application/octet-stream'
];
$upload_image_body = file_get_contents('/path/to/image_file);
$upload_image_request = new Request('POST', $apiUrl.'/upload', $upload_image_headers, $upload_image_body);
$upload_image_res = $upload_image_client->sendAsync($upload_image_request)->wait();
echo $upload_image_res->getBody() . PHP_EOL;

$upload_image_response_json = json_decode($upload_image_res->getBody());

$uploaded_image_id = $upload_image_response_json->{'files'}[0]->{'id'};

echo "Image file successfully uploaded with an id of: " . $uploaded_image_id . PHP_EOL;



$add_image_client = new Client(['http_errors' => false]);
$add_image_headers = [
  'api-key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'Content-Type' => 'application/json'
];
$add_image_body = '{"id":"'.$uploaded_pdf_id.'", "image_id": "'.$uploaded_image_id.'", "x":0, "y":0, "page":1}';
$add_image_request = new Request('POST', $apiUrl.'/pdf-with-added-image', $add_image_headers, $add_image_body);
$add_image_res = $add_image_client->sendAsync($add_image_request)->wait();
echo $add_image_res->getBody() . PHP_EOL;

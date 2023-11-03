<?php
require 'vendor/autoload.php'; // Require the autoload file to load Guzzle HTTP client.

use GuzzleHttp\Client; // Import the Guzzle HTTP client namespace.
use GuzzleHttp\Psr7\Request; // Import the PSR-7 Request class.
use GuzzleHttp\Psr7\Utils; // Import the PSR-7 Utils class for working with streams.

$upload_pdf_client = new Client(['http_errors' => false]);
$upload_pdf_headers = [
  'api-key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'content-filename' => 'pdf_filename.pdf',
  'Content-Type' => 'application/octet-stream'
];
$upload_pdf_body = file_get_contents('/path/to/pdf_file');
$upload_pdf_request = new Request('POST', 'https://api.pdfrest.com/upload', $upload_pdf_headers, $upload_pdf_body);
$upload_pdf_res = $upload_pdf_client->sendAsync($upload_pdf_request)->wait();
echo $upload_pdf_res->getBody() . PHP_EOL;

$upload_pdf_response_json = json_decode($upload_pdf_res->getBody());

$uploaded_pdf_id = $upload_pdf_response_json->{'files'}[0]->{'id'};

echo "PDF file successfully uploaded with an id of: " . $uploaded_pdf_id . PHP_EOL;


$upload_attachment_client = new Client(['http_errors' => false]);
$upload_attachment_headers = [
  'api-key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'content-filename' => 'attachment_filename.pdf',
  'Content-Type' => 'application/octet-stream'
];
$upload_attachment_body = file_get_contents('/path/to/attachment_file');
$upload_attachment_request = new Request('POST', 'https://api.pdfrest.com/upload', $upload_attachment_headers, $upload_attachment_body);
$upload_attachment_res = $upload_attachment_client->sendAsync($upload_attachment_request)->wait();
echo $upload_attachment_res->getBody() . PHP_EOL;

$upload_attachment_response_json = json_decode($upload_attachment_res->getBody());

$uploaded_attachment_id = $upload_attachment_response_json->{'files'}[0]->{'id'};

echo "Attachment file successfully uploaded with an id of: " . $uploaded_attachment_id . PHP_EOL;



$add_attachment_client = new Client(['http_errors' => false]);
$add_attachment_headers = [
  'api-key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'Content-Type' => 'application/json'
];
$add_attachment_body = '{"id":"'.$uploaded_pdf_id.'", "id_to_attach": "'.$uploaded_attachment_id.'"}';
$add_attachment_request = new Request('POST', 'https://api.pdfrest.com/pdf-with-added-attachment', $add_attachment_headers, $add_attachment_body);
$add_attachment_res = $add_attachment_client->sendAsync($add_attachment_request)->wait();
echo $add_attachment_res->getBody() . PHP_EOL;

<?php
require 'vendor/autoload.php'; // Require the autoload file to load Guzzle HTTP client.

use GuzzleHttp\Client; // Import the Guzzle HTTP client namespace.
use GuzzleHttp\Psr7\Request; // Import the PSR-7 Request class.
use GuzzleHttp\Psr7\Utils; // Import the PSR-7 Utils class for working with streams.

$upload_pdf_client = new Client(['http_errors' => false]);
$upload_pdf_headers = [
  'api-key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'content-filename' => 'filename.pdf',
  'Content-Type' => 'application/octet-stream'
];
$upload_pdf_body = file_get_contents('/path/to/pdf_file');
$upload_pdf_request = new Request('POST', 'https://api.pdfrest.com/upload', $upload_pdf_headers, $upload_pdf_body);
$upload_pdf_res = $upload_pdf_client->sendAsync($upload_pdf_request)->wait();
echo $upload_pdf_res->getBody() . PHP_EOL;

$upload_pdf_response_json = json_decode($upload_pdf_res->getBody());

$uploaded_pdf_id = $upload_pdf_response_json->{'files'}[0]->{'id'};

echo "PDF file successfully uploaded with an id of: " . $uploaded_pdf_id . PHP_EOL;


$upload_data_client = new Client(['http_errors' => false]);
$upload_data_headers = [
  'api-key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'content-filename' => 'filename.xml',
  'Content-Type' => 'application/octet-stream'
];
$upload_data_body = file_get_contents('/path/to/data_file');
$upload_data_request = new Request('POST', 'https://api.pdfrest.com/upload', $upload_data_headers, $upload_data_body);
$upload_data_res = $upload_data_client->sendAsync($upload_data_request)->wait();
echo $upload_data_res->getBody() . PHP_EOL;

$upload_data_response_json = json_decode($upload_data_res->getBody());

$uploaded_data_id = $upload_data_response_json->{'files'}[0]->{'id'};

echo "data file successfully uploaded with an id of: " . $uploaded_data_id . PHP_EOL;



$import_data_client = new Client(['http_errors' => false]);
$import_data_headers = [
  'api-key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'Content-Type' => 'application/json'
];
$import_data_body = '{"id":"'.$uploaded_pdf_id.'", "data_file_id": "'.$uploaded_data_id.'"}';
$import_data_request = new Request('POST', 'https://api.pdfrest.com/pdf-with-imported-form-data', $import_data_headers, $import_data_body);
$import_data_res = $import_data_client->sendAsync($import_data_request)->wait();
echo $import_data_res->getBody() . PHP_EOL;

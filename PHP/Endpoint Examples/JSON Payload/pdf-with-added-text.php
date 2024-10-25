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

$add_text_client = new Client(['http_errors' => false]);
$text_options = '[{\"font\":\"Times New Roman\",\"max_width\":\"175\",\"opacity\":\"1\",\"page\":\"1\",\"rotation\":\"0\",\"text\":\"sample text in PDF\",\"text_color_rgb\":\"0,0,0\",\"text_size\":\"30\",\"x\":\"72\",\"y\":\"144\"}]';
$add_text_headers = [
  'api-key' => 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx',
  'Content-Type' => 'application/json'
];
$add_text_body = '{"id":"'.$uploaded_id.'", "text_objects":"'.$text_options.'"}';
$add_text_request = new Request('POST', 'https://api.pdfrest.com/pdf-with-added-text', $add_text_headers, $add_text_body);
$add_text_res = $add_text_client->sendAsync($add_text_request)->wait();
echo $add_text_res->getBody() . PHP_EOL;

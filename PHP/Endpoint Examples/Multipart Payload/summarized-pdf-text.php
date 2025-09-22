<?php
require 'vendor/autoload.php';

use GuzzleHttp\Client;
use GuzzleHttp\Psr7\Request;
use GuzzleHttp\Psr7\Utils;

// By default, we use the US-based API service. This is the primary endpoint for global use.
$apiUrl = "https://api.pdfrest.com";

/* For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
 * For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 */
//$apiUrl = "https://eu-api.pdfrest.com";

$client = new Client();

$headers = [
  'Api-Key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'
];

$options = [
  'multipart' => [
    [
      'name' => 'file',
      'contents' => Utils::tryFopen('/path/to/file', 'r'),
      'filename' => 'filename.pdf',
      'headers' => [
        'Content-Type' => 'application/pdf'
      ]
    ],
    [
      'name' => 'target_word_count',
      'contents' => '100'
    ]
  ]
];

$request = new Request('POST', $apiUrl.'/summarized-pdf-text', $headers);
$res = $client->sendAsync($request, $options)->wait();
echo $res->getBody();


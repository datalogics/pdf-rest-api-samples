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
      // Translates text to American English. Format the output_language as a 2-3 character ISO 639 code, optionally with a region/script (e.g., 'en', 'es', 'zh-Hant', 'eng-US').
      'name' => 'output_language',
      'contents' => 'en-US'
    ]
  ]
];

$request = new Request('POST', $apiUrl.'/translated-pdf-text', $headers);
$res = $client->sendAsync($request, $options)->wait();
echo $res->getBody();


<?php

require 'vendor/autoload.php';

use GuzzleHttp\Client;
use GuzzleHttp\Psr7\Request;
use GuzzleHttp\Psr7\Utils;

/*
* This sample demonstrates the workflow from unredacted document to fully
* redacted document. The output file from the preview tool is immediately
* forwarded to the finalization stage. We recommend inspecting the output from
* the preview stage before utilizing this workflow to ensure that content is
* redacted as intended.
*/


$client = new Client();

$headers = [
  'Api-Key' => 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' // Set the API key in the headers for authentication.
];

$redactionPreviewOptions = [
  'multipart' => [
    [
      'name' => 'file',
      'contents' => Utils::tryFopen('/path/to/file.pdf', 'r'),
      'filename' => 'file.pdf',
      'headers' => [
        'Content-Type' => '<Content-type header>'
      ]
    ],
    [
        'name' => 'redactions',
        'contents' => '[{"type":"regex","value":"[Tt]he"}]'
    ]
  ]
];

$redactionPreviewRequest = new Request('POST', 'https://api.pdfrest.com/pdf-with-redacted-text-preview', $headers);

$redactionPreviewResponse = $client->sendAsync($redactionPreviewRequest, $redactionPreviewOptions)->wait();

$redactionPreviewedFileID = json_decode($redactionPreviewResponse->getBody())->{'outputId'};



$redactionAppliedOptions = [
  'multipart' => [
    [
      'name' => 'id',
      'contents' => $redactionPreviewedFileID
    ],
    [
      'name' => 'output',
      'contents' => 'pdfrest_redactionApplied'
    ]
  ]
];

$redactionAppliedRequest = new Request('POST', 'https://api.pdfrest.com/pdf-with-redacted-text-applied', $headers);

$redactionAppliedResponse = $client->sendAsync($redactionAppliedRequest, $redactionAppliedOptions)->wait();

echo $redactionAppliedResponse->getBody();

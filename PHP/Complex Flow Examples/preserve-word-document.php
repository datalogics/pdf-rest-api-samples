<?php

require 'vendor/autoload.php';

use GuzzleHttp\Client;
use GuzzleHttp\Psr7\Request;
use GuzzleHttp\Psr7\Utils;

/* In this sample, we will show how to optimize a Word file for long-term preservation
* as discussed in https://pdfrest.com/solutions/optimize-word-excel-and-powerpoint-files-for-long-term-preservation/
* We will take our Word (or Excel or PowerPoint) document and first convert it to
* a PDF with a call to the /pdf route. Then, we will take that converted PDF
* and convert it to the PDF/A format for long-term storage.
*/

// By default, we use the US-based API service. This is the primary endpoint for global use.
$apiUrl = "https://api.pdfrest.com";

/* For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
 * For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 */
//$apiUrl = "https://eu-api.pdfrest.com";


$client = new Client();

$headers = [
  'Api-Key' => 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' // Set the API key in the headers for authentication.
];

$wordToPDFOptions = [
  'multipart' => [
    [
      'name' => 'file',
      'contents' => Utils::tryFopen('/path/to/word.doc', 'r'),
      'filename' => 'word.doc',
      'headers' => [
        'Content-Type' => '<Content-type header>'
      ]
    ]
  ]
];

$wordToPDFRequest = new Request('POST', $apiUrl.'/pdf', $headers);

$wordToPDFResponse = $client->sendAsync($wordToPDFRequest, $wordToPDFOptions)->wait();

$wordToPDFedFileID = json_decode($wordToPDFResponse->getBody())->{'outputId'};



$pdfaOptions = [
  'multipart' => [
    [
      'name' => 'id',
      'contents' => $wordToPDFedFileID
    ],
    [
      'name' => 'output_type',
      'contents' => 'PDF/A-3b'
    ],
    [
      'name' => 'output',
      'contents' => 'pdfrest_pdfa'
    ]
  ]
];

$pdfaRequest = new Request('POST', $apiUrl.'/pdfa', $headers);

$pdfaResponse = $client->sendAsync($pdfaRequest, $pdfaOptions)->wait();

echo $pdfaResponse->getBody();

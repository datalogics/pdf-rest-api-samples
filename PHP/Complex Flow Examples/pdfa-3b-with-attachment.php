<?php

require 'vendor/autoload.php';

use GuzzleHttp\Client;
use GuzzleHttp\Psr7\Request;
use GuzzleHttp\Psr7\Utils;

/* In this sample we will show how attach an xml document to a PDF file and then
* convert the file with the attachment to conform to the PDF/A standard which
* can be useful for invoicing and standards compliance. We will be running the
*input document through /pdf-with-added-attachment to add the attachment and
* then /pdfa to do the PDF/A conversion.

* Note that there is nothing special about attaching an xml file, and any approprite
* file may be attached and wrapped into the PDF/A conversion.
*/


$client = new Client();

$headers = [
  'Api-Key' => 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' // Set the API key in the headers for authentication.
];

$attachOptions = [
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
      'name' => 'file_to_attach',
      'contents' => Utils::tryFopen('/path/to/file.xml', 'r'),
      'filename' => 'file.xml',
      'headers' => [
        'Content-Type' => '<Content-type header>'
      ]
    ]
  ]
];

$attachRequest = new Request('POST', 'https://api.pdfrest.com/pdf-with-added-attachment', $headers);

$attachResponse = $client->sendAsync($attachRequest, $attachOptions)->wait();

$attachedFileID = json_decode($attachResponse->getBody())->{'outputId'};



$pdfaOptions = [
  'multipart' => [
    [
      'name' => 'id',
      'contents' => $attachedFileID
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

$pdfaRequest = new Request('POST', 'https://api.pdfrest.com/pdfa', $headers);

$pdfaResponse = $client->sendAsync($pdfaRequest, $pdfaOptions)->wait();

echo $pdfaResponse->getBody();

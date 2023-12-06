<?php

require 'vendor/autoload.php';

use GuzzleHttp\Client;
use GuzzleHttp\Psr7\Request;
use GuzzleHttp\Psr7\Utils;

/* In this sample we will show how to to watermark a PDF document and then restrict
* editing on the document so that the watermark cannot be removed as discussed in
* https://pdfrest.com/solutions/add-pdf-watermarks-that-cannot-be-removed/.
* We will be running the input file through watermarked-pdf to apply the watermark
* and then /restricted-pdf to lock the watermark in.
*/

$client = new Client();

$headers = [
  'Api-Key' => 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' // Set the API key in the headers for authentication.
];

$watermarkOptions = [
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
      'name' => 'watermark_text',
      'contents' => 'TEXT'
    ]
  ]
];

$watermarkRequest = new Request('POST', 'https://api.pdfrest.com/watermarked-pdf', $headers);

$watermarkResponse = $client->sendAsync($watermarkRequest, $watermarkOptions)->wait();

$watermarkID = json_decode($watermarkResponse->getBody())->{'outputId'};


$restrictOptions = [
  'multipart' => [
    [
      'name' => 'id', // Specify the field name for the file.
      'contents' => $watermarkID
    ],
    [
      'name' => 'output', // Specify the field name for the output option.
      'contents' => 'pdfrest_restricted_pdf' // Set the value for the output option to generate a restricted PDF.
    ],
    [
      'name' => 'new_permissions_password', // Specify the field name for the new_permissions_password option.
      'contents' => 'new_example_pw' // Set the value for the new_permissions_password option to 'new_example_pw'.
    ],
    [
      'name' => 'restrictions[]', // Specify the field name for the restrictions option as an array.
      'contents' => 'edit_annotations' // Add the 'print_low' restriction to the restrictions array.
    ],
    [
      'name' => 'restrictions[]', // Specify the field name for the restrictions option as an array.
      'contents' => 'copy_content' // Add the 'accessibility_off' restriction to the restrictions array.
    ],
    [
      'name' => 'restrictions[]', // Specify the field name for the restrictions option as an array.
      'contents' => 'edit_content' // Add the 'edit_content' restriction to the restrictions array.
    ]
  ]
];

$restrictRequest = new Request('POST', 'https://api.pdfrest.com/restricted-pdf', $headers);

$restrictResponse = $client->sendAsync($restrictRequest, $restrictOptions)->wait();

echo $restrictResponse->getBody();

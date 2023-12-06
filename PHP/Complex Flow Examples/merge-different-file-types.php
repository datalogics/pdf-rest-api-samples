<?php

require 'vendor/autoload.php';

use GuzzleHttp\Client;
use GuzzleHttp\Psr7\Request;
use GuzzleHttp\Psr7\Utils;

/* In this sample we will show how to merge different file types together as
* discussed in https://pdfrest.com/solutions/merge-multiple-types-of-files-together/.
* Specifically we will be uploadng an image file to the /pdf route and capturing
* the output ID, uploading a powerpoint file to the /pdf route and capturing the
* output ID and then passing both of those IDs to the /merged-pdf route to get
* a singular output PDF combining the two inputs

* Note that there is nothing special about an image and a powepoint file and
* this sample could be easily used to convert and combine any two file types
* that the /pdf route takes as inputs
*/


$client = new Client();

$headers = [
  'Api-Key' => 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' // Set the API key in the headers for authentication.
];

$imageToPDFOptions = [
  'multipart' => [
    [
      'name' => 'file',
      'contents' => Utils::tryFopen('/path/to/image.png', 'r'),
      'filename' => 'image.png',
      'headers' => [
        'Content-Type' => '<Content-type header>'
      ]
    ]
  ]
];

$imageToPDFRequest = new Request('POST', 'https://api.pdfrest.com/pdf', $headers);

$imageToPDFResponse = $client->sendAsync($imageToPDFRequest, $imageToPDFOptions)->wait();

$convertedImageID = json_decode($imageToPDFResponse->getBody())->{'outputId'};



$powerpointToPDFOptions = [
  'multipart' => [
    [
      'name' => 'file',
      'contents' => Utils::tryFopen('/path/to/powerpoint.ppt', 'r'),
      'filename' => 'powerpoint.ppt',
      'headers' => [
        'Content-Type' => '<Content-type header>'
      ]
    ]
  ]
];

$powerpointToPDFRequest = new Request('POST', 'https://api.pdfrest.com/pdf', $headers);

$powerpointToPDFResponse = $client->sendAsync($powerpointToPDFRequest, $powerpointToPDFOptions)->wait();

$convertedPowerpointID = json_decode($powerpointToPDFResponse->getBody())->{'outputId'};


$mergeOptions = [
  'multipart' => [
    [
      'name' => 'id[]',
      'contents' => $convertedImageID
    ],
    [
      'name' => 'pages[]',
      'contents' => '1-last'
    ],
    [
      'name' => 'type[]',
      'contents' => 'id'
    ],
    [
      'name' => 'id[]',
      'contents' => $convertedPowerpointID

    ],
    [
      'name' => 'pages[]',
      'contents' => '1-last'
    ],
    [
      'name' => 'type[]',
      'contents' => 'id'
    ]
  ]
];

$mergeRequest = new Request('POST', 'https://api.pdfrest.com/merged-pdf', $headers);

$mergeResponse = $client->sendAsync($mergeRequest, $mergeOptions)->wait();

echo $mergeResponse->getBody();

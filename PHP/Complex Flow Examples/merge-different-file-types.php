<?php

require 'vendor/autoload.php';

use GuzzleHttp\Client;
use GuzzleHttp\Psr7\Request;
use GuzzleHttp\Psr7\Utils;

/* In this sample, we will show how to merge different file types together as
* discussed in https://pdfrest.com/solutions/merge-multiple-types-of-files-together/.
* First, we will upload an image file to the /pdf route and capture the output ID.
* Next, we will upload a PowerPoint file to the /pdf route and capture its output
* ID. Finally, we will pass both IDs to the /merged-pdf route to combine both inputs
* into a single PDF.
*
* Note that there is nothing special about an image and a PowerPoint file, and
* this sample could be easily used to convert and combine any two file types
* that the /pdf route takes as inputs.
*/

// By default, we use the US-based API service. This is the primary endpoint for global use.
$apiUrl = "https://api.pdfrest.com";

/* For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
 * For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 */
//$apiUrl "https://eu-api.pdfrest.com";

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

$imageToPDFRequest = new Request('POST', $apiUrl.'/pdf', $headers);

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

$powerpointToPDFRequest = new Request('POST', $apiUrl.'/pdf', $headers);

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

$mergeRequest = new Request('POST', $apiUrl.'/merged-pdf', $headers);

$mergeResponse = $client->sendAsync($mergeRequest, $mergeOptions)->wait();

echo $mergeResponse->getBody();

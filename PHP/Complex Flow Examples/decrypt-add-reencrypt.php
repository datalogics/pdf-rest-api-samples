<?php

require 'vendor/autoload.php';

use GuzzleHttp\Client;
use GuzzleHttp\Psr7\Request;
use GuzzleHttp\Psr7\Utils;

/* In this sample, we will show how to take an encrypted file and decrypt, modify
* and re-encrypt it to create an encryption-at-rest solution as discussed in
* https://pdfrest.com/solutions/create-secure-document-workflows-with-pdf-password-protection/
* We will be running the document through /decrypted-pdf to open the document
* to modification, running the decrypted result through /pdf-with-added-image,
* and then sending the output with the new image through /encrypted-pdf to
* lock it up again.
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

$decryptOptions = [
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
      'name' => 'current_open_password',
      'contents' => 'current_example_pw'
    ]
  ]
];

$decryptRequest = new Request('POST', $apiUrl.'/decrypted-pdf', $headers);

$decryptResponse = $client->sendAsync($decryptRequest, $decryptOptions)->wait();

$decryptID = json_decode($decryptResponse->getBody())->{'outputId'};


$addImageOptions = [
  'multipart' => [
    [
      'name' => 'id',
      'contents' => $decryptID
    ],
    [
      'name' => 'image_file',
      'contents' => Utils::tryFopen('/path/to/file.jpg', 'r'),
      'filename' => 'file.jpg',
      'headers' => [
        'Content-Type' => '<Content-type header>'
      ]
    ],
    [
      'name' => 'x',
      'contents' => '72'
    ],
    [
      'name' => 'y',
      'contents' => '72'
    ],
    [
      'name' => 'page',
      'contents' => '1'
    ],
    [
      'name' => 'output',
      'contents' => 'pdfrest_pdf_with_added_image'
    ]
  ]
];

$addImageRequest = new Request('POST', $apiUrl.'/pdf-with-added-image', $headers);

$addImageResponse = $client->sendAsync($addImageRequest, $addImageOptions)->wait();

$addImageID = json_decode($addImageResponse->getBody())->{'outputId'};




$encryptOptions = [
  'multipart' => [
    [
      'name' => 'id',
      'contents' => $addImageID,
    ],
    [
      'name' => 'new_open_password',
      'contents' => 'new_example_pw'
    ],
    [
      'name' => 'output',
      'contents' => 'pdfrest_encrypted_pdf'
    ]
  ]
];

$encryptRequest = new Request('POST', $apiUrl.'/encrypted-pdf', $headers);

$encryptResponse = $client->sendAsync($encryptRequest, $encryptOptions)->wait();

echo $encryptResponse->getBody();

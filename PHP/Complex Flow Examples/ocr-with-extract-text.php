<?php

require 'vendor/autoload.php';

use GuzzleHttp\Client;
use GuzzleHttp\Psr7\Request;
use GuzzleHttp\Psr7\Utils;

/* In this sample, we will show how to convert a scanned document into a PDF with
 * searchable and extractable text using Optical Character Recognition (OCR), and then
 * extract that text from the newly created document.
 *
 * First, we will upload a scanned PDF to the /pdf-with-ocr-text route and capture the
 * output ID. Then, we will send the output ID to the /extracted-text route, which will
 * return the newly added text.
 */

 // By default, we use the US-based API service. This is the primary endpoint for global use.
 $apiUrl = "https://api.pdfrest.com";

 /* For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
  * For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
  */
 //$apiUrl "https://eu-api.pdfrest.com";

$client = new Client();

$headers = [
  'Api-Key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' // Replace with your API key
];

// Upload PDF for OCR
$pdfToOCROptions = [
  'multipart' => [
    [
      'name' => 'file',
      'contents' => Utils::tryFopen('/path/to/file.pdf', 'r'),
      'filename' => 'file.pdf',
      'headers' => [
        'Content-Type' => 'application/pdf'
      ]
    ],
    [
      'name' => 'output',
      'contents' => 'example_pdf-with-ocr-text_out'
    ]
  ]
];

$pdfToOCRRequest = new Request('POST', $apiUrl.'/pdf-with-ocr-text', $headers);

echo "Sending POST request to OCR endpoint...\n";
$pdfToOCRResponse = $client->sendAsync($pdfToOCRRequest, $pdfToOCROptions)->wait();

echo "Response status code: " . $pdfToOCRResponse->getStatusCode() . "\n";

$ocrPDFID = json_decode($pdfToOCRResponse->getBody())->outputId;
echo "Got the output ID: " . $ocrPDFID . "\n";

// Extract text from OCR'd PDF
$extractTextOptions = [
  'multipart' => [
    [
      'name' => 'id',
      'contents' => $ocrPDFID
    ]
  ]
];

$extractTextRequest = new Request('POST', $apiUrl.'/extracted-text', $headers);

echo "Sending POST request to extract text endpoint...\n";
$extractTextResponse = $client->sendAsync($extractTextRequest, $extractTextOptions)->wait();

echo "Response status code: " . $extractTextResponse->getStatusCode() . "\n";

$fullText = json_decode($extractTextResponse->getBody())->fullText;
echo $fullText . "\n";

?>

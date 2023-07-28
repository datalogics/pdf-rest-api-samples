<?php
require 'vendor/autoload.php'; // Require the autoload file to load Guzzle HTTP client.

use GuzzleHttp\Client; // Import the Guzzle HTTP client namespace.
use GuzzleHttp\Psr7\Request; // Import the PSR-7 Request class.
use GuzzleHttp\Psr7\Utils; // Import the PSR-7 Utils class for working with streams.

$client = new Client(); // Create a new instance of the Guzzle HTTP client.

$headers = [
    'Api-Key' => 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' // Set the API key in the headers for authentication.
];

$options = [
    'multipart' => [
        [
            'name' => 'file',
            'contents' => Utils::tryFopen('/path/to/file', 'r'), // Provide the path to the PDF file to be watermarked.
            'filename' => '/path/to/file',
            'headers'  => [
                'Content-Type' => '<Content-type header>' // Specify the Content-Type header for the PDF file.
            ]
        ],
        [
            'name' => 'watermark_text',
            'contents' => 'Hello, watermarked world!' // Specify the text to be used as the watermark.
        ],
        [
            'name' => 'font',
            'contents' => 'Arial' // Specify the font to be used for the watermark text.
        ],
        [
            'name' => 'text_size',
            'contents' => '72' // Specify the font size of the watermark text.
        ],
        [
            'name' => 'text_color_rgb',
            'contents' => '255,0,0' // Specify the RGB color (red, green, blue) of the watermark text.
        ],
        [
            'name' => 'opacity',
            'contents' => '0.5' // Specify the opacity of the watermark (0.0 to 1.0).
        ],
        [
            'name' => 'x',
            'contents' => '0' // Specify the x-coordinate of the watermark's position.
        ],
        [
            'name' => 'y',
            'contents' => '0' // Specify the y-coordinate of the watermark's position.
        ],
        [
            'name' => 'rotation',
            'contents' => '0' // Specify the rotation angle of the watermark (in degrees).
        ],
        [
            'name' => 'output',
            'contents' => 'pdfrest_watermarked_pdf' // Specify the desired output format for the watermarked PDF.
        ]
    ]
];

$request = new Request('POST', 'https://api.pdfrest.com/watermarked-pdf', $headers); // Create a new HTTP POST request with the API endpoint and headers.

$res = $client->sendAsync($request, $options)->wait(); // Send the asynchronous request and wait for the response.

echo $res->getBody(); // Output the response body, which contains the watermarked PDF.
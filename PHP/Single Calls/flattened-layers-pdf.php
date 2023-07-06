<?php

// Initialize a cURL session.
$curl = curl_init();

// Set cURL options for the request.
curl_setopt_array($curl, array(
  CURLOPT_URL => 'https://api.pdfrest.com/watermarked-pdf',
  CURLOPT_RETURNTRANSFER => true,
  CURLOPT_ENCODING => '',
  CURLOPT_MAXREDIRS => 10,
  CURLOPT_TIMEOUT => 0,
  CURLOPT_FOLLOWLOCATION => true,
  CURLOPT_HTTP_VERSION => CURL_HTTP_VERSION_1_1,
  CURLOPT_CUSTOMREQUEST => 'POST',
  CURLOPT_POSTFIELDS => array(
    'file' => new CURLFILE('/path/to/file'), // Specify the path to the file
    'watermark_text' => 'Hello, watermarked world!', // Set the watermark text
    'font' => 'Arial', // Set the font for the watermark
    'text_size' => '72', // Set the text size for the watermark
    'text_color_rgb' => '255,0,0', // Set the text color (RGB) for the watermark
    'opacity' => '0.5', // Set the opacity of the watermark
    'x' => '0', // Set the x-coordinate of the watermark position
    'y' => '0', // Set the y-coordinate of the watermark position
    'rotation' => '0', // Set the rotation angle of the watermark
    'output' => 'pdfrest_watermarked_pdf' // Set the output file name
  ),
  CURLOPT_HTTPHEADER => array(
    'Api-Key: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' // Place your API key here
  ),
));

// Execute the cURL request and store the response.
$response = curl_exec($curl);

// Close the cURL session.
curl_close($curl);

// Output the response.
echo $response;

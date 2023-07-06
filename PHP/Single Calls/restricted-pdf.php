<?php

// Initialize a cURL session.
$curl = curl_init();

// Set cURL options for the request.
curl_setopt_array($curl, array(
  CURLOPT_URL => 'https://api.pdfrest.com/restricted-pdf',
  CURLOPT_RETURNTRANSFER => true,
  CURLOPT_ENCODING => '',
  CURLOPT_MAXREDIRS => 10,
  CURLOPT_TIMEOUT => 0,
  CURLOPT_FOLLOWLOCATION => true,
  CURLOPT_HTTP_VERSION => CURL_HTTP_VERSION_1_1,
  CURLOPT_CUSTOMREQUEST => 'POST',
  CURLOPT_POSTFIELDS => array(
    'file' => new CURLFILE('/path/to/file'), // Specify the path to the file
    'new_permissions_password' => 'new_example_pw', // Set the new permissions password
    'restrictions[]' => 'print_low', // Specify the print restriction
    'restrictions[]' => 'accessibility_off', // Specify the accessibility restriction
    'restrictions[]' => 'edit_content', // Specify the content editing restriction
    'output' => 'pdfrest_restricted_pdf' // Set the output file name
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

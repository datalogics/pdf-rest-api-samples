<?php

// Initialize a cURL session.
$curl = curl_init();

// Set cURL options for the request.
curl_setopt_array($curl, array(
  CURLOPT_URL => 'https://api.pdfrest.com/pdf-info',
  CURLOPT_RETURNTRANSFER => true,
  CURLOPT_ENCODING => '',
  CURLOPT_MAXREDIRS => 10,
  CURLOPT_TIMEOUT => 0,
  CURLOPT_FOLLOWLOCATION => true,
  CURLOPT_HTTP_VERSION => CURL_HTTP_VERSION_1_1,
  CURLOPT_CUSTOMREQUEST => 'POST',
  CURLOPT_POSTFIELDS => array(
    'file' => new CURLFILE('/path/to/file'), // Specify the path to the PDF file
    'queries' => 'tagged,image_only,title,subject,author,producer,creator,creation_date,modified_date,keywords,doc_language,page_count,contains_annotations,contains_signature,pdf_version,file_size,filename,restrict_permissions_set,contains_xfa,contains_acroforms,contains_javascript,contains_transparency,contains_embedded_file,uses_embedded_fonts,uses_nonembedded_fonts,pdfa,requires_password_to_open,pdfua_claim,pdfe_claim,pdfx_claim' // Set the queries for PDF information
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

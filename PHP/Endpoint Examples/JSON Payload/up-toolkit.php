<?php

// By default, we use the US-based API service. This is the primary endpoint for global use.
$up_url = "https://api.pdfrest.com/up-toolkit";

/* For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
 * For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 */
//$up_url = "https://eu-api.pdfrest.com/up-toolkit";


// Initialize a cURL session.
$ch = curl_init();

// Set the url that will be sent to the up-toolkit endpoint.
curl_setopt($ch, CURLOPT_URL, $up_url);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);

// up-forms and up-office can be used to query the other tools
print "Sending GET request to /up-toolkit endpoint...\n";
$response = curl_exec($ch);

print "Response status code: " . curl_getinfo($ch, CURLINFO_HTTP_CODE) . "\n";

if($response === false){
    print 'Error: ' . curl_error($ch) . "\n";
}else{

    print json_encode(json_decode($response), JSON_PRETTY_PRINT | JSON_UNESCAPED_SLASHES);
    print "\n";

}

curl_close($ch);
?>

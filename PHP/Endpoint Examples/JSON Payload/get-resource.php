<?php


// By default, we use the US-based API service. This is the primary endpoint for global use.
$apiUrl = "https://api.pdfrest.com";

/* For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
 * For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 */
//$apiUrl = "https://eu-api.pdfrest.com";

// Resource UUIDs can be found in the JSON response of POST requests as "outputId". Resource UUIDs usually look like this: '0950b9bdf-0465-4d3f-8ea3-d2894f1ae839'.
$id = 'xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'; // place resource uuid here

// The response format can be 'file' or 'url'.
// If 'url', then JSON containing the url of the resource file is returned.
// If 'file', then the file itself is returned.
$format = 'file';

$resource_url = "$apiUrl/resource/$id?format=$format";

// Initialize a cURL session.
$ch = curl_init();

// Set the url that will be sent to resource/{id} endpoint.
curl_setopt($ch, CURLOPT_URL, $resource_url);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);

print "Sending GET request to /resource/{id} endpoint...\n";
$response = curl_exec($ch);

print "Response status code: " . curl_getinfo($ch, CURLINFO_HTTP_CODE) . "\n";

if($response === false){
    print 'Error: ' . curl_error($ch) . "\n";
}else{
    if($format == 'file'){
        // For example, the string 'place_output_name_with_extension_here' can be replaced with the name of the output file with the proper extension.
        // If you are expecting the resource to be a PDF file, then the 'place_output_name_with_extension_here' string could be replaced with 'out.pdf'.
        // Given the example above, you will find a file named 'out.pdf' in the same folder as the sample when the sample executes successfully.
        $output_file_name = 'place_output_name_with_extension_here';

        if(file_put_contents($output_file_name, @file_get_contents($resource_url))){
            print "The file $output_file_name was created.\n";
        }else{
            print json_encode(json_decode($response), JSON_PRETTY_PRINT | JSON_UNESCAPED_SLASHES);
            print "\n";
        }
    }else{
        print json_encode(json_decode($response), JSON_PRETTY_PRINT | JSON_UNESCAPED_SLASHES);
        print "\n";
    }
}

curl_close($ch);
?>

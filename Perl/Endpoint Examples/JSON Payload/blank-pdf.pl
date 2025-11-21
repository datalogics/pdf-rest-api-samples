#!/usr/bin/env perl
use strict;
use warnings;
use utf8;
use FindBin qw($Bin);
use JSON::PP qw(encode_json);
use LWP::UserAgent;
use HTTP::Request;
use Dotenv;

#!
# What this sample does:
# - Requests a three-page blank PDF using a JSON payload.
#
# Setup (.env):
# - Copy .env.example to .env (Perl folder root)
# - Set PDFREST_API_KEY=your_api_key_here
# - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
#     PDFREST_URL=https://eu-api.pdfrest.com
#   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#
# Usage:
#   perl "Endpoint Examples/JSON Payload/blank-pdf.pl"
#
# Output:
# - Prints the API JSON response to stdout. Non-2xx responses exit non-zero.

binmode STDOUT, ':raw';
binmode STDERR, ':encoding(UTF-8)';

my $env_path = "$Bin/../../.env";
-e $env_path and Dotenv->load($env_path);

my $api_key = $ENV{PDFREST_API_KEY} // '';
if (!$api_key || $api_key =~ /^\s*$/) {
    print STDERR "Missing PDFREST_API_KEY in .env or environment\n";
    exit 1;
}

my $api_base = $ENV{PDFREST_URL} // $ENV{PDFREST_API} // 'https://api.pdfrest.com';
$api_base =~ s{/+$}{};

my $ua = LWP::UserAgent->new( timeout => 60 );

my $payload = encode_json({
    page_size  => 'letter',
    page_count => 3,
    page_orientation => 'portrait',
});

my $request = HTTP::Request->new('POST', "$api_base/blank-pdf");
$request->header('api-key' => $api_key);
$request->header('Content-Type' => 'application/json');
$request->content($payload);

my $response = $ua->request($request);
print STDOUT $response->decoded_content // '';

unless ($response->is_success) {
    print STDERR "\nblank-pdf request failed with status " . $response->code . "\n";
    exit 1;
}

exit 0;

__END__

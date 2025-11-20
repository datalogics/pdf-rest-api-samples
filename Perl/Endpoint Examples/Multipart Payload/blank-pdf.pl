#!/usr/bin/env perl
use strict;
use warnings;
use utf8;
use FindBin qw($Bin);
use LWP::UserAgent;
use HTTP::Request::Common qw(POST);
use Dotenv;

#!
# What this sample does:
# - Calls /blank-pdf with multipart/form-data to create a three-page blank PDF.
#
# Setup (.env):
# - Copy .env.example to .env (Perl folder root)
# - Set PDFREST_API_KEY=your_api_key_here
# - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
#     PDFREST_URL=https://eu-api.pdfrest.com
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#
# Usage:
#   perl "Endpoint Examples/Multipart Payload/blank-pdf.pl"
#
# Output:
# - Prints the API JSON response to stdout. Non-2xx responses exit with a concise message.

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

eval {
    my $req = POST(
        "$api_base/blank-pdf",
        'Content_Type' => 'form-data',
        'Content'      => [
            page_size  => 'letter',
            page_count => '3',
            page_orientation => 'portrait',
        ]
    );
    $req->header('api-key' => $api_key);

    my $resp = $ua->request($req);
    print STDOUT $resp->decoded_content // '';
    if (!$resp->is_success) {
        print STDERR "\nblank-pdf request failed with status " . $resp->code . "\n";
        exit 1;
    }
    1;
} or do {
    my $err = $@ || 'Unknown error';
    $err =~ s/\s+$//;
    print STDERR "Error: $err\n";
    exit 1;
};

__END__

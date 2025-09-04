#!/usr/bin/env perl
use strict;
use warnings;
use utf8;
use FindBin qw($Bin);
use File::Basename qw(basename);
use LWP::UserAgent;
use HTTP::Request::Common qw(POST);
use Dotenv;

#!
# What this sample does:
# - Converts a PDF to Markdown using pdfRest.
# - Sends a single multipart/form-data request directly to /markdown with the file.
#
# Setup (.env):
# - Copy .env.example to .env (Perl folder root)
# - Set PDFREST_API_KEY=your_api_key_here
# - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
#     PDFREST_URL=https://eu-api.pdfrest.com
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#
# Usage:
#   perl "Endpoint Examples/Multipart Payload/markdown.pl" /path/to/input.pdf
#
# Output:
# - Prints the API JSON response to stdout. Non-2xx responses exit with a concise message.
# - Tip: pipe output to a file: perl ... > response.json

binmode STDOUT, ':raw';
binmode STDERR, ':encoding(UTF-8)';

# Load .env from the Perl folder root
my $env_path = "$Bin/../../.env";
-e $env_path and Dotenv->load($env_path);

my $api_key = $ENV{PDFREST_API_KEY} // '';
if (!$api_key || $api_key =~ /^\s*$/) {
    print STDERR "Missing PDFREST_API_KEY in .env or environment\n";
    exit 1;
}

my $api_base = $ENV{PDFREST_URL} // $ENV{PDFREST_API} // 'https://api.pdfrest.com';
$api_base =~ s{/+$}{};

my $pdf_path = shift @ARGV;
if (!$pdf_path || !-f $pdf_path) {
    print STDERR "Usage: perl markdown.pl /path/to/file.pdf\n";
    exit 1;
}

my $filename = basename($pdf_path);
my $ua = LWP::UserAgent->new( timeout => 60 );

eval {
    my $req = POST("$api_base/markdown",
        'Content_Type' => 'form-data',
        'Content' => [
            file => [$pdf_path, $filename, 'Content-Type' => 'application/pdf'],
            output => 'pdfrest_markdown',
            page_break_comments => 'on',
            # Optional: page_range => '1-3',
        ]
    );
    $req->header('api-key' => $api_key);

    my $resp = $ua->request($req);
    print STDOUT $resp->decoded_content // '';
    if (!$resp->is_success) {
        print STDERR "\nMarkdown conversion failed with status " . $resp->code . "\n";
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

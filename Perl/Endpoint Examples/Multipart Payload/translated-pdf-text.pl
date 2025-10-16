#!/usr/bin/env perl
use strict;
use warnings;
use utf8;
use FindBin qw($Bin);
use File::Basename qw(basename);
use LWP::UserAgent;
use HTTP::Request::Common qw(POST);
use Dotenv;

binmode STDOUT, ':raw';
binmode STDERR, ':encoding(UTF-8)';

my $env_path = "$Bin/../../.env";
-e $env_path and Dotenv->load($env_path);

my $api_key = $ENV{PDFREST_API_KEY} // '';
if (!$api_key || $api_key =~ /^\s*$/) { print STDERR "Missing PDFREST_API_KEY\n"; exit 1; }

my $api_base = $ENV{PDFREST_URL} // $ENV{PDFREST_API} // 'https://api.pdfrest.com';
$api_base =~ s{/+$}{};

my $pdf_path = shift @ARGV;
if (!$pdf_path || !-f $pdf_path) { print STDERR "Usage: perl translated-pdf-text.pl /path/to/file.pdf\n"; exit 1; }

my $filename = basename($pdf_path);
my $ua = LWP::UserAgent->new( timeout => 60 );

eval {
    my $req = POST("$api_base/translated-pdf-text",
        'Content_Type' => 'form-data',
        'Content' => [
            file => [$pdf_path, $filename, 'Content-Type' => 'application/pdf'],
            output_language => 'en-US', # Translates text to American English. Format the output_language as a 2-3 character ISO 639 code, optionally with a region/script (e.g., 'en', 'es', 'zh-Hant', 'eng-US').
        ]
    );
    $req->header('api-key' => $api_key);
    my $resp = $ua->request($req);
    print STDOUT $resp->decoded_content // '';
    if (!$resp->is_success) { print STDERR "\nTranslate failed with status " . $resp->code . "\n"; exit 1; }
    1;
} or do { my $err = $@ || 'Unknown error'; $err =~ s/\s+$//; print STDERR "Error: $err\n"; exit 1; };

__END__


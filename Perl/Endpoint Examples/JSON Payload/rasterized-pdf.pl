#!/usr/bin/env perl
use strict;
use warnings;
use utf8;
use FindBin qw($Bin);
use File::Basename qw(basename);
use JSON::PP qw(encode_json decode_json);
use LWP::UserAgent;
use HTTP::Request;

#!
# What this sample does:
# - Creates a rasterized version of a PDF using pdfRest.
# - Uses a JSON payload in two steps: upload to /upload, then call /rasterized-pdf with the returned id.
#
# Setup (.env):
# - Copy .env.example to .env (Perl folder root)
# - Set PDFREST_API_KEY=your_api_key_here
# - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
#     PDFREST_URL=https://eu-api.pdfrest.com
#
# Usage:
#   perl "Endpoint Examples/JSON Payload/rasterized-pdf.pl" /path/to/input.pdf
#
# Output:
# - Prints the API JSON response to stdout. Non-2xx responses exit with a concise message.
# - Tip: pipe output to a file: perl ... > response.json

binmode STDOUT, ':raw';
binmode STDERR, ':encoding(UTF-8)';

# Load .env from the Perl folder root
my $env_path = "$Bin/../../.env";
if (-f $env_path) {
    open my $env_fh, '<:encoding(UTF-8)', $env_path or die "Cannot open $env_path: $!\n";
    while (my $line = <$env_fh>) {
        chomp $line;
        next if $line =~ /^\s*#/;
        next if $line !~ /\S/;
        if ($line =~ /^\s*([A-Za-z_][A-Za-z0-9_]*)\s*=\s*(.*)\s*$/) {
            my ($k, $v) = ($1, $2);
            $v =~ s/^['"]|['"]$//g;
            $ENV{$k} = $v if !exists $ENV{$k};
        }
    }
    close $env_fh;
}

my $api_key = $ENV{PDFREST_API_KEY} // '';
if (!$api_key || $api_key =~ /^\s*$/) {
    print STDERR "Missing PDFREST_API_KEY in .env or environment\n";
    exit 1;
}

my $api_base = $ENV{PDFREST_URL} // $ENV{PDFREST_API} // 'https://api.pdfrest.com';
$api_base =~ s{/+$}{};

my $pdf_path = shift @ARGV;
if (!$pdf_path || !-f $pdf_path) {
    print STDERR "Usage: perl rasterized-pdf.pl /path/to/file.pdf\n";
    exit 1;
}

my $filename = basename($pdf_path);
open my $fh, '<:raw', $pdf_path or do { print STDERR "Unable to read $pdf_path: $!\n"; exit 1; };
my $file_bytes; { local $/; $file_bytes = <$fh>; }
close $fh;

my $ua = LWP::UserAgent->new( timeout => 60 );

eval {
    # 1) Upload the PDF
    my $upload_req = HTTP::Request->new('POST', "$api_base/upload");
    $upload_req->header('api-key' => $api_key);
    $upload_req->header('content-filename' => $filename);
    $upload_req->header('Content-Type' => 'application/octet-stream');
    $upload_req->content($file_bytes);
    my $upload_resp = $ua->request($upload_req);
    print STDERR $upload_resp->decoded_content // '';
    if (!$upload_resp->is_success) {
        print STDERR "\nUpload failed with status " . $upload_resp->code . "\n";
        exit 1;
    }
    my $upload_json = decode_json($upload_resp->decoded_content // '{}');
    my $uploaded_id = $upload_json->{files} && ref $upload_json->{files} eq 'ARRAY'
        ? $upload_json->{files}[0]{id}
        : undef;
    if (!$uploaded_id) {
        print STDERR "Unexpected response format: missing files[0].id\n";
        exit 1;
    }
    print STDERR "Successfully uploaded with an id of: $uploaded_id\n";

    # 2) Call /rasterized-pdf with JSON body
    my $body = encode_json({ id => $uploaded_id });
    my $ras_req = HTTP::Request->new('POST', "$api_base/rasterized-pdf");
    $ras_req->header('api-key' => $api_key);
    $ras_req->header('Content-Type' => 'application/json');
    $ras_req->content($body);
    my $ras_resp = $ua->request($ras_req);
    print STDOUT $ras_resp->decoded_content // '';
    if (!$ras_resp->is_success) {
        print STDERR "\nRasterization failed with status " . $ras_resp->code . "\n";
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

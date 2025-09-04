#!/usr/bin/env perl
use strict;
use warnings;
use utf8;
use FindBin qw($Bin);
use File::Basename qw(basename);
use JSON::PP qw(encode_json decode_json);
use LWP::UserAgent;
use HTTP::Request;
use HTTP::Request::Common qw(POST);
use Encode qw(encode);
use Dotenv;

#!
# What this sample does:
# - Converts a PDF to Markdown using pdfRest.
# - Uses a JSON payload in two steps: upload to /upload, then call /markdown with the returned id.
#
# Setup (.env):
# - Copy .env.example to .env (Perl folder root)
# - Set PDFREST_API_KEY=your_api_key_here
# - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
#     PDFREST_URL=https://eu-api.pdfrest.com
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#
# Usage:
#   perl "Endpoint Examples/JSON Payload/markdown.pl" /path/to/input.pdf
#
# Output:
# - Prints the API JSON response to stdout. Non-2xx responses exit with a concise message.
# - Tip: pipe output to a file: perl ... > response.json

binmode STDOUT, ':raw';
binmode STDERR, ':encoding(UTF-8)';

# Load .env from the Perl folder root (two levels up from this script)
my $env_path = "$Bin/../../.env";
-e $env_path and Dotenv->load($env_path);

my $api_key = $ENV{PDFREST_API_KEY} // '';
if (!$api_key || $api_key =~ /^\s*$/) {
    print STDERR "Missing PDFREST_API_KEY in .env or environment\n";
    exit 1;
}

my $api_base = $ENV{PDFREST_URL} // $ENV{PDFREST_API} // 'https://api.pdfrest.com';
$api_base =~ s{/+$}{}; # trim trailing slashes

my $pdf_path = shift @ARGV;
if (!$pdf_path || !-f $pdf_path) {
    print STDERR "Usage: perl markdown.pl /path/to/file.pdf\n";
    exit 1;
}

my $filename = basename($pdf_path);
open my $fh, '<:raw', $pdf_path or do { print STDERR "Unable to read $pdf_path: $!\n"; exit 1; };
my $file_bytes; { local $/; $file_bytes = <$fh>; }
close $fh;

my $ua = LWP::UserAgent->new( timeout => 60 );

eval {
    # Step 1: Upload the file to receive a reusable id
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

    # Step 2: Request Markdown output using the uploaded id
    my $markdown_body = encode_json({ id => $uploaded_id, page_break_comments => 'on' });
    my $markdown_req = HTTP::Request->new('POST', "$api_base/markdown");
    $markdown_req->header('api-key' => $api_key);
    $markdown_req->header('Content-Type' => 'application/json');
    $markdown_req->content($markdown_body);

    my $markdown_resp = $ua->request($markdown_req);
    print STDOUT $markdown_resp->decoded_content // '';
    if (!$markdown_resp->is_success) {
        print STDERR "\nMarkdown conversion failed with status " . $markdown_resp->code . "\n";
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

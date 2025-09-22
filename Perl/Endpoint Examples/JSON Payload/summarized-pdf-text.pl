#!/usr/bin/env perl
use strict;
use warnings;
use utf8;
use FindBin qw($Bin);
use File::Basename qw(basename);
use JSON::PP qw(encode_json decode_json);
use LWP::UserAgent;
use HTTP::Request;
use Encode qw(encode);
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
if (!$pdf_path || !-f $pdf_path) { print STDERR "Usage: perl summarized-pdf-text.pl /path/to/file.pdf\n"; exit 1; }

my $filename = basename($pdf_path);
open my $fh, '<:raw', $pdf_path or do { print STDERR "Unable to read $pdf_path: $!\n"; exit 1; };
my $file_bytes; { local $/; $file_bytes = <$fh>; }
close $fh;

my $ua = LWP::UserAgent->new( timeout => 60 );

eval {
    # Upload
    my $upload_req = HTTP::Request->new('POST', "$api_base/upload");
    $upload_req->header('api-key' => $api_key);
    $upload_req->header('content-filename' => $filename);
    $upload_req->header('Content-Type' => 'application/octet-stream');
    $upload_req->content($file_bytes);
    my $upload_resp = $ua->request($upload_req);
    print STDERR $upload_resp->decoded_content // '';
    if (!$upload_resp->is_success) { print STDERR "\nUpload failed with status " . $upload_resp->code . "\n"; exit 1; }

    my $upload_json = decode_json($upload_resp->decoded_content // '{}');
    my $uploaded_id = $upload_json->{files} && ref $upload_json->{files} eq 'ARRAY' ? $upload_json->{files}[0]{id} : undef;
    if (!$uploaded_id) { print STDERR "Unexpected response format: missing files[0].id\n"; exit 1; }
    print STDERR "Successfully uploaded with an id of: $uploaded_id\n";

    # Summarize
    my $body = encode_json({ id => $uploaded_id, target_word_count => 100 });
    my $req = HTTP::Request->new('POST', "$api_base/summarized-pdf-text");
    $req->header('api-key' => $api_key);
    $req->header('Content-Type' => 'application/json');
    $req->content($body);
    my $resp = $ua->request($req);
    print STDOUT $resp->decoded_content // '';
    if (!$resp->is_success) { print STDERR "\nSummarize failed with status " . $resp->code . "\n"; exit 1; }
    1;
} or do { my $err = $@ || 'Unknown error'; $err =~ s/\s+$//; print STDERR "Error: $err\n"; exit 1; };

__END__


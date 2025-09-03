#!/usr/bin/env perl
use strict;
use warnings;
use utf8;
use FindBin qw($Bin);
use File::Basename qw(basename);
use JSON::PP qw(decode_json);
use LWP::UserAgent;
use HTTP::Request;
use HTTP::Request::Common qw(POST);
use URI::Escape qw(uri_escape);

#!
# What this sample does:
# - Merges multiple inputs (PDFs and non-PDFs) into a single PDF.
# - Non-PDFs are converted to PDF; PDFs are uploaded. Collected IDs are merged via /merged-pdf.
#
# Setup (.env):
# - Copy .env.example to .env (Perl folder root)
# - Set PDFREST_API_KEY=your_api_key_here
# - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
#     PDFREST_URL=https://eu-api.pdfrest.com
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#
# Usage:
#   perl "Complex Flow Examples/merge-different-file-types.pl" /path/to/file1 /path/to/file2 [/path/to/file3 ...]
#
# Output:
# - Prints the API JSON response to stdout. Non-2xx responses exit with a concise message.
# - Tip: pipe output to a file: perl ... > response.json

binmode STDOUT, ':raw';
binmode STDERR, ':encoding(UTF-8)';

# Load .env from the Perl folder root
my $env_path = "$Bin/../.env"; # one level up from Complex Flow Examples
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

my @paths = @ARGV;
if (@paths < 2) {
    print STDERR "Usage: perl merge-different-file-types.pl /path/to/file1 /path/to/file2 [/path/to/file3 ...]\n";
    exit 1;
}
for my $p (@paths) { if (!-f $p) { print STDERR "Not a file: $p\n"; exit 1; } }

sub content_type_for {
    my ($path) = @_;
    my ($ext) = $path =~ /(\.[^.]+)$/;
    $ext = lc($ext // '');
    return 'application/pdf' if $ext eq '.pdf';
    return 'image/png' if $ext eq '.png';
    return 'image/jpeg' if $ext eq '.jpg' || $ext eq '.jpeg';
    return 'image/gif' if $ext eq '.gif';
    return 'image/tiff' if $ext eq '.tif' || $ext eq '.tiff';
    return 'image/bmp' if $ext eq '.bmp';
    return 'image/webp' if $ext eq '.webp';
    return 'application/msword' if $ext eq '.doc';
    return 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' if $ext eq '.docx';
    return 'application/vnd.ms-powerpoint' if $ext eq '.ppt';
    return 'application/vnd.openxmlformats-officedocument.presentationml.presentation' if $ext eq '.pptx';
    return 'application/vnd.ms-excel' if $ext eq '.xls';
    return 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' if $ext eq '.xlsx';
    return 'text/plain' if $ext eq '.txt';
    return 'application/rtf' if $ext eq '.rtf';
    return 'text/html' if $ext eq '.html' || $ext eq '.htm';
    return 'application/octet-stream';
}

my $ua = LWP::UserAgent->new( timeout => 120 );

eval {
    my @ids;
    for my $i (0..$#paths) {
        my $p = $paths[$i];
        my $ext = lc(($p =~ /(\.[^.]+)$/)[0] // '');
        if ($ext eq '.pdf') {
            # Upload and capture id
            open my $fh, '<:raw', $p or do { print STDERR "Unable to read $p: $!\n"; exit 1; };
            my $bytes; { local $/; $bytes = <$fh>; }
            close $fh;
            my $req = HTTP::Request->new('POST', "$api_base/upload");
            $req->header('api-key' => $api_key);
            $req->header('content-filename' => basename($p));
            $req->header('Content-Type' => 'application/octet-stream');
            $req->content($bytes);
            my $resp = $ua->request($req);
            print STDERR $resp->decoded_content // '';
            if (!$resp->is_success) { print STDERR "\nUpload failed (input #" . ($i+1) . ") status " . $resp->code . "\n"; exit 1; }
            my $json = decode_json($resp->decoded_content // '{}');
            my $id = $json->{files} && ref $json->{files} eq 'ARRAY' ? $json->{files}[0]{id} : undef;
            if (!$id) { print STDERR "Unexpected upload response format for input #" . ($i+1) . "\n"; exit 1; }
            push @ids, $id;
            print STDERR "Uploaded PDF (#" . ($i+1) . "); id=$id\n";
        } else {
            # Convert to PDF via /pdf and capture outputId
            my $ct = content_type_for($p);
            my $req = POST("$api_base/pdf",
                'Content_Type' => 'form-data',
                'Content' => [ file => [$p, basename($p), 'Content-Type' => $ct] ]
            );
            $req->header('api-key' => $api_key);
            my $resp = $ua->request($req);
            print STDERR $resp->decoded_content // '';
            if (!$resp->is_success) { print STDERR "\nConversion failed (input #" . ($i+1) . ") status " . $resp->code . "\n"; exit 1; }
            my $json = decode_json($resp->decoded_content // '{}');
            my $id = $json->{outputId};
            if (!$id) { print STDERR "Unexpected conversion response format for input #" . ($i+1) . "\n"; exit 1; }
            push @ids, $id;
            print STDERR "Converted non-PDF (#" . ($i+1) . "); outputId=$id\n";
        }
    }

    # Build x-www-form-urlencoded with repeated arrays
    my @parts;
    for my $id (@ids) {
        push @parts, 'id[]=' . uri_escape($id);
        push @parts, 'pages[]=' . uri_escape('1-last');
        push @parts, 'type[]=id';
    }
    my $body = join('&', @parts);

    my $merge_req = HTTP::Request->new('POST', "$api_base/merged-pdf");
    $merge_req->header('api-key' => $api_key);
    $merge_req->header('Content-Type' => 'application/x-www-form-urlencoded');
    $merge_req->content($body);
    my $merge_resp = $ua->request($merge_req);
    print STDOUT $merge_resp->decoded_content // '';
    if (!$merge_resp->is_success) { print STDERR "\nMerge failed with status " . $merge_resp->code . "\n"; exit 1; }
    1;
} or do {
    my $err = $@ || 'Unknown error';
    $err =~ s/\s+$//;
    print STDERR "Error: $err\n";
    exit 1;
};

__END__

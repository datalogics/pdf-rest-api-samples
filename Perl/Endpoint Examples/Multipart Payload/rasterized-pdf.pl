#!/usr/bin/env perl
use strict;
use warnings;
use utf8;
use FindBin qw($Bin);
use File::Basename qw(basename);
use LWP::UserAgent;
use HTTP::Request::Common qw(POST);

# Tutorial: Rasterize a PDF using the Multipart style
#
# What this does
# - Sends the input PDF and options in a single multipart/form-data request to /rasterized-pdf.
#
# Why Multipart style?
# - Simpler for one-off operations (no separate upload step). Re-uploads the file each time.
#
# Setup
# - Put PDFREST_API_KEY in .env at the Perl folder root
# - Optionally set PDFREST_URL (defaults to https://api.pdfrest.com)
#   - EU/GDPR routing and proximity: https://eu-api.pdfrest.com/
#     More info: https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#
# Usage
# - perl "Endpoint Examples/Multipart Payload/rasterized-pdf.pl" /path/to/input.pdf

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
my $ua = LWP::UserAgent->new( timeout => 60 );

eval {
    my $req = POST("$api_base/rasterized-pdf",
        'Content_Type' => 'form-data',
        'Content' => [
            file => [$pdf_path, $filename, 'Content-Type' => 'application/pdf'],
            output => 'pdfrest_rasterize',
            # Optional: dpi => '300', color_space => 'rgb', page_range => '1-3',
        ]
    );
    $req->header('api-key' => $api_key);

    my $resp = $ua->request($req);
    print STDOUT $resp->decoded_content // '';
    if (!$resp->is_success) {
        print STDERR "\nRasterization failed with status " . $resp->code . "\n";
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

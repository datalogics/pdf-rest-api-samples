# pdfRest Perl Samples

A set of Perl scripts that demonstrate how to call the pdfRest API. Samples are organized by request style and include a multi‑step “Complex Flow” example.

- JSON Payload: upload first, then call the endpoint by `id`.
- Multipart Payload: send the file and options in a single request.

## Prerequisites
- Perl 5.26+ on macOS/Linux/WSL.
- A pdfRest API key (get one at https://pdfrest.com/getstarted/).
- cpanm (cpanminus) to install dependencies.

### Install cpanm
- macOS (Homebrew): `brew install cpanminus`
- Debian/Ubuntu: `sudo apt-get install cpanminus`
- CPAN: `perl -MCPAN -e 'install App::cpanminus'`
- Curl (alternative): `curl -L https://cpanmin.us | perl - --sudo App::cpanminus`

## Setup
1) Copy environment template and set your key
- `cp .env.example .env`
- Edit `.env` and set `PDFREST_API_KEY=your_api_key_here`
- Optional EU endpoint for GDPR/proximity: `PDFREST_URL=https://eu-api.pdfrest.com/`

2) Install dependencies from `cpanfile`
- Standard: `cpanm --installdeps .`
- macOS (Homebrew OpenSSL + HTTPS stack): `make install-macos` or `scripts/setup-macos.sh`
  - This installs `openssl@3` and builds `Net::SSLeay`, `IO::Socket::SSL`, etc.

## EU/GDPR Endpoint
- Default base URL is `https://api.pdfrest.com` (US).
- For GDPR compliance and improved performance in Europe, set in `.env`:
  - `PDFREST_URL=https://eu-api.pdfrest.com/`
- If `PDFREST_URL` is not set, the default US endpoint is used.
- More information: https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work

## Run the Samples
- JSON Markdown: `perl "Endpoint Examples/JSON Payload/markdown.pl" input.pdf`
- JSON Rasterize: `perl "Endpoint Examples/JSON Payload/rasterized-pdf.pl" input.pdf`
- Multipart Markdown: `perl "Endpoint Examples/Multipart Payload/markdown.pl" input.pdf`
- Multipart Rasterize: `perl "Endpoint Examples/Multipart Payload/rasterized-pdf.pl" input.pdf`
- Complex Flow Merge: `perl "Complex Flow Examples/merge-different-file-types.pl" file1.png file2.pptx file3.pdf`
- Save output: append `> response.json`

## Troubleshooting
- HTTPS modules missing: `cpanm --installdeps .` (includes `LWP::Protocol::https`, `IO::Socket::SSL`, `Mozilla::CA`).
- Net::SSLeay fails to build: install OpenSSL dev tools.
  - macOS: `xcode-select --install && brew install openssl@3 pkg-config` then `make install-macos`.
  - Debian/Ubuntu: `sudo apt-get install -y build-essential libssl-dev pkg-config` then `cpanm --reinstall Net::SSLeay IO::Socket::SSL`.
- Verify TLS: `perl -MIO::Socket::SSL -e 'print $IO::Socket::SSL::VERSION, qq(\n)'` and `perl -MLWP::Protocol::https -e 'print qq(https OK\n)'`.

Notes
- Scripts read `.env` from the Perl folder root and print API responses to stdout; diagnostics go to stderr and non‑zero exit on errors.

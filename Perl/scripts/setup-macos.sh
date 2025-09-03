#!/usr/bin/env bash
set -euo pipefail

echo "Installing prerequisites (Xcode tools, Homebrew OpenSSL, pkg-config)..."
if ! xcode-select -p >/dev/null 2>&1; then
  xcode-select --install || true
fi

if ! command -v brew >/dev/null 2>&1; then
  echo "Homebrew is required. Install from https://brew.sh and re-run." >&2
  exit 1
fi

brew install openssl@3 pkg-config || true

OPENSSL_PREFIX="$(brew --prefix openssl@3)"
export PKG_CONFIG_PATH="$OPENSSL_PREFIX/lib/pkgconfig"
export CPATH="$OPENSSL_PREFIX/include"
export LIBRARY_PATH="$OPENSSL_PREFIX/lib"

echo "Building Perl SSL modules against OpenSSL at $OPENSSL_PREFIX..."
cpanm --verbose --notest --reinstall --configure-args="--openssl-prefix=$OPENSSL_PREFIX" Net::SSLeay
cpanm IO::Socket::SSL LWP::Protocol::https Mozilla::CA

echo "Installing application dependencies from cpanfile..."
cpanm --installdeps .

echo "Verifying HTTPS support..."
perl -MIO::Socket::SSL -e 'print $IO::Socket::SSL::VERSION, qq(\n)'
perl -MLWP::Protocol::https -e 'print qq(https OK\n)'

echo "Done. You can now run:"
echo "  perl \"Endpoint Examples/JSON Payload/markdown.pl\" /path/to/input.pdf"

# C++ Samples for pdfRest

These C++ examples mirror the DotNET samples: JSON Payload (upload → JSON call), Multipart Payload (single multipart request), and a Complex Flow (convert then merge). All binaries are placed in `build/` for convenience.

## Prerequisites
- CMake 3.16+ and a C++20 compiler (Clang/GCC/MSVC)
- vcpkg (package manager): https://vcpkg.io/en/getting-started
- pdfRest API key: https://pdfrest.com/getstarted/

## Install Dependencies (vcpkg)
- Install required libraries:
  - `vcpkg install cpr nlohmann-json`
- Tip: set `VCPKG_ROOT` to your vcpkg folder (e.g., `~/vcpkg`).
  - Note: use the vcpkg CMake toolchain file; do not rely on `vcpkg integrate install`.

## Build (macOS/Linux/Windows)
- macOS/Linux:
  - `cmake -S . -B build -DCMAKE_TOOLCHAIN_FILE=$VCPKG_ROOT/scripts/buildsystems/vcpkg.cmake -DCMAKE_BUILD_TYPE=Release`
  - `cmake --build build --parallel`
- Windows (PowerShell):
  - `cmake -S . -B build -DCMAKE_TOOLCHAIN_FILE="$env:VCPKG_ROOT/scripts/buildsystems/vcpkg.cmake" -DCMAKE_BUILD_TYPE=Release`
  - `cmake --build build --config Release --parallel`
  - Note: specifying `-DCMAKE_TOOLCHAIN_FILE=...` is required to use vcpkg with CMake across platforms.

## Environment
- Copy `.env.example` to `.env` and set:
  - `PDFREST_API_KEY=your_api_key_here`
  - Optional: `PDFREST_URL=https://eu-api.pdfrest.com` (EU/GDPR region). More info: https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work

## Run Samples
- JSON two‑step (upload → JSON):
  - `./build/markdown_json /path/to/input.pdf`
  - `./build/rasterized_pdf_json /path/to/input.pdf`
- Multipart (single POST):
  - `./build/markdown_multipart /path/to/input.pdf`
  - `./build/rasterized_pdf_multipart /path/to/input.pdf`
- Complex flow (convert files to PDF, then merge):
  - `./build/merge_different_file_types image.png slides.pptx`

Notes
- Output: responses are printed as JSON; non‑2xx results exit non‑zero with concise errors.
- Proxies: respect `HTTPS_PROXY` / `HTTP_PROXY` if set.
- Paths with spaces: quote your paths on all platforms.

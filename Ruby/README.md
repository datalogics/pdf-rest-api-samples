# pdfRest Ruby Samples

A growing collection of Ruby scripts that demonstrate how to call the pdfRest API from the command line. These samples are organized by the type of HTTP payload they send to the API and will expand over time to cover many endpoints and options.

Current samples illustrate two request styles:
- JSON Payload samples
- Multipart Payload samples

Use this README from the repository’s top-level directory.

---

## Prerequisites

- Ruby 3.4.4 (via rbenv recommended)
- Bundler 2.7.1
- A pdfRest API key
- macOS/Linux/WSL or any environment where Ruby and Bundler are available

Project gems:
- dotenv
- faraday
- faraday-multipart
- faraday-retry

---

## Quick Start

1) Install Ruby (recommended via rbenv)
- Install rbenv per your OS instructions.
- Install and select the required Ruby version:
    - rbenv install 3.4.4
    - rbenv local 3.4.4

2) Install dependencies
- bundle install

3) Configure your API key
- Copy .env.example to .env
- Set PDFREST_API_KEY in .env:
    - PDFREST_API_KEY=your_api_key_here

4) Run a sample (from the repo root)
- JSON payload example:
    - ruby "Endpoint Examples/JSON Payload/markdown.rb" /path/to/input.pdf
- Multipart payload example:
    - ruby "Endpoint Examples/Multipart Payload/markdown.rb" /path/to/input.pdf

Each script prints the API response to stdout and exits non-zero on errors.

---

## Project Structure

- Endpoint Examples/
    - JSON Payload/
        - <endpoint>.rb
    - Multipart Payload/
        - <endpoint>.rb

You can:

- Browse the folders above to find the endpoint you want.
- Use the Ruby script name as a hint for the pdfRest API operation it demonstrates.
- Run the script with the path to your input file as the first argument, unless noted otherwise.

---

## JSON vs Multipart: What’s the Difference?

Both styles ultimately call the same API endpoints and return JSON responses, but they differ in how they send files and parameters.

1) JSON Payload Samples
- Flow:
    - Upload the file first to receive a file id.
    - Call the target endpoint with a JSON body that includes the file id and any options.
- Characteristics:
    - Content-Type: application/json
    - Useful when you want to reuse the same uploaded file across multiple operations without re-uploading.
    - Slightly more steps (upload + operation) but efficient for pipelines or repeated operations.

2) Multipart Payload Samples
- Flow:
    - Send the file and options in a single multipart/form-data request directly to the endpoint.
- Characteristics:
    - Content-Type: multipart/form-data
    - Simpler for one-off operations (no separate upload step).
    - Re-uploads the file each time you call an endpoint.

When to choose which:
- Choose JSON Payload if you plan to chain operations or reuse files.
- Choose Multipart Payload for quick, single-shot conversions per file.

---

## Running the Samples

From the repository root:

- JSON Payload samples:
    - ruby "Endpoint Examples/JSON Payload/markdown.rb" /path/to/input.pdf
    - ruby "Endpoint Examples/JSON Payload/rasterized-pdf.rb" /path/to/input.pdf

- Multipart Payload samples:
    - ruby "Endpoint Examples/Multipart Payload/markdown.rb" /path/to/input.pdf
    - ruby "Endpoint Examples/Multipart Payload/rasterized-pdf.rb" /path/to/input.pdf

Notes:
- Replace /path/to/input.pdf with your local file path.
- Some samples may accept or document optional parameters (e.g., page ranges, DPI, color space). Review the sample file and inline comments to see what options are available for that endpoint type.
- All scripts require PDFREST_API_KEY in your environment via .env.

---

## Environment and Configuration

- API key
    - Set PDFREST_API_KEY in .env (dotenv loads this automatically).
- Retries
    - Samples are configured with brief, limited retries for transient network errors.
- Output
    - Responses are printed to stdout (typically JSON). You may pipe or redirect to files:
        - ruby "<path to sample>.rb" input.pdf > response.json

### EU/GDPR Endpoint (Optional)

- Base URL
    - By default, samples call `https://api.pdfrest.com`.
    - For GDPR compliance and improved performance for European users, set in `.env`:
        - `PDFREST_URL=https://eu-api.pdfrest.com/`
    - If `PDFREST_URL` is not set, the default US endpoint is used.
- More information
    - Learn how EU GDPR API calls work: https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work

---

## Troubleshooting

- “Missing PDFREST_API_KEY in .env”
    - Ensure .env exists at the top level and includes a valid PDFREST_API_KEY.
- “Usage: … /path/to/file.pdf”
    - Pass a valid file path as the first argument.
- HTTP error responses
    - Non-2xx responses will cause the script to exit with a message and non-zero status. Inspect the printed response for details.
- Network/Proxy issues
    - If behind a corporate proxy, configure standard environment variables (e.g., HTTPS_PROXY) before running the scripts.

---

## Developing and Contributing

- Update dependencies
    - bundle update
- Ruby version
    - rbenv local 3.4.4 (if not already set)
- Adding new samples
    - Place new scripts under:
        - Endpoint Examples/JSON Payload/ for two-step (upload + JSON) examples
        - Endpoint Examples/Multipart Payload/ for single-step multipart examples
    - Keep naming consistent with the target endpoint for easy discovery.
    - Follow the pattern of reading configuration from .env and accepting the input file path as the first argument.
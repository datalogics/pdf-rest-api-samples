#!
# What this sample does:
# - Converts a PDF to Markdown using pdfRest.
# - Uses a JSON payload in two steps: first uploads, then calls /markdown with the uploaded id.
#
# Setup (.env):
# - Copy .env.example to .env
# - Set PDFREST_API_KEY=your_api_key_here
# - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
#     PDFREST_URL=https://eu-api.pdfrest.com
#
# Usage:
#   ruby "Endpoint Examples/JSON Payload/markdown.rb" /path/to/input.pdf
#
# Output:
# - Prints the API JSON response to stdout. Non-2xx responses abort with a concise message.
# - Tip: pipe output to a file: ruby ... > response.json

require "json"
require "faraday"
require "faraday/retry"
require "dotenv"

Dotenv.load

API_KEY = ENV["PDFREST_API_KEY"]
abort("Missing PDFREST_API_KEY in .env") if API_KEY.nil? || API_KEY.strip.empty?

# Allow overriding the API base URL via .env (default to US endpoint)
API_BASE = (ENV["PDFREST_URL"] || ENV["PDFREST_API"] || "https://api.pdfrest.com").sub(%r{/+$}, "")

pdf_path = ARGV[0]
abort("Usage: ruby markdown.rb /path/to/file.pdf") unless pdf_path && File.file?(pdf_path)

filename = File.basename(pdf_path)
file_bytes = File.binread(pdf_path)

begin
  # 1) Upload the file
  upload_conn = Faraday.new(url: API_BASE) do |f|
    f.request :retry, max: 2, interval: 0.2
    f.adapter Faraday.default_adapter
  end

  upload_resp = upload_conn.post("/upload") do |req|
    req.headers["api-key"] = API_KEY
    req.headers["content-filename"] = filename
    req.headers["Content-Type"] = "application/octet-stream"
    req.body = file_bytes
  end

  STDERR.puts upload_resp.body
  unless upload_resp.success?
    abort("Upload failed with status #{upload_resp.status}")
  end

  upload_json = JSON.parse(upload_resp.body)
  uploaded_id = upload_json.fetch("files").fetch(0).fetch("id")
  STDERR.puts "Successfully uploaded with an id of: #{uploaded_id}"

  # 2) Convert to Markdown
  markdown_conn = Faraday.new(url: API_BASE) do |f|
    f.request :retry, max: 2, interval: 0.2
    f.adapter Faraday.default_adapter
  end

  body = { id: uploaded_id, page_break_comments: "on" }.to_json

  markdown_resp = markdown_conn.post("/markdown") do |req|
    req.headers["api-key"] = API_KEY
    req.headers["Content-Type"] = "application/json"
    req.body = body
  end

  puts markdown_resp.body
  unless markdown_resp.success?
    abort("Markdown conversion failed with status #{markdown_resp.status}")
  end

rescue KeyError => e
  abort("Unexpected response format: #{e.message}")
rescue => e
  abort("Error: #{e.class}: #{e.message}")
end

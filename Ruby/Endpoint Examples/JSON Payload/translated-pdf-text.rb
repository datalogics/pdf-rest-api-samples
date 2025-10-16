#!
# Translate PDF text via two-step JSON flow.

require "json"
require "faraday"
require "faraday/retry"
require "dotenv"

Dotenv.load

API_KEY = ENV["PDFREST_API_KEY"]
abort("Missing PDFREST_API_KEY in .env") if API_KEY.nil? || API_KEY.strip.empty?
API_BASE = (ENV["PDFREST_URL"] || ENV["PDFREST_API"] || "https://api.pdfrest.com").sub(%r{/+$}, "")

pdf_path = ARGV[0]
abort("Usage: ruby translated-pdf-text.rb /path/to/file.pdf") unless pdf_path && File.file?(pdf_path)

filename = File.basename(pdf_path)
file_bytes = File.binread(pdf_path)

begin
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
  abort("Upload failed with status #{upload_resp.status}") unless upload_resp.success?
  upload_json = JSON.parse(upload_resp.body)
  uploaded_id = upload_json.fetch("files").fetch(0).fetch("id")
  STDERR.puts "Successfully uploaded with an id of: #{uploaded_id}"

  conn = Faraday.new(url: API_BASE) do |f|
    f.request :retry, max: 2, interval: 0.2
    f.adapter Faraday.default_adapter
  end
  # Translates text to American English. Format the output_language as a 2-3 character ISO 639 code, optionally with a region/script (e.g., 'en', 'es', 'zh-Hant', 'eng-US').
  body = { id: uploaded_id, output_language: "en-US" }.to_json
  resp = conn.post("/translated-pdf-text") do |req|
    req.headers["api-key"] = API_KEY
    req.headers["Content-Type"] = "application/json"
    req.body = body
  end
  puts resp.body
  abort("Translate failed with status #{resp.status}") unless resp.success?
rescue KeyError => e
  abort("Unexpected response format: #{e.message}")
rescue => e
  abort("Error: #{e.class}: #{e.message}")
end


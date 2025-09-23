#!
# Summarize PDF text via single multipart/form-data request.

require "faraday"
require "faraday/retry"
require "faraday/multipart"
require "dotenv"

Dotenv.load

API_KEY = ENV["PDFREST_API_KEY"]
abort("Missing PDFREST_API_KEY in .env") if API_KEY.nil? || API_KEY.strip.empty?
API_BASE = (ENV["PDFREST_URL"] || ENV["PDFREST_API"] || "https://api.pdfrest.com").sub(%r{/+$}, "")

pdf_path = ARGV[0]
abort("Usage: ruby summarized-pdf-text.rb /path/to/file.pdf") unless pdf_path && File.file?(pdf_path)

filename = File.basename(pdf_path)

begin
  conn = Faraday.new(url: API_BASE) do |f|
    f.request :retry, max: 2, interval: 0.2
    f.request :multipart
    f.adapter Faraday.default_adapter
  end
  body = {
    file: Faraday::Multipart::FilePart.new(pdf_path, "application/pdf", filename),
    target_word_count: "100",
  }
  resp = conn.post("/summarized-pdf-text") do |req|
    req.headers["api-key"] = API_KEY
    req.body = body
  end
  puts resp.body
  abort("Summarize failed with status #{resp.status}") unless resp.success?
rescue => e
  abort("Error: #{e.class}: #{e.message}")
end


# ruby
require "faraday"
require "faraday/retry"
require "faraday/multipart"
require "dotenv"

Dotenv.load

API_KEY = ENV["PDFREST_API_KEY"]
abort("Missing PDFREST_API_KEY in .env") if API_KEY.nil? || API_KEY.strip.empty?

# Allow overriding the API base URL via .env (default to US endpoint)
API_BASE = (ENV["PDFREST_URL"] || ENV["PDFREST_API"] || "https://api.pdfrest.com").sub(%r{/+$}, "")

pdf_path = ARGV[0]
abort("Usage: ruby markdown_multipart.rb /path/to/file.pdf") unless pdf_path && File.file?(pdf_path)

filename = File.basename(pdf_path)

begin
  conn = Faraday.new(url: API_BASE) do |f|
    f.request :retry, max: 2, interval: 0.2
    f.request :multipart
    f.adapter Faraday.default_adapter
  end

  # Build multipart form body. Middleware will set multipart/form-data with boundary.
  body = {
    file: Faraday::Multipart::FilePart.new(pdf_path, "application/pdf", filename),
    output: "pdfrest_markdown",
    page_break_comments: "on",
    # Optional parameters:
    # page_range: "1-3"
  }

  resp = conn.post("/markdown") do |req|
    req.headers["api-key"] = API_KEY
    req.body = body
  end

  puts resp.body
  unless resp.success?
    abort("Markdown conversion failed with status #{resp.status}")
  end

rescue => e
  abort("Error: #{e.class}: #{e.message}")
end

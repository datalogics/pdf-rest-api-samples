require "faraday"
require "faraday/retry"
require "dotenv"
require "faraday/multipart"

Dotenv.load

API_KEY = ENV["PDFREST_API_KEY"]
abort("Missing PDFREST_API_KEY in .env") if API_KEY.nil? || API_KEY.strip.empty?

pdf_path = ARGV[0]
abort("Usage: ruby rasterized_pdf_multipart.rb /path/to/file.pdf") unless pdf_path && File.file?(pdf_path)

filename = File.basename(pdf_path)

begin
  conn = Faraday.new(url: "https://api.pdfrest.com") do |f|
    f.request :retry, max: 2, interval: 0.2
    f.request :multipart
    f.adapter Faraday.default_adapter
  end

  # Build multipart form body. Faraday will set the proper multipart/form-data boundary.
  body = {
    file: Faraday::Multipart::FilePart.new(pdf_path, "application/pdf", filename),
    output: "pdfrest_rasterize"
    # Optional parameters you can include:
    # dpi: "300",
    # color_space: "rgb",
    # page_range: "1-3"
  }

  resp = conn.post("/rasterized-pdf") do |req|
    req.headers["api-key"] = API_KEY
    req.body = body
  end

  puts resp.body
  unless resp.success?
    abort("Rasterization failed with status #{resp.status}")
  end

rescue => e
  abort("Error: #{e.class}: #{e.message}")
end

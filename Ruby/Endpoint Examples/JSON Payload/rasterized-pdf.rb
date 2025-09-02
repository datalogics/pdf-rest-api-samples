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
abort("Usage: ruby rasterized_pdf.rb /path/to/file.pdf") unless pdf_path && File.file?(pdf_path)

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

  # 2) Create rasterized PDF
  raster_conn = Faraday.new(url: API_BASE) do |f|
    f.request :retry, max: 2, interval: 0.2
    f.adapter Faraday.default_adapter
  end

  # Provide only the required parameter (id). Add optional params as needed.
  # Example optional params you might add later: dpi, color_space, page_range, etc.
  body = { id: uploaded_id }.to_json

  raster_resp = raster_conn.post("/rasterized-pdf") do |req|
    req.headers["api-key"] = API_KEY
    req.headers["Content-Type"] = "application/json"
    req.body = body
  end

  puts raster_resp.body
  unless raster_resp.success?
    abort("Rasterization failed with status #{raster_resp.status}")
  end

rescue KeyError => e
  abort("Unexpected response format: #{e.message}")
rescue => e
  abort("Error: #{e.class}: #{e.message}")
end

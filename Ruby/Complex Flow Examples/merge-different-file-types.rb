#!
# What this sample does:
# - Merges multiple inputs (PDFs and non-PDFs) into a single PDF.
# - Non-PDF files are first converted to PDF; PDFs are uploaded as-is. All resulting IDs are then merged.
#
# Setup (.env):
# - Copy .env.example to .env
# - Set PDFREST_API_KEY=your_api_key_here
# - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
#     PDFREST_URL=https://eu-api.pdfrest.com
# For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#
# Usage:
#   ruby "Complex Flow Examples/merge-different-file-types.rb" /path/to/file1 /path/to/file2 [...]
#
# Output:
# - Prints the API JSON response to stdout. Non-2xx responses abort with a concise message.
# - Tip: pipe output to a file: ruby ... > response.json

require "json"
require "faraday"
require "faraday/retry"
require "faraday/multipart"
require "dotenv"
require "cgi"

Dotenv.load

API_KEY = ENV["PDFREST_API_KEY"]
abort("Missing PDFREST_API_KEY in .env") if API_KEY.nil? || API_KEY.strip.empty?

# Allow overriding the API base URL via .env (default to US endpoint)
API_BASE = (ENV["PDFREST_URL"] || ENV["PDFREST_API"] || "https://api.pdfrest.com").sub(%r{/+$}, "")

paths = ARGV
if paths.length < 2 || paths.any? { |p| !File.file?(p) }
  abort("Usage: ruby merge-different-file-types.rb /path/to/file1 /path/to/file2 [/path/to/file3 ...]")
end

def content_type_for(path)
  ext = File.extname(path).downcase
  case ext
  when ".pdf" then "application/pdf"
  when ".png" then "image/png"
  when ".jpg", ".jpeg" then "image/jpeg"
  when ".gif" then "image/gif"
  when ".tif", ".tiff" then "image/tiff"
  when ".bmp" then "image/bmp"
  when ".webp" then "image/webp"
  when ".doc" then "application/msword"
  when ".docx" then "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
  when ".ppt" then "application/vnd.ms-powerpoint"
  when ".pptx" then "application/vnd.openxmlformats-officedocument.presentationml.presentation"
  when ".xls" then "application/vnd.ms-excel"
  when ".xlsx" then "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
  when ".txt" then "text/plain"
  when ".rtf" then "application/rtf"
  when ".html", ".htm" then "text/html"
  else "application/octet-stream"
  end
end

begin
  conn = Faraday.new(url: API_BASE) do |f|
    f.request :retry, max: 2, interval: 0.2
    f.request :multipart
    f.adapter Faraday.default_adapter
  end

  # Collect IDs for merge (from either /pdf or /upload)
  collected_ids = []
  paths.each_with_index do |p, idx|
    if File.extname(p).downcase == ".pdf"
      # Already a PDF: upload to get an id
      upload_resp = conn.post("/upload") do |req|
        req.headers["api-key"] = API_KEY
        req.headers["content-filename"] = File.basename(p)
        req.headers["Content-Type"] = "application/octet-stream"
        req.body = File.binread(p)
      end
      STDERR.puts upload_resp.body
      abort("Upload failed for input ##{idx + 1} with status #{upload_resp.status}") unless upload_resp.success?
      upload_json = JSON.parse(upload_resp.body)
      collected_ids << upload_json.fetch("files").fetch(0).fetch("id")
      STDERR.puts "Uploaded PDF (##{idx + 1}); id=#{collected_ids.last}"
    else
      # Not a PDF: convert to PDF via /pdf to get an outputId
      ct = content_type_for(p)
      part = Faraday::Multipart::FilePart.new(p, ct, File.basename(p))
      conv_resp = conn.post("/pdf") do |req|
        req.headers["api-key"] = API_KEY
        req.body = { file: part }
      end
      STDERR.puts conv_resp.body
      abort("Conversion failed for input ##{idx + 1} with status #{conv_resp.status}") unless conv_resp.success?
      collected_ids << JSON.parse(conv_resp.body).fetch("outputId")
      STDERR.puts "Converted non-PDF (##{idx + 1}); outputId=#{collected_ids.last}"
    end
  end

  # Build merge parameters for all inputs
  # Build urlencoded body to avoid multipart array encoding issues
  form_parts = []
  collected_ids.each do |id|
    form_parts << "id[]=#{CGI.escape(id)}"
    form_parts << "pages[]=#{CGI.escape('1-last')}"
    form_parts << "type[]=id"
  end

  resp_merge = conn.post("/merged-pdf") do |req|
    req.headers["api-key"] = API_KEY
    req.headers["Content-Type"] = "application/x-www-form-urlencoded"
    req.body = form_parts.join('&')
  end

  puts resp_merge.body
  abort("Merge failed with status #{resp_merge.status}") unless resp_merge.success?

rescue KeyError => e
  abort("Unexpected response format: #{e.message}")
rescue => e
  abort("Error: #{e.class}: #{e.message}")
end

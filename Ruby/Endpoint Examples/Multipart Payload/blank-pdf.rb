#!
# What this sample does:
# - Calls /blank-pdf via multipart/form-data to create a blank three-page PDF.
#
# Setup (.env):
# - Copy .env.example to .env
# - Set PDFREST_API_KEY=your_api_key_here
# - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
#     PDFREST_URL=https://eu-api.pdfrest.com
#   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
#
# Usage:
#   ruby "Endpoint Examples/Multipart Payload/blank-pdf.rb"
#
# Output:
# - Prints the API JSON response to stdout. Non-2xx responses abort with a concise message.

require "faraday"
require "faraday/retry"
require "faraday/multipart"
require "dotenv"

Dotenv.load

API_KEY = ENV["PDFREST_API_KEY"]
abort("Missing PDFREST_API_KEY in .env") if API_KEY.nil? || API_KEY.strip.empty?

API_BASE = (ENV["PDFREST_URL"] || ENV["PDFREST_API"] || "https://api.pdfrest.com").sub(%r{/+$}, "")

conn = Faraday.new(url: API_BASE) do |f|
  f.request :retry, max: 2, interval: 0.2
  f.request :multipart
  f.adapter Faraday.default_adapter
end

body = {
  page_size: "letter",
  page_count: "3",
  page_orientation: "portrait"
}

response = conn.post("/blank-pdf") do |req|
  req.headers["api-key"] = API_KEY
  req.body = body
end

puts response.body
abort("blank-pdf request failed with status #{response.status}") unless response.success?

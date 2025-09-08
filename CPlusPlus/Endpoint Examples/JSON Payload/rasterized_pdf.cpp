/*
 * What this sample does:
 * - Uploads a file, then calls /rasterized-pdf with a JSON body {"id": ...}.
 *   (JSON two-step flow: upload → rasterized-pdf)
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   ./rasterized_pdf_json /path/to/input.pdf
 *
 * Output:
 * - Prints JSON responses to stdout; non-2xx results print concise errors and exit non-zero.
 */

#include <cpr/cpr.h>
#include <nlohmann/json.hpp>

#include <cstdlib>
#include <filesystem>
#include <fstream>
#include <iostream>
#include <optional>
#include <string>

namespace fs = std::filesystem;
using json = nlohmann::json;

static std::string rtrim_slashes(std::string s) {
    while (!s.empty() && (s.back() == '/' || s.back() == '\\')) s.pop_back();
    return s;
}

static void load_dotenv_if_present(const fs::path &path) {
    std::ifstream f(path);
    if (!f.is_open()) return;
    std::string line;
    while (std::getline(f, line)) {
        if (line.empty() || line[0] == '#') continue;
        auto pos = line.find('=');
        if (pos == std::string::npos) continue;
        std::string key = line.substr(0, pos);
        std::string val = line.substr(pos + 1);
        auto trim = [](std::string &s) {
            size_t b = s.find_first_not_of(" \t\r\n");
            size_t e = s.find_last_not_of(" \t\r\n");
            if (b == std::string::npos) { s.clear(); return; }
            s = s.substr(b, e - b + 1);
        };
        trim(key); trim(val);
        if (key.empty()) continue;
        if (std::getenv(key.c_str()) == nullptr) {
#ifdef _WIN32
            _putenv_s(key.c_str(), val.c_str());
#else
            setenv(key.c_str(), val.c_str(), 0);
#endif
        }
    }
}

static void load_env() {
    const fs::path here = fs::current_path();
    load_dotenv_if_present(here / ".env");
    if (fs::exists(here.parent_path())) load_dotenv_if_present(here.parent_path() / ".env");
}

static std::optional<std::string> read_file_to_string(const fs::path &p) {
    std::ifstream in(p, std::ios::binary);
    if (!in) return std::nullopt;
    return std::string((std::istreambuf_iterator<char>(in)), std::istreambuf_iterator<char>());
}

int main(int argc, char **argv) {
    load_env();
    if (argc < 2) {
        std::cerr << "Usage: rasterized_pdf <input.pdf>\n";
        return 1;
    }
    fs::path input_path(argv[1]);
    if (!fs::exists(input_path)) {
        std::cerr << "File not found: " << input_path << "\n";
        return 1;
    }
    const char *api_key_c = std::getenv("PDFREST_API_KEY");
    if (!api_key_c || std::string(api_key_c).empty()) {
        std::cerr << "Missing required environment variable: PDFREST_API_KEY\n";
        return 1;
    }
    std::string api_key = api_key_c;
    std::string base_url = std::getenv("PDFREST_URL") ? std::string(std::getenv("PDFREST_URL")) : std::string("https://api.pdfrest.com");
    base_url = rtrim_slashes(base_url);

    auto data = read_file_to_string(input_path);
    if (!data) {
        std::cerr << "Failed to read input file\n";
        return 1;
    }

    // Upload
    cpr::Header up_headers{{"Api-Key", api_key}, {"Accept", "application/json"}, {"Content-Type", "application/octet-stream"}, {"Content-Filename", input_path.filename().string()}};
    auto up = cpr::Post(cpr::Url{base_url + "/upload"}, up_headers, cpr::Body{*data});
    if (up.error || up.status_code < 200 || up.status_code >= 300) {
        std::cerr << "Upload failed (status " << up.status_code << ")\n" << up.error.message << "\n" << up.text << "\n";
        return 1;
    }
    std::cout << up.text << "\n";
    std::string id;
    try {
        auto j = json::parse(up.text);
        id = j.at("files").at(0).at("id").get<std::string>();
    } catch (const std::exception &e) {
        std::cerr << "Parse error: " << e.what() << "\n";
        return 1;
    }

    // Rasterize
    json payload = {{"id", id}};
    cpr::Header md_headers{{"Api-Key", api_key}, {"Accept", "application/json"}, {"Content-Type", "application/json"}};
    auto rs = cpr::Post(cpr::Url{base_url + "/rasterized-pdf"}, md_headers, cpr::Body{payload.dump()});
    if (rs.error || rs.status_code < 200 || rs.status_code >= 300) {
        std::cerr << "Rasterize failed (status " << rs.status_code << ")\n" << rs.error.message << "\n" << rs.text << "\n";
        return 1;
    }
    std::cout << rs.text << "\n";
    return 0;
}

/*
 * What this sample does:
 * - Uploads a PDF via /upload, then calls /summarized-pdf-text with a JSON payload
 *   referencing the uploaded resource id (two-step JSON flow).
 *
 * Setup (environment):
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   More info: https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   ./summarized_pdf_text_json /path/to/input.pdf
 *
 * Output:
 * - Prints JSON responses to stdout. Non-2xx responses print a concise
 *   error to stderr and exit non-zero.
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
    while (!s.empty() && (s.back() == '/' || s.back() == '\\')) {
        s.pop_back();
    }
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
            size_t start = s.find_first_not_of(" \t\r\n");
            size_t end = s.find_last_not_of(" \t\r\n");
            if (start == std::string::npos) { s.clear(); return; }
            s = s.substr(start, end - start + 1);
        };
        trim(key);
        trim(val);
        if (key.empty()) continue;
#ifdef _WIN32
        _putenv_s(key.c_str(), val.c_str());
#else
        if (std::getenv(key.c_str()) == nullptr) setenv(key.c_str(), val.c_str(), 0);
#endif
    }
}

static void load_env() {
    const fs::path here = fs::current_path();
    load_dotenv_if_present(here / ".env");
    if (fs::exists(here.parent_path())) {
        load_dotenv_if_present(here.parent_path() / ".env");
    }
}

static std::optional<std::string> read_file_to_string(const fs::path &p) {
    std::ifstream in(p, std::ios::binary);
    if (!in) return std::nullopt;
    std::string data((std::istreambuf_iterator<char>(in)), std::istreambuf_iterator<char>());
    return data;
}

int main(int argc, char *argv[]) {
    load_env();
    if (argc < 2) {
        std::cerr << "Usage: summarized_pdf_text_json <input.pdf>\n";
        return 1;
    }
    fs::path input_path(argv[1]);
    if (!fs::exists(input_path)) {
        std::cerr << "File not found: " << input_path << "\n";
        return 1;
    }

    const char *api_key_c = std::getenv("PDFREST_API_KEY");
    if (api_key_c == nullptr || std::string(api_key_c).empty()) {
        std::cerr << "Missing required environment variable: PDFREST_API_KEY\n";
        return 1;
    }
    std::string api_key = api_key_c;

    const char *base_url_c = std::getenv("PDFREST_URL");
    std::string base_url = base_url_c && std::string(base_url_c).size() ? base_url_c : std::string("https://api.pdfrest.com");
    base_url = rtrim_slashes(base_url);

    auto maybe_data = read_file_to_string(input_path);
    if (!maybe_data) {
        std::cerr << "Failed to read input file: " << input_path << "\n";
        return 1;
    }
    std::string body = std::move(*maybe_data);

    cpr::Header headers{{"Api-Key", api_key}, {"Accept", "application/json"}, {"Content-Type", "application/octet-stream"}, {"Content-Filename", input_path.filename().string()}};
    auto res = cpr::Post(cpr::Url{base_url + "/upload"}, headers, cpr::Body{body});
    if (res.error || res.status_code < 200 || res.status_code >= 300) {
        std::cerr << "Upload failed (status " << res.status_code << "): " << res.error.message << "\n" << res.text << "\n";
        return 1;
    }
    std::cout << res.text << "\n";

    std::string uploaded_id;
    try {
        auto j = json::parse(res.text);
        uploaded_id = j.at("files").at(0).at("id").get<std::string>();
    } catch (const std::exception &e) {
        std::cerr << "Failed to parse upload id: " << e.what() << "\n";
        return 1;
    }

    json payload = { {"id", uploaded_id}, {"target_word_count", 100} };

    cpr::Header sum_headers{{"Api-Key", api_key}, {"Accept", "application/json"}, {"Content-Type", "application/json"}};
    auto sum_res = cpr::Post(cpr::Url{base_url + "/summarized-pdf-text"}, sum_headers, cpr::Body{payload.dump()});
    if (sum_res.error || sum_res.status_code < 200 || sum_res.status_code >= 300) {
        std::cerr << "Summarize failed (status " << sum_res.status_code << "): " << sum_res.error.message << "\n" << sum_res.text << "\n";
        return 1;
    }
    std::cout << sum_res.text << "\n";
    return 0;
}

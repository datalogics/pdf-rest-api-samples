/*
 * What this sample does:
 * - Calls /blank-pdf with a JSON payload to create an empty three-page PDF.
 *
 * Setup (environment):
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   More info: https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   ./blank_pdf_json
 *
 * Output:
 * - Prints the JSON response. Non-2xx responses exit non-zero.
 */

#include <cpr/cpr.h>
#include <nlohmann/json.hpp>

#include <cstdlib>
#include <filesystem>
#include <fstream>
#include <iostream>
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
    if (fs::exists(here.parent_path())) {
        load_dotenv_if_present(here.parent_path() / ".env");
    }
}

int main() {
    load_env();

    const char *api_key_c = std::getenv("PDFREST_API_KEY");
    if (api_key_c == nullptr || std::string(api_key_c).empty()) {
        std::cerr << "Missing required environment variable: PDFREST_API_KEY\n";
        return 1;
    }

    const char *base_url_c = std::getenv("PDFREST_URL");
    std::string base_url = base_url_c && std::string(base_url_c).size()
                               ? base_url_c
                               : std::string("https://api.pdfrest.com");
    base_url = rtrim_slashes(base_url);

    json payload = {
        {"page_size", "letter"},
        {"page_count", 3},
        {"page_orientation", "portrait"}
    };

    cpr::Header headers{
        {"Api-Key", api_key_c},
        {"Accept", "application/json"},
        {"Content-Type", "application/json"}
    };

    auto res = cpr::Post(
        cpr::Url{base_url + "/blank-pdf"},
        headers,
        cpr::Body{payload.dump()}
    );

    if (res.error || res.status_code < 200 || res.status_code >= 300) {
        std::cerr << "blank-pdf failed (status " << res.status_code << "): "
                  << res.error.message << "\n" << res.text << "\n";
        return 1;
    }

    std::cout << res.text << "\n";
    return 0;
}

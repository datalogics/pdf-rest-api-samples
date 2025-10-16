/*
 * What this sample does:
 * - Translates PDF text content via multipart/form-data (file + options).
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   ./translated_pdf_text_multipart /path/to/input.pdf
 *
 * Output:
 * - Prints the JSON response to stdout; non-2xx exits with concise error.
 */

#include <cpr/cpr.h>
#include <filesystem>
#include <iostream>
#include <fstream>
#include <cstdlib>

namespace fs = std::filesystem;

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
        auto p = line.find('=');
        if (p == std::string::npos) continue;
        std::string k = line.substr(0, p);
        std::string v = line.substr(p + 1);
        auto trim = [](std::string &s) { size_t b = s.find_first_not_of(" \t\r\n"), e = s.find_last_not_of(" \t\r\n"); if (b == std::string::npos) { s.clear(); return; } s = s.substr(b, e - b + 1); };
        trim(k); trim(v);
#ifdef _WIN32
        _putenv_s(k.c_str(), v.c_str());
#else
        if (!std::getenv(k.c_str())) setenv(k.c_str(), v.c_str(), 0);
#endif
    }
}

static void load_env() {
    auto here = fs::current_path();
    load_dotenv_if_present(here / ".env");
    if (fs::exists(here.parent_path())) load_dotenv_if_present(here.parent_path() / ".env");
}

int main(int argc, char **argv) {
    load_env();
    if (argc < 2) { std::cerr << "Usage: translated_pdf_text_multipart <input.pdf>\n"; return 1; }
    fs::path input = argv[1];
    if (!fs::exists(input)) { std::cerr << "File not found: " << input << "\n"; return 1; }
    const char *key = getenv("PDFREST_API_KEY");
    if (!key || !*key) { std::cerr << "Missing PDFREST_API_KEY\n"; return 1; }
    std::string base = getenv("PDFREST_URL") ? getenv("PDFREST_URL") : "https://api.pdfrest.com";
    base = rtrim_slashes(base);

    cpr::Header hdr{{"Api-Key", key}, {"Accept", "application/json"}};
    cpr::Multipart mp{
        {"file", cpr::File{input.string()}},
        // Translates text to American English. Format the output_language as a 2-3 character ISO 639 code,
        // optionally with a region/script (e.g., 'en', 'es', 'zh-Hant', 'eng-US').
        {"output_language", "en-US"}
    };
    auto res = cpr::Post(cpr::Url{base + "/translated-pdf-text"}, hdr, mp);
    if (res.error || res.status_code < 200 || res.status_code >= 300) {
        std::cerr << "Request failed (" << res.status_code << ")\n" << res.error.message << "\n" << res.text << "\n";
        return 1;
    }
    std::cout << res.text << "\n";
    return 0;
}


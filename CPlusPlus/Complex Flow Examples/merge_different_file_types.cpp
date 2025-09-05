/*
 * What this sample does:
 * - Converts two different file types to PDF via multipart (/pdf), then merges them via /merged-pdf.
 *
 * Setup (environment):
 * - Copy .env.example to .env
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL to override the API region. For EU/GDPR compliance and proximity, use:
 *     PDFREST_URL=https://eu-api.pdfrest.com
 *   For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage:
 *   ./merge_different_file_types image.png slides.pptx
 *
 * Output:
 * - Prints JSON responses for the two conversions and the final merge; non-2xx exits with concise error.
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

static void load_dotenv_if_present(const fs::path &p) {
    std::ifstream f(p);
    if (!f.is_open()) return;
    std::string l;
    while (std::getline(f, l)) {
        if (l.empty() || l[0] == '#') continue;
        auto pos = l.find('=');
        if (pos == std::string::npos) continue;
        std::string k = l.substr(0, pos);
        std::string v = l.substr(pos + 1);
        auto trim = [](std::string &s) {
            size_t b = s.find_first_not_of(" \t\r\n");
            size_t e = s.find_last_not_of(" \t\r\n");
            if (b == std::string::npos) { s.clear(); return; }
            s = s.substr(b, e - b + 1);
        };
        trim(k); trim(v);
        if (k.empty()) continue;
        if (!std::getenv(k.c_str())) {
#ifdef _WIN32
            _putenv_s(k.c_str(), v.c_str());
#else
            setenv(k.c_str(), v.c_str(), 0);
#endif
        }
    }
}

static void load_env() {
    auto here = fs::current_path();
    load_dotenv_if_present(here / ".env");
    if (fs::exists(here.parent_path())) {
        load_dotenv_if_present(here.parent_path() / ".env");
    }
}

static std::string convert_to_pdf_id(const std::string &base,
                                     const std::string &api_key,
                                     const fs::path &input) {
    cpr::Header hdr{{"Api-Key", api_key}, {"Accept", "application/json"}};
    cpr::Multipart mp{{"file", cpr::File{input.string()}}};
    auto res = cpr::Post(cpr::Url{base + "/pdf"}, hdr, mp);
    if (res.error || res.status_code < 200 || res.status_code >= 300) {
        throw std::runtime_error(
            "Convert failed: " + std::to_string(res.status_code) + "\n" +
            res.error.message + "\n" + res.text);
    }
    std::cout << res.text << "\n";
    auto j = json::parse(res.text);
    return j.at("outputId").get<std::string>();
}

int main(int argc, char **argv) {
    load_env();
    if (argc < 3) {
        std::cerr << "Usage: merge_different_file_types <imageFile> <pptFile>\n";
        return 1;
    }
    fs::path img = argv[1], ppt = argv[2];
    if (!fs::exists(img) || !fs::exists(ppt)) {
        std::cerr << "One or more input files not found.\n";
        return 1;
    }
    const char *key = getenv("PDFREST_API_KEY");
    if (!key || !*key) {
        std::cerr << "Missing PDFREST_API_KEY\n";
        return 1;
    }
    std::string base = getenv("PDFREST_URL") ? getenv("PDFREST_URL") : "https://api.pdfrest.com";
    base = rtrim_slashes(base);

    try {
        std::string img_id = convert_to_pdf_id(base, key, img);
        std::string ppt_id = convert_to_pdf_id(base, key, ppt);

        cpr::Header hdr{{"Api-Key", key}, {"Accept", "application/json"}};
        cpr::Multipart mp{
            {"id[]", img_id}, {"type[]", "id"}, {"pages[]", "all"},
            {"id[]", ppt_id}, {"type[]", "id"}, {"pages[]", "all"}
        };
        auto merge = cpr::Post(cpr::Url{base + "/merged-pdf"}, hdr, mp);
        if (merge.error || merge.status_code < 200 || merge.status_code >= 300) {
            std::cerr << "Merge failed (" << merge.status_code << ")\n"
                      << merge.error.message << "\n" << merge.text << "\n";
            return 1;
        }
        std::cout << merge.text << "\n";
    } catch (const std::exception &e) {
        std::cerr << e.what() << "\n";
        return 1;
    }
    return 0;
}

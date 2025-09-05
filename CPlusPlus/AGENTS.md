# Repository Guidelines

## Project Structure & Module Organization
- `CMakeLists.txt`: CMake config (C++20 target).
- `main.cpp`: entry point and sample scaffold.
- `build/` or `cmake-build-*`: local build output (untracked).
- New samples: mirror repo layout when relevant (`Endpoint Examples/JSON Payload/` vs `Endpoint Examples/Multipart Payload/`). Name files after endpoints (e.g., `markdown.cpp`).

## Build, Test, and Development Commands
- Install deps via vcpkg (pick one HTTP client):
  - CPR: `vcpkg install cpr nlohmann-json`
  - cpp-httplib: `vcpkg install cpp-httplib[openssl] nlohmann-json`
- Configure with vcpkg toolchain:
  - macOS/Linux: `cmake -S . -B build -DCMAKE_TOOLCHAIN_FILE=$VCPKG_ROOT/scripts/buildsystems/vcpkg.cmake -DCMAKE_BUILD_TYPE=Debug`
  - Windows (PowerShell): `cmake -S . -B build -DCMAKE_TOOLCHAIN_FILE="$env:VCPKG_ROOT/scripts/buildsystems/vcpkg.cmake" -DCMAKE_BUILD_TYPE=Debug`
- Build: `cmake --build build -j`  (Release: set at configure)
- Run: `./build/CPlusPlus` (Windows: `build\CPlusPlus.exe`)
- Tests (if added): `ctest --test-dir build -j`

## Coding Style & Naming Conventions
- C++20; 4‑space indentation; consistent brace style.
- Files: `snake_case.cpp/.h`; types: `PascalCase`; funcs/vars: `snake_case`; constants/macros: `UPPER_SNAKE_CASE`; namespace: `pdfrest`.
- Use `clang-format` (LLVM/Google) and keep functions small/clear.
- HTTP/JSON via vcpkg: prefer `cpr` (batteries‑included) or `cpp-httplib`[openssl]; JSON via `nlohmann::json`.

### CMake (vcpkg) example
```
find_package(nlohmann_json CONFIG REQUIRED)
# choose one client
find_package(cpr CONFIG REQUIRED) # or: find_package(httplib CONFIG REQUIRED)
target_link_libraries(CPlusPlus PRIVATE nlohmann_json::nlohmann_json cpr::cpr)
```

## Testing Guidelines
- None present. If adding tests, use GoogleTest via vcpkg: `vcpkg install gtest` and wire CTest (`enable_testing(); add_test(...)`).
- Put tests in `tests/` and name after samples (e.g., `markdown_test.cpp`). Failures must return non‑zero.

## Commit & Pull Request Guidelines
- Commits: concise, imperative; prefixes welcome: `feat(sample)`, `fix(sample)`, `chore(build)`, `docs(cpp)`.
- PRs: include purpose, endpoints touched, build/run example (with toolchain flag), sample output/artifact, and linked issues. Avoid unrelated changes.

## Security & Configuration Tips
- Do not commit secrets. Read `PDFREST_API_KEY` from environment.
- Region/GDPR: `PDFREST_URL=https://eu-api.pdfrest.com` (default `https://api.pdfrest.com`).
- Optional cleanup: respect `PDFREST_DELETE_SENSITIVE_FILES=true` when implemented.
- Respect proxies via `HTTPS_PROXY`/`HTTP_PROXY`. Never print API keys.

## Sample Header Template
Place this at the top of each sample (before includes):
```
/*
 * What this sample does:
 * - Brief purpose and request style
 *
 * Setup (environment):
 * - Set PDFREST_API_KEY=your_api_key_here
 * - Optional: set PDFREST_URL (EU/GDPR: https://eu-api.pdfrest.com)
 *   More info: https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
 *
 * Usage: ./CPlusPlus <args>
 * Output: JSON to stdout; non‑2xx exits with concise error.
 */
```


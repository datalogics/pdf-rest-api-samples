# Repository Guidelines

## Project Structure & Module Organization
- `CMakeLists.txt`: CMake config (targets C++20).
- `main.cpp`: entry point and sample scaffold.
- `build/` or `cmake-build-*`: local build output (not required for source control).
- `.idea/`: IDE settings (optional).
- When adding new samples, mirror the repo’s language layout: consider `Endpoint Examples/JSON Payload/` (two‑step: upload then operate) and `Endpoint Examples/Multipart Payload/` (single multipart request). Keep filenames aligned to endpoint names (e.g., `markdown.cpp`).

## Build, Test, and Development Commands
- Configure: `cmake -S . -B build -DCMAKE_BUILD_TYPE=Debug`
- Build: `cmake --build build -j`  (release: `-DCMAKE_BUILD_TYPE=Release`)
- Run: `./build/CPlusPlus` (Windows: `build\CPlusPlus.exe`)
- Clean: `cmake --build build --target clean` (or remove `build/`)
- Optional tests (if added via CTest): `ctest --test-dir build -j`.

## Coding Style & Naming Conventions
- Language: C++20, 4‑space indentation, consistent brace style.
- Files: `snake_case.cpp/.h`; classes/types: `PascalCase`; functions/vars: `snake_case`; constants/macros: `UPPER_SNAKE_CASE`; namespace: `pdfrest`.
- Prefer `clang-format` if available (LLVM/Google style). Add a repo `.clang-format` when standardizing.
- HTTP client: prefer `libcurl` for multipart and headers; JSON handling via `nlohmann/json` if needed.

## Testing Guidelines
- No formal test suite present. If adding tests, place under `tests/`, integrate with CTest/GoogleTest, and wire into CMake (e.g., `enable_testing()` + `add_test(...)`).
- Name tests after the sample under test (e.g., `markdown_test.cpp`). Ensure `ctest` returns non‑zero on failures.

## Commit & Pull Request Guidelines
- Commits: imperative and scoped (e.g., "Add multipart markdown sample (C++)"). Reference endpoint and path when useful.
- PRs: include what/why, run commands used for verification, expected output snippet or artifact path, and linked issues. Avoid unrelated changes.

## Security & Configuration Tips
- Never commit secrets. Read `PDFREST_API_KEY` from the environment (e.g., `std::getenv`).
- Optional region override: `PDFREST_URL` (EU: `https://eu-api.pdfrest.com`).
- Optional clean‑up toggle: honor `PDFREST_DELETE_SENSITIVE_FILES=true` when implemented to delete uploaded/generated files.
- Log minimal diagnostics; never print API keys.


cmake_minimum_required(VERSION 3.16)

find_package(cpr CONFIG QUIET)
find_package(nlohmann_json CONFIG QUIET)

if (cpr_FOUND AND nlohmann_json_FOUND)
  add_executable(markdown_json markdown.cpp)
  target_link_libraries(markdown_json PRIVATE cpr::cpr nlohmann_json::nlohmann_json)
  target_compile_features(markdown_json PRIVATE cxx_std_20)

  add_executable(rasterized_pdf_json rasterized_pdf.cpp)
  target_link_libraries(rasterized_pdf_json PRIVATE cpr::cpr nlohmann_json::nlohmann_json)
  target_compile_features(rasterized_pdf_json PRIVATE cxx_std_20)

  add_executable(summarized_pdf_text_json summarized_pdf_text.cpp)
  target_link_libraries(summarized_pdf_text_json PRIVATE cpr::cpr nlohmann_json::nlohmann_json)
  target_compile_features(summarized_pdf_text_json PRIVATE cxx_std_20)

  add_executable(translated_pdf_text_json translated_pdf_text.cpp)
  target_link_libraries(translated_pdf_text_json PRIVATE cpr::cpr nlohmann_json::nlohmann_json)
  target_compile_features(translated_pdf_text_json PRIVATE cxx_std_20)

  add_executable(blank_pdf_json blank_pdf.cpp)
  target_link_libraries(blank_pdf_json PRIVATE cpr::cpr nlohmann_json::nlohmann_json)
  target_compile_features(blank_pdf_json PRIVATE cxx_std_20)
endif()

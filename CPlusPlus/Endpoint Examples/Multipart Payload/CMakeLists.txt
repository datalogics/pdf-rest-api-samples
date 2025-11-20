cmake_minimum_required(VERSION 3.16)

find_package(cpr CONFIG QUIET)

if (cpr_FOUND)
  add_executable(markdown_multipart markdown.cpp)
  target_link_libraries(markdown_multipart PRIVATE cpr::cpr)
  target_compile_features(markdown_multipart PRIVATE cxx_std_20)

  add_executable(rasterized_pdf_multipart rasterized_pdf.cpp)
  target_link_libraries(rasterized_pdf_multipart PRIVATE cpr::cpr)
  target_compile_features(rasterized_pdf_multipart PRIVATE cxx_std_20)

  add_executable(summarized_pdf_text_multipart summarized_pdf_text.cpp)
  target_link_libraries(summarized_pdf_text_multipart PRIVATE cpr::cpr)
  target_compile_features(summarized_pdf_text_multipart PRIVATE cxx_std_20)

  add_executable(translated_pdf_text_multipart translated_pdf_text.cpp)
  target_link_libraries(translated_pdf_text_multipart PRIVATE cpr::cpr)
  target_compile_features(translated_pdf_text_multipart PRIVATE cxx_std_20)

  add_executable(blank_pdf_multipart blank_pdf.cpp)
  target_link_libraries(blank_pdf_multipart PRIVATE cpr::cpr)
  target_compile_features(blank_pdf_multipart PRIVATE cxx_std_20)
endif()

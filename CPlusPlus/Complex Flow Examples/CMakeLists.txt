cmake_minimum_required(VERSION 3.16)

find_package(cpr CONFIG QUIET)
find_package(nlohmann_json CONFIG QUIET)

if (cpr_FOUND AND nlohmann_json_FOUND)
  add_executable(merge_different_file_types merge_different_file_types.cpp)
  target_link_libraries(merge_different_file_types PRIVATE cpr::cpr nlohmann_json::nlohmann_json)
  target_compile_features(merge_different_file_types PRIVATE cxx_std_20)
endif()

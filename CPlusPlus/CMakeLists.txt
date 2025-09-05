cmake_minimum_required(VERSION 4.0)
project(CPlusPlus)

set(CMAKE_CXX_STANDARD 20)

add_executable(CPlusPlus main.cpp)

# Dependencies via vcpkg
find_package(nlohmann_json CONFIG QUIET)
find_package(cpr CONFIG QUIET)

if (cpr_FOUND AND nlohmann_json_FOUND)
    add_executable(markdown_json markdown_json.cpp)
    target_link_libraries(markdown_json PRIVATE cpr::cpr nlohmann_json::nlohmann_json)
    target_compile_features(markdown_json PRIVATE cxx_std_20)
endif()

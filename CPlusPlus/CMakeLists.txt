cmake_minimum_required(VERSION 4.0)
project(CPlusPlus)

set(CMAKE_CXX_STANDARD 20)

# Place all executables in the top-level build directory for convenience
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR})
foreach(OUTPUTCONFIG DEBUG RELEASE RELWITHDEBINFO MINSIZEREL)
  string(TOUPPER ${OUTPUTCONFIG} OUTPUTCONFIG_UPPER)
  set(CMAKE_RUNTIME_OUTPUT_DIRECTORY_${OUTPUTCONFIG_UPPER} ${CMAKE_BINARY_DIR})
endforeach()

# Define sample targets in subdirectories
add_subdirectory("Endpoint Examples/JSON Payload")
add_subdirectory("Endpoint Examples/Multipart Payload")
add_subdirectory("Complex Flow Examples")

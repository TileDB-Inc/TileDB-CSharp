#
# CMakeLists.txt
#
#
# setup
cmake_minimum_required(VERSION 3.16)

set(TILEDB_VERSION "2.5.3" CACHE STRING "TileDB version")
set(TILEDB_GIT_TAG "dev" CACHE STRING "git branch")

project(TileDB-CSharp)

# C++11
set(CMAKE_CXX_STANDARD 11)
set(CMAKE_CXX_STANDARD_REQUIRED 11)
set(CMAKE_CXX_EXTENSIONS OFF)

# Release build by default
set(default_build_type "Release")
if (NOT CMAKE_BUILD_TYPE)
  set(CMAKE_BUILD_TYPE ${default_build_type})
endif()

# installation prefix
message(STATUS "CMAKE_SYSTEM_NAME:${CMAKE_SYSTEM_NAME}")
option(OVERRIDE_INSTALL_PREFIX "override default installation directory to ../dist" ON)
if (OVERRIDE_INSTALL_PREFIX)
#  set(PREFIX_REL_PATH "${CMAKE_SOURCE_DIR}/../dist/${CMAKE_SYSTEM_NAME}")
  set(PREFIX_REL_PATH "${CMAKE_SOURCE_DIR}/../TileDB.CSharp")
  get_filename_component(DEFAULT_PREFIX "${PREFIX_REL_PATH}" ABSOLUTE)
  set(CMAKE_INSTALL_PREFIX "${DEFAULT_PREFIX}" CACHE PATH "Default install prefix" FORCE)
  message(STATUS "Using default install prefix ${CMAKE_INSTALL_PREFIX}. To control CMAKE_INSTALL_PREFIX, set OVERRIDE_INSTALL_PREFIX=OFF")
#  set(CMAKE_INSTALL_BINDIR "${CMAKE_INSTALL_PREFIX}/lib")
#  set(CMAKE_INSTALL_LIBDIR "${CMAKE_INSTALL_PREFIX}/lib")  
  ### the following is to force zlib.dll in the lib directory
#  set(INSTALL_BIN_DIR "${CMAKE_INSTALL_PREFIX}/lib" CACHE INTERNAL "") 

endif()

if (WIN32) # Windows
  set(CMAKE_INSTALL_BINDIR "${CMAKE_INSTALL_PREFIX}/runtimes/win-x64/native")
  set(CMAKE_INSTALL_LIBDIR "${CMAKE_INSTALL_PREFIX}/runtimes/win-x64/native")
elseif(APPLE) # OSX
  set(CMAKE_INSTALL_BINDIR "${CMAKE_INSTALL_PREFIX}/runtimes/osx-x64/native")
  set(CMAKE_INSTALL_LIBDIR "${CMAKE_INSTALL_PREFIX}/runtimes/osx-x64/native")
else() # Linux
  set(CMAKE_INSTALL_BINDIR "${CMAKE_INSTALL_PREFIX}/runtimes/linux-x64/native")
  set(CMAKE_INSTALL_LIBDIR "${CMAKE_INSTALL_PREFIX}/runtimes/linux-x64/native")
endif()

message(STATUS "CMAKE_INSTALL_BINDIR: ${CMAKE_INSTALL_BINDIR}")
message(STATUS "CMAKE_INSTALL_LIBDIR: ${CMAKE_INSTALL_LIBDIR}")

# compiler flags
string(TOUPPER "${CMAKE_BUILD_TYPE}" BUILD_TYPE)
message(STATUS "BUILD_TYPE:${BUILD_TYPE}")
if(BUILD_TYPE STREQUAL "RELEASE")
  add_compile_options(-DNDEBUG)
endif()

if(BUILD_TYPE STREQUAL "DEBUG")
  add_compile_options(-DDEBUG )
endif()

## relative path
if(NOT APPLE)
  set(CMAKE_BUILD_RPATH "$\{ORIGIN\}")
endif()

######################################
# Build
######################################
# Enable testing
#enable_testing()

# src
add_subdirectory(src)

# test
#add_subdirectory(test)


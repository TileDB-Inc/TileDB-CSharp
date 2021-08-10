message(STATUS "Starting TileDB-CSharp build")

find_package(TileDB HINTS /usr/local/lib64/cmake/TileDB/)
if (NOT TileDB_FOUND)
  include(../cmake/TileDB.cmake)
endif()

##################################
# TileDB cxx api library
##################################
set(TILEDB_CXX_API_SOURCES
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_array.h
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_array_schema.h
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_array_util.h
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_array_util.cc
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_attribute.h
  #${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_column.h
  #${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_column.cc
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_config.h
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_context.h
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_core_interface.h
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_deleter.h
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_dimension.h
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_domain.h
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_enum.h
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_exception.h
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_filter.h
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_filter_list.h
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_group.h
  #${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_json.hpp
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_object.h
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_object_iter.h
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_query.h
   ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_query_condition.h
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_schema_base.h
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_stats.h
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_string_util.h
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_string_util.cc
 ## ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx.h
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_enum.h
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_type.h
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_utils.h
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_version.h
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_vfs.h
)

#######
# csharp wrapper sources
#######
if(${CMAKE_SYSTEM_NAME} STREQUAL "Linux")
  set(TILEDB_CSHARP_SOURCES
    ${CMAKE_CURRENT_SOURCE_DIR}/tiledb_csharp_cpp_wrapper_linux64.cxx
  )
else()
  set(TILEDB_CSHARP_SOURCES
    ${CMAKE_CURRENT_SOURCE_DIR}/tiledb_csharp_cpp_wrapper.cxx
  )
endif()
message(STATUS "TILEDB_CSHARP_SOURCES:${TILEDB_CSHARP_SOURCES}")

######
#tiledb csharp wrapper
######
add_library(tiledbcs
  SHARED
  ${TILEDB_CXX_API_SOURCES}
  ${TILEDB_CSHARP_SOURCES}

)

target_include_directories(tiledbcs
  PUBLIC
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api

)




if(UNIX)
set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -fpermissive")

add_definitions(
  "-DTILEDB_STATIC_DEFINE"

)
add_compile_options(
  "HAVE_PYCONFIG"
  "-fpermissive"
  "-D_GLIBCXX_USE_NANOSLEEP"
  "-D_THREAD_SAFE"
  "-D_XOPEN_SOURCE_EXTENEDED"
  "-D_GCC"
  "-D_PROTOTYPES"
  "-D_DRTP_V4"
  "-DLINUX"
  "-D_POSIX_THREADS"

)

if(APPLE)
   set_target_properties(tiledbcs
    PROPERTIES
      PREFIX ""
      SUFFIX ".dylib"
      INSTALL_RPATH "@loader_path/"
  )
else()
   set_target_properties(tiledbcs
    PROPERTIES
      PREFIX ""
      SUFFIX ".so"
      INSTALL_RPATH "$ORIGIN"
  )
endif()

target_link_libraries(tiledbcs
  PUBLIC
    pthread
    TileDB::tiledb_shared
#    -l:libarrow.so.200
#    -l:libarrow_python.so.200
)

endif()

if(MSVC)
set(CMAKE_CXX_FLAGS ${CMAKE_CXX_FLAGS}
 ### "HAVE_PYCONFIG"
    "WIN32"
    "_WIN32"
    "WIN32_LEAN_AND_MEAN"
    "_WIN64"
    "/wd\"4996\""
    "/wd\"4819\""
    "/wd\"4251\""
    "/wd\"4005\""
    "/wd\"4275\""
    "/wd\"4800\""
  )
  set_target_properties(tiledbcs
    PROPERTIES
      PREFIX ""
      SUFFIX ".dll"
  )

  target_link_libraries(tiledbcs
  TileDB::tiledb_shared
  )
#  target_link_libraries(pytiledb
#    PUBLIC
#      arrow
#      arrow_python
#  )

endif()

file(
  GLOB TILDB_LIBFILES
  ${tiledb_prebuilt_SOURCE_DIR}/lib/*.so*
  ${tiledb_prebuilt_SOURCE_DIR}/lib/*.dll*
  ${tiledb_prebuilt_SOURCE_DIR}/lib/*.lib*
  ${tiledb_prebuilt_SOURCE_DIR}/lib/*.dylib*
  ${tiledb_prebuilt_SOURCE_DIR}/bin/*.so*
  ${tiledb_prebuilt_SOURCE_DIR}/bin/*.dll*
  ${tiledb_prebuilt_SOURCE_DIR}/bin/*.lib*
  ${tiledb_prebuilt_SOURCE_DIR}/bin/*.dylib*

)
install(
  FILES
    ${TILDB_LIBFILES}
  DESTINATION
  ${CMAKE_INSTALL_LIBDIR}
)

install(
  TARGETS
    tiledbcs
  ARCHIVE
    DESTINATION ${CMAKE_INSTALL_BINDIR}
  RUNTIME
    DESTINATION ${CMAKE_INSTALL_LIBDIR}
  LIBRARY
    DESTINATION ${CMAKE_INSTALL_LIBDIR}
  PUBLIC_HEADER
    DESTINATION ${CMAKE_INSTALL_INCLUDEDIR}
)

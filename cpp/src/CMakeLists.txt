message(STATUS "Starting TileDB-CSharp build")


include(../cmake/TileDB.cmake)



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
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_exception.h 
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_filter.h 
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_filter_list.h 
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_group.h 
  #${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_json.hpp 
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_object.h  
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_object_iter.h 
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_query.h 
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_schema_base.h     
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_stats.h 
 # ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_string_util.h 
 # ${CMAKE_CURRENT_SOURCE_DIR}/tiledb/cxx_api/tiledb_cxx_string_util.cc 
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
set(TILEDB_CSHARP_SOURCES
  ${CMAKE_CURRENT_SOURCE_DIR}/tiledb_csharp_cpp_wrapper.cxx 
)

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

target_link_libraries(tiledbcs
  TileDB::tiledb_shared 
)


if(UNIX)

add_compile_options(
  "HAVE_PYCONFIG"
  "-D_GLIBCXX_USE_NANOSLEEP"
  "-D_THREAD_SAFE"
  "-D_XOPEN_SOURCE_EXTENEDED"
  "-D_GCC"
  "-D_PROTOTYPES"
  "-D_DRTP_V4"
  "-DLINUX"
  "-D_POSIX_THREADS"      
)

set_target_properties(tiledbcs
  PROPERTIES
    PREFIX ""
    SUFFIX ".so"
)
target_link_libraries(tiledbcs
  PUBLIC
    pthread
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
    DESTINATION ${CMAKE_INSTALL_BINDIR}
  LIBRARY 
    DESTINATION ${CMAKE_INSTALL_LIBDIR}
  PUBLIC_HEADER
    DESTINATION ${CMAKE_INSTALL_INCLUDEDIR}
)
 
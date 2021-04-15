include(FetchContent)
include(GNUInstallDirs)
###############################
# zlib  
###############################
FetchContent_Declare(
    zlib
    GIT_REPOSITORY "https://github.com/madler/zlib.git"
    GIT_TAG "v1.2.11"
)
FetchContent_GetProperties(zlib)
 
if(NOT zlib_POPULATED)
    FetchContent_Populate(zlib)
    message(STATUS "zlib_SOURCE_DIR:${zlib_SOURCE_DIR}")
    message(STATUS "zlib_BINARY_DIR:${zlib_BINARY_DIR}")
    include_directories(
      ${zlib_SOURCE_DIR} 
      ${zlib_BINARY_DIR}      
    )
 ##   set(INSTALL_BIN_DIR "${CMAKE_INSTALL_PREFIX}/lib")
    add_subdirectory(
      ${zlib_SOURCE_DIR} 
      ${zlib_BINARY_DIR}
      )
endif()
if(MSVC)

file(
  GLOB ZLIB_DLLFILES 
  ${zlib_BINARY_DIR}/Release/*.dll
)
install(
  FILES 
    ${ZLIB_DLLFILES}
  DESTINATION 
  ${CMAKE_INSTALL_LIBDIR}
)

endif()
 

#################################
# snappy
################################
#FetchContent_Declare(
#    snappy
#    GIT_REPOSITORY "https://github.com/google/snappy.git"
#    GIT_TAG "1.1.8"
#)
#FetchContent_GetProperties(snappy)
#if(NOT snappy_POPULATED)
#    FetchContent_Populate(snappy)
#    message(STATUS "snappy_SOURCE_DIR:${snappy_SOURCE_DIR}")
#    message(STATUS "snappy_BINARY_DIR:${snappy_BINARY_DIR}")
#    add_subdirectory(
#      ${snappy_SOURCE_DIR} 
#      ${snappy_BINARY_DIR}
#      )
#endif()
#set(SNAPPY_INSTALL ON)

###########################
# tiledb
############################
##TODO: generate more different libraries according to tiledb versions
message(STATUS "start to set tiledb for version:${TILEDB_VERSION}")

if(${TILEDB_VERSION} STREQUAL "nightly_build")
##TODO: change to nightly_build
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.2.7/tiledb-windows-2.2.7-a788ce5-full.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "67878e0274db413d22b738a45b6cc228badf084e")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.2.7/tiledb-macos-2.2.7-a788ce5-full.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "e1e0c78a3cdd25c7f9357125b67871fcfd5a8bfa")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.2.7/tiledb-linux-2.2.7-a788ce5-full.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "ad3ecd762fbd5ff6adf2fada5f923d2056a33a2f")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.2.7")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.2.7/tiledb-windows-2.2.7-a788ce5-full.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "67878e0274db413d22b738a45b6cc228badf084e")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.2.7/tiledb-macos-2.2.7-a788ce5-full.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "e1e0c78a3cdd25c7f9357125b67871fcfd5a8bfa")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.2.7/tiledb-linux-2.2.7-a788ce5-full.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "ad3ecd762fbd5ff6adf2fada5f923d2056a33a2f")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.2.6")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.2.6/tiledb-windows-2.2.6-b6926bc-full.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "700e5cdbaa77b00d31f498dbab353b06d4890ae7")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.2.6/tiledb-macos-2.2.6-b6926bc-full.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "348d56dede19a22e351571f1b5bdc5c1cba70684")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.2.6/tiledb-linux-2.2.6-b6926bc-full.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "198c84e74638d46949aad63881902eff095282f9")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.2.3")  
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.2.3/tiledb-windows-2.2.3-dbaf5ff-full.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "82eabce749f070f3a48095e229ec5f3389beee2f")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.2.3/tiledb-macos-2.2.3-dbaf5ff-full.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "8b07960e274d5eb156279edafc6f6ebc5d219ec8")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.2.3/tiledb-linux-2.2.3-dbaf5ff-full.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "817dfcbfc873a1728a66525d8c275e66d6742300")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.2.1")  
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.2.1/tiledb-windows-2.2.1-4744a3f-full.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "6b30dbd909ba92e78b9805434e316b5b93765442")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.2.1/tiledb-macos-2.2.1-4744a3f-full.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "66d81a4efba76966a8e37786f79b9825588e1e92")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.2.1/tiledb-windows-2.2.1-4744a3f-full.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "79f671ab8af89e6b2f270c59bbda4af0b973c2fc")
  endif()  
elseif(${TILEDB_VERSION} STREQUAL "2.1.0")  
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.1.0/tiledb-windows-2.1.0-1073faa-full.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "1d3e5437d105822b29a0881f5599512a59d50120")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.1.0/tiledb-macos-2.1.0-1073faa-full.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "bbce94181f3cf1ea279dd255f596dd3e90dcd7cb")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.1.0/tiledb-linux-2.1.0-1073faa-full.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "b748a1bb5aec052d9ec7c2ad4b39b50ae100cd9b")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.0.7")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.0.7/tiledb-windows-2.0.7-2058d3d.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "4E3BEED60F2F29B31DDA04CAA21DDE22B96B77B9")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.0.7/tiledb-macos-2.0.7-2058d3d.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "B5CF25FC57B89009D3F5E21AC902C7642BA295E6")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.0.7/tiledb-linux-2.0.7-2058d3d.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "556CCD6265A9F62884A8F798753625D7352ACCD5")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.0.0")  
#TODO no compiled binaries for downloading
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "")
    SET(TILEDB_DOWNLOAD_SHA1 "")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "")
    SET(TILEDB_DOWNLOAD_SHA1 "")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "")
    SET(TILEDB_DOWNLOAD_SHA1 "")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "1.7.0")  
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/1.7.0/tiledb-windows-x64-1.7.0.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "1520c6a19d082a61b36fe284f88c00d3cb3079ea")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "")
    SET(TILEDB_DOWNLOAD_SHA1 "")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "")
    SET(TILEDB_DOWNLOAD_SHA1 "")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "1.6.0")  
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/1.6.0/tiledb-windows-x64-1.6.0.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "0855d539091d74ba90d70d101c7cc66f0a5fbfa0")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "")
    SET(TILEDB_DOWNLOAD_SHA1 "")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "")
    SET(TILEDB_DOWNLOAD_SHA1 "")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "1.4.1")  
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/1.4.1/tiledb-windows-x64.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "6282adffb47749752a7811ff863c433cdea65e70")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "")
    SET(TILEDB_DOWNLOAD_SHA1 "")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "")
    SET(TILEDB_DOWNLOAD_SHA1 "")
  endif()
else()
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.2.6/tiledb-windows-2.2.6-b6926bc-full.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "700e5cdbaa77b00d31f498dbab353b06d4890ae7")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.2.6/tiledb-macos-2.2.6-b6926bc-full.tar.gz")
    SET(DOWNLOAD_SHA1 "348d56dede19a22e351571f1b5bdc5c1cba70684")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.2.6/tiledb-linux-2.2.6-b6926bc-full.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "198c84e74638d46949aad63881902eff095282f9")
  endif()
endif() ###

 


##fetch tiledb 
FetchContent_Declare(
  tiledb_prebuilt
  URL ${TILEDB_DOWNLOAD_URL}
  URL_HASH SHA1=${TILEDB_DOWNLOAD_SHA1}
)
FetchContent_GetProperties(tiledb_prebuilt)

if(NOT tiledb_prebuilt_POPULATED)
  FetchContent_Populate(tiledb_prebuilt)

  message(STATUS "tiledb_prebuilt_SOURCE_DIR:${tiledb_prebuilt_SOURCE_DIR}")
  message(STATUS "tiledb_prebuilt_BINARY_DIR:${tiledb_prebuilt_BINARY_DIR}")
#  add_subdirectory(
#    ${tiledb_prebuilt_SOURCE_DIR}
#    ${tiledb_prebuilt_BINARY_DIR}
#  )
endif()

 
include(${tiledb_prebuilt_SOURCE_DIR}/lib/cmake/TileDB/TileDBConfig.cmake)
 


#get_target_property(TILEDB_BINARY_DIR TileDB::tiledb_shared BINARY_DIR)
#get_target_property(TILEDB_IMPORTED_LINK_DEPENDENT_LIBRARIES_RELEASE TileDB::tiledb_shared IMPORTED_LINK_DEPENDENT_LIBRARIES_RELEASE)
get_target_property(TILEDB_IMPORTED_LOCATION_RELEASE TileDB::tiledb_shared IMPORTED_LOCATION_RELEASE)
get_filename_component(TILEDB_RELEASE_BINARY_DIR ${TILEDB_IMPORTED_LOCATION_RELEASE} PATH)
message(STATUS "TILEDB_BINARY_DIR:${TILEDB_RELEASE_BINARY_DIR}")
#message(STATUS "TILEDB dependent:${TILEDB_IMPORTED_LINK_DEPENDENT_LIBRARIES_RELEASE}")
#message(STATUS "TILEDB location release:${TILEDB_IMPORTED_LOCATION_RELEASE}")
#message(STATUS "TILEDB_RELEASE_BINARY_DIR:${TILEDB_RELEASE_BINARY_DIR}")

file(GLOB TILEDB_BIN_FILES_AND_DIRS "${TILEDB_RELEASE_BINARY_DIR}/*")
foreach(item ${TILEDB_BIN_FILES_AND_DIRS})
  if(IS_DIRECTORY "${item}")
    LIST(APPEND TILEDB_BIN_DIRS "${item}")
  else()
    LIST(APPEND TILEDB_BIN_FILES "${item}")
  endif()
endforeach()


message(STATUS "TILEDB install  ${TILEDB_BIN_FILES} to bin directory:${CMAKE_INSTALL_BINDIR}")
install(FILES ${TILEDB_BIN_FILES}
  DESTINATION ${CMAKE_INSTALL_LIBDIR}
)

if(TILEDB_BIN_DIRS)
message(STATUS "TILEDB install  ${TILEDB_BIN_DIRS} to bin directory:${CMAKE_INSTALL_BINDIR}")
  install( DIRECTORY ${TILEDB_BIN_DIRS}
    DESTINATION ${CMAKE_INSTALL_LIBDIR}
  )
endif()
 
 
include(FetchContent)
include(GNUInstallDirs)

############################
# Download pre-build tiledb or build from source
############################
message(STATUS "start to set tiledb for version:${TILEDB_VERSION}")

if(${TILEDB_VERSION} MATCHES "^([0-9]+)\\.([0-9]+)\\.([0-9]+)$")
message(STATUS "start to set TILEDB_DOWNLOAD_URL and TILEDB_DOWNLOAD_SHA1")
if(${TILEDB_VERSION} STREQUAL "2.3.0")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.3.0/tiledb-windows-2.3.0-a87da7f-full.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "CBE6F41108B49DA6ECA516A9A12BAD2064BD2240")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.3.0/tiledb-macos-2.3.0-a87da7f-full.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "BFA0247199BD6E2E08104534B45FF83123B7D4AB")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.3.0/tiledb-linux-2.3.0-a87da7f-full.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "15592594E38560A55FD7E3B7A052D9FF79F59A49")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.3.1")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.3.1/tiledb-windows-x86_64-2.3.1-6d36169.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "FF1DB0E556B2922D7DD0C2D7ECE6FDD03AEA1258")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.3.1/tiledb-macos-x86_64-2.3.1-6d36169.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "E9086167F6B9B5B304F3A724BAC08E9128AA7913")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.3.1/tiledb-linux-x86_64-2.3.1-6d36169.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "DD65523F22632161B43E2263AD6080F338F759D1")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.3.2")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.3.2/tiledb-windows-x86_64-2.3.2-4b563fe.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "5C0F8C7FDBD927151DFA927129ED87B7C073C5EE")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.3.2/tiledb-macos-x86_64-2.3.2-4b563fe.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "7610017E1835903286942C388FE58BF55CF8EC5C")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.3.2/tiledb-linux-x86_64-2.3.2-4b563fe.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "06360C3F6D6B96B2BE8038DE6E56ABB3C1A00E43")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.3.3")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.3.3/tiledb-windows-x86_64-2.3.3-9336d3f.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "7C777FC98E40E72400A590EF8EB9C23046D18689")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.3.3/tiledb-macos-x86_64-2.3.3-9336d3f.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "AE0D1606DAA6D984333E06D42DF25044BF7E347E")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.3.3/tiledb-linux-x86_64-2.3.3-9336d3f.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "803368D31C2EADF1B866A362CE0514073F84DAD9")
  endif()      
elseif(${TILEDB_VERSION} STREQUAL "2.3.4")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.3.4/tiledb-windows-x86_64-2.3.4-e19855e.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "C0F3D834519B2FB7D58CFB5BC8EB03F5004EDFAA")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.3.4/tiledb-macos-x86_64-2.3.4-e19855e.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "7B674AD6DE878017B400F0F3DCC3B8D6CF9982B3")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.3.4/tiledb-linux-x86_64-2.3.4-e19855e.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "B2BBDCBBAEF341AC7E14D428A2302D06BC899CF0")
  endif()    
elseif(${TILEDB_VERSION} STREQUAL "2.2.9")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.2.9/tiledb-windows-2.2.9-dc3bb54-full.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "41D72B04A2C503AEEEB45A893DD82B071C732D45")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.2.9/tiledb-macos-2.2.9-dc3bb54-full.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "6E6A04351A6DDA4C22BE15B639F48931423F3B86")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.2.9/tiledb-linux-2.2.9-dc3bb54-full.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "CB0AD1D6D942F926260B580F2C3073740DEAAD9F")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.2.8")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.2.8/tiledb-windows-2.2.8-6e7a5a2-full.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "54633F19A142FFB507E030633C8C713FB529169E")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.2.8/tiledb-macos-2.2.8-6e7a5a2-full.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "0037958A043F21B9392E3261C7495F81C6780C5B")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.2.8/tiledb-linux-2.2.8-6e7a5a2-full.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "C8CEF21926EE9860B307ACD200961DB0B5E693A3")
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
else()
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.3.0/tiledb-windows-2.3.0-a87da7f-full.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "CBE6F41108B49DA6ECA516A9A12BAD2064BD2240")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.3.0/tiledb-macos-2.3.0-a87da7f-full.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "BFA0247199BD6E2E08104534B45FF83123B7D4AB")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.3.0/tiledb-linux-2.3.0-a87da7f-full.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "15592594E38560A55FD7E3B7A052D9FF79F59A49")
  endif()
endif() 

message(STATUS "TILEDB_DOWNLOAD_URL: ${TILEDB_DOWNLOAD_URL}")
message(STATUS "TILEDB_DOWNLOAD_SHA1: ${TILEDB_DOWNLOAD_SHA1}")

endif()

 
### fetch prebuilt tiledb  
if(${TILEDB_VERSION} MATCHES "^([0-9]+)\\.([0-9]+)\\.([0-9]+)$")

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

else()
  message(STATUS "start to build tiledb from source")
  FetchContent_Declare(
    tiledb
    GIT_REPOSITORY https://github.com/TileDB-Inc/TileDB.git
    GIT_TAG ${TILEDB_GIT_TAG}
  )
  FetchContent_GetProperties(tiledb)
  if(NOT tiledb_POPULATED)
    FetchContent_Populate(tiledb)
    message(STATUS "tiledb_SOURCE_DIR:${tiledb_SOURCE_DIR}, cmake_generator:${CMAKE_GENERATOR}")
    message(STATUS "tiledb_BINARY_DIR:${tiledb_BINARY_DIR}")
    set(SUPERBUILD OFF)
    message("start to build tiledb at ${tiledb_BINARY_DIR}, buildtype:${CMAKE_BUILD_TYPE}...")
    if(WIN32)
      execute_process(
        COMMAND
          "${CMAKE_COMMAND}" -G "${CMAKE_GENERATOR}" -A x64
          -DCMAKE_INSTALL_PREFIX=${CMAKE_INSTALL_PREFIX}
          -DCMAKE_BUILD_TYPE=${CMAKE_BUILD_TYPE}
          -DTILEDB_S3=${TILEDB_S3}  
          -DTILEDB_AZURE=${TILEDB_AZURE}
          -DTILEDB_TESTS=OFF
          -DTILEDB_WERROR=OFF
          ${tiledb_SOURCE_DIR}
        WORKING_DIRECTORY
          ${tiledb_BINARY_DIR}
      )    
    else()
      execute_process(
        COMMAND
          "${CMAKE_COMMAND}" -G "${CMAKE_GENERATOR}"
          -DCMAKE_INSTALL_PREFIX=${CMAKE_INSTALL_PREFIX}
          -DCMAKE_BUILD_TYPE=${CMAKE_BUILD_TYPE}
          -DTILEDB_S3=${TILEDB_S3}  
          -DTILEDB_AZURE=${TILEDB_AZURE}
          -DTILEDB_TESTS=OFF
          -DTILEDB_WERROR=OFF
          ${tiledb_SOURCE_DIR}
        WORKING_DIRECTORY
         ${tiledb_BINARY_DIR}
      )
    endif()

    message("start to build tiledb at ${tiledb_BINARY_DIR} ...")
    execute_process(
      COMMAND
       "${CMAKE_COMMAND}" --build .  --config Release
      WORKING_DIRECTORY
       ${tiledb_BINARY_DIR}
    )

    message("start to build install-tiledb at ${tiledb_BINARY_DIR} ...")
    execute_process(
      COMMAND
        "${CMAKE_COMMAND}" --build .  --target install-tiledb  --config Release
      WORKING_DIRECTORY
       ${tiledb_BINARY_DIR}
    )    

  endif()

  message("start to include TileDBConfig.cmake")
  include( ${CMAKE_INSTALL_LIBDIR}/cmake/TileDB/TileDBConfig.cmake)

  file(GLOB TILEDB_BIN_FILES_AND_DIRS "${CMAKE_INSTALL_PREFIX}/bin/*")

endif()  


### copy files from /bin to /lib
foreach(item ${TILEDB_BIN_FILES_AND_DIRS})
  if(IS_DIRECTORY "${item}")
    LIST(APPEND TILEDB_BIN_DIRS "${item}")
  else()
    LIST(APPEND TILEDB_BIN_FILES "${item}")
  endif()
endforeach()

message(STATUS "TILEDB install  ${TILEDB_BIN_FILES} to lib directory:${CMAKE_INSTALL_BINDIR}")
install(FILES ${TILEDB_BIN_FILES}
  DESTINATION ${CMAKE_INSTALL_LIBDIR}
)

if(TILEDB_BIN_DIRS)
  message(STATUS "TILEDB install  ${TILEDB_BIN_DIRS} to lib directory:${CMAKE_INSTALL_BINDIR}")
  install( DIRECTORY ${TILEDB_BIN_DIRS}
    DESTINATION ${CMAKE_INSTALL_LIBDIR}
  )
endif() 
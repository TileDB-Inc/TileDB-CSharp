include(FetchContent)
include(GNUInstallDirs)

############################
# Download pre-build tiledb or build from source
############################
message(STATUS "start to set tiledb for version:${TILEDB_VERSION}")

if(${TILEDB_VERSION} MATCHES "^([0-9]+)\\.([0-9]+)\\.([0-9]+)$")
message(STATUS "start to set TILEDB_DOWNLOAD_URL and TILEDB_DOWNLOAD_SHA1")

if(${TILEDB_VERSION} STREQUAL "2.11.0")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.11.0-rc1/tiledb-windows-x86_64-2.11.0-rc1-34e5dbc.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "1bbaba7e7cbd2f123628819afb1db714ac3cedfd")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.11.0-rc1/tiledb-macos-x86_64-2.11.0-rc1-34e5dbc.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "baeea51ab0d78ff2f3d63fa3fc1fb3d381e6a9a7")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.11.0-rc1/tiledb-linux-x86_64-2.11.0-rc1-34e5dbc.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "ae9d38a199ed5b2b96fc2d003e14106b7ec16aab")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.10.4")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.10.4/tiledb-windows-x86_64-2.10.4-f2b5d11.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "68b4a8a22a3964efb8ef254854cf7d73f4fa2c50")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.10.4/tiledb-macos-x86_64-2.10.4-f2b5d11.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "7dffaf2ca3e3585641f1b6840618f3563b821041")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.10.4/tiledb-linux-x86_64-2.10.4-f2b5d11.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "b765f9bd29dceaf29be9bca098c18cb74c7c1c37")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.10.3")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.10.3/tiledb-windows-x86_64-2.10.3-7a5d1cd.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "aa4030a55339d23ef25a0d9593dbad76e9246742")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.10.3/tiledb-macos-x86_64-2.10.3-7a5d1cd.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "897c17caec445f74114ac1c6e7d453d6f6d1c2e7")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.10.3/tiledb-linux-x86_64-2.10.3-7a5d1cd.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "49387ad851693a748d17d638a7d716ad62ac8706")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.10.2")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.10.2/tiledb-windows-x86_64-2.10.2-9ab84f9.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "fe6dac320afb08dd3f2f40f2705306f6eb245f8b")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.10.2/tiledb-macos-x86_64-2.10.2-9ab84f9.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "2be62a6c48c0bd7aeea786414aa32aef26fe6a64")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.10.2/tiledb-linux-x86_64-2.10.2-9ab84f9.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "03f6d4892f11cbd939660b78c923325396bd600f")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.10.1")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.10.1/tiledb-windows-x86_64-2.10.1-6535d4c.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "d0c5ed50f3c5215cae8a4dd82f756624e43252aa")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.10.1/tiledb-macos-x86_64-2.10.1-6535d4c.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "68a43a62544ac849e1ca1b7c724f129dd7c8e50d")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.10.1/tiledb-linux-x86_64-2.10.1-6535d4c.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "8c48232cd52934724b2b7a254ebad6c27e5a7683")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.9.4")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.9.4/tiledb-windows-x86_64-2.9.4-4e14c01.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "f5280ce74b52cdc5071e03c98f9251d635e27fa4")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.9.4/tiledb-macos-x86_64-2.9.4-4e14c01.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "cc1db32693de9d8311ed245889d2aa3bb3eb5df9")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.9.4/tiledb-linux-x86_64-2.9.4-4e14c01.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "74b72ecd136490bd5aeedc75502efef95d27293b")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.9.1")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.9.1/tiledb-windows-x86_64-2.9.1-1855f7c.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "515661B7121CF1E73F2A079143E9AE4D2DF54EDB")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.9.1/tiledb-macos-x86_64-2.9.1-1855f7c.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "92C89D5FC5024CDF158E0534227C9037A1742919")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.9.1/tiledb-linux-x86_64-2.9.1-1855f7c.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "EEC2066E0CEC7F9BABEA0B5CDFC03DCA834B9F5D")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.8.2")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.8.2/tiledb-windows-x86_64-2.8.2-6f382df.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "32D8EDDAACF017B99D96F04746484D5A23974E9D")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.8.2/tiledb-macos-x86_64-2.8.2-6f382df.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "6852E8829117DCAB6E89067E4C1A26C7B151CCF2")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.8.2/tiledb-linux-x86_64-2.8.2-6f382df.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "9AF5C8DB1B5E7D7F16E39976AA433544BC78E7EC")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.8.1")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.8.1/tiledb-windows-x86_64-2.8.1-e9a945c.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "2D759909A7693626388F6FD5528FFF2823CA8450")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.8.1/tiledb-macos-x86_64-2.8.1-e9a945c.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "67F07C0EFA757FA0ECB3917ED2E60A56D477C5E9")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.8.1/tiledb-linux-x86_64-2.8.1-e9a945c.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "F60C8DFDDDF4EEDBFA414FE16F1ABE4A1A2D46F1")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.8.0")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.8.0/tiledb-windows-x86_64-2.8.0-f8efd39.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "EE28B243E1B025A1966643A4C924CB7485418285")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.8.0/tiledb-macos-x86_64-2.8.0-f8efd39.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "6E6033B26BCEA96B8A9D0948833958D5E8609E56")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.8.0/tiledb-linux-x86_64-2.8.0-f8efd39.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "8FE5E69D825829F10A4C35F529A9839DA0C254B0")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.7.1")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.7.1/tiledb-windows-x86_64-2.7.1-a942c71.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "FD3CD7C72A8C7056445B402622264CA3C3DA89CC")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.7.1/tiledb-macos-x86_64-2.7.1-a942c71.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "07074BAF703440C0A54D2EB41EB1F59C53A519F0")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.7.1/tiledb-linux-x86_64-2.7.1-a942c71.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "D61B01FDD6944F5AE3816426CF66350C54A2D737")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.6.4")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.6.4/tiledb-windows-x86_64-2.6.4-477532b.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "0A12236ACFB6F45CBCF1842464781A3F0F897E4E")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.6.4/tiledb-macos-x86_64-2.6.4-477532b.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "9783E3C3E574C146A42A673E8680D04B03D6D450")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.6.4/tiledb-linux-x86_64-2.6.4-477532b.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "5028B159F6EE6B8ABDE115EE3C431C6ABC396214")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.6.2")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.6.2/tiledb-windows-x86_64-2.6.2-bf10e49.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "4892B62C44CF897AC6C851703143C045D5AC3E82")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.6.2/tiledb-macos-x86_64-2.6.2-bf10e49.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "8312ECE1A7832E24E0B3B556B173B78255505E0E")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.6.2/tiledb-linux-x86_64-2.6.2-bf10e49.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "2596A6825D16A9E0119C8DB280EA7D4A64458072")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.6.1")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.6.1/tiledb-windows-x86_64-2.6.1-2f6b7f6.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "D59084906DC34E8B2416DCE7A203B125609286FB")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.6.1/tiledb-macos-x86_64-2.6.1-2f6b7f6.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "FC3B6642001C2AE036F8CFB990382F7BCFE27CD9")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.6.1/tiledb-linux-x86_64-2.6.1-2f6b7f6.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "F773AE6D4BFD9182CBA5F6879F595EAFBDA26006")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.6.0")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.6.0/tiledb-windows-x86_64-2.6.0-66f4b41.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "7FF516F55E19F460CE68EB1DCB52E17F72F845A3")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.6.0/tiledb-macos-x86_64-2.6.0-66f4b41.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "3C3A80AA2217B0C38E0FCCCA5C8FC4E63433E9FA")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.6.0/tiledb-linux-x86_64-2.6.0-66f4b41.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "57FDA45FD19F9E0DD7734DE2F1A8ECA427DE0B10")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.5.3")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.5.3/tiledb-windows-x86_64-2.5.3-dd6a41b.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "AE26920E9D7DEDCBAD3C0E5681DECD7A66D3DCF6")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.5.3/tiledb-macos-x86_64-2.5.3-dd6a41b.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "D29E2EE28AE9316FCB6710CA93C9E5D5A6C65795")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.5.3/tiledb-linux-x86_64-2.5.3-dd6a41b.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "AC28E99F0E1B445E72E2D2F0D84791CB3F40FF2E")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.5.2")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.5.2/tiledb-windows-x86_64-2.5.2-f9c058f.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "6DA1A3BC5BE2855EDD74CC521E6076A26F60ED19")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.5.2/tiledb-macos-x86_64-2.5.2-f9c058f.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "581168DF22F0F507DF87B8E52CEFD51630A99A70")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.5.2/tiledb-linux-x86_64-2.5.2-f9c058f.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "904D28E5480E0AFA04AE772D75AF737AF2ACC5E5")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.5.1")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.5.1/tiledb-windows-x86_64-2.5.1-5b65a96.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "D08658D08FAB8DA1C6D21C6C7AF68B02D9D5D9E7")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.5.1/tiledb-macos-x86_64-2.5.1-5b65a96.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "66521CCF7B5E6FE9BE29C0107E8BB433BB50CFAD")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.5.1/tiledb-linux-x86_64-2.5.1-5b65a96.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "D276396E0242C64D54479018517AD93206DC9E1B")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.4.4")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.4.4/tiledb-windows-x86_64-2.4.4-7257605.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "4E85442762F2B00761BDF08ADF47B2DF849D5B49")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.4.4/tiledb-macos-x86_64-2.4.4-7257605.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "188A5633E3E316782271B3BE770C8ABB92219A11")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.4.4/tiledb-linux-x86_64-2.4.4-7257605.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "F7A3FB9D72E17941103EC176B7B1F7F4770F71F9")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.4.2")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.4.2/tiledb-windows-x86_64-2.4.2-81a0286.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "2A1CE6FCDE6C2E6A744EB6F68914121EDFB33B94")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.4.2/tiledb-macos-x86_64-2.4.2-81a0286.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "54B13C8D6F57E2B8AF310F5C0331F8D95B0BEEA1")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.4.2/tiledb-linux-x86_64-2.4.2-81a0286.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "035AD2D81A2ED3B84B00125586ED68F8D75958E8")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.4.0")
  if (WIN32) # Windows
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.4.0/tiledb-windows-x86_64-2.4.0-baf64e1.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "CE9247503A7DF2A876717FD091CD23BBE1C32593")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.4.0/tiledb-macos-x86_64-2.4.0-baf64e1.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "78D74D6D3C3A259087CA00707554AC8545AA9311")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.4.0/tiledb-linux-x86_64-2.4.0-baf64e1.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "8FB19EFFA55BF4E7DAA38205C0C9CE32890CC821")
  endif()
elseif(${TILEDB_VERSION} STREQUAL "2.3.0")
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
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.11.0-rc1/tiledb-windows-x86_64-2.11.0-rc1-34e5dbc.zip")
    SET(TILEDB_DOWNLOAD_SHA1 "1bbaba7e7cbd2f123628819afb1db714ac3cedfd")
  elseif(APPLE) # OSX
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.11.0-rc1/tiledb-macos-x86_64-2.11.0-rc1-34e5dbc.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "baeea51ab0d78ff2f3d63fa3fc1fb3d381e6a9a7")
  else() # Linux
    SET(TILEDB_DOWNLOAD_URL "https://github.com/TileDB-Inc/TileDB/releases/download/2.11.0-rc1/tiledb-linux-x86_64-2.11.0-rc1-34e5dbc.tar.gz")
    SET(TILEDB_DOWNLOAD_SHA1 "ae9d38a199ed5b2b96fc2d003e14106b7ec16aab")
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

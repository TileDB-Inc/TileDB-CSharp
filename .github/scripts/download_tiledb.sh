set -e -x
wget https://github.com/TileDB-Inc/TileDB/releases/download/2.9.4/tiledb-windows-x86_64-2.9.4-4e14c01.zip
unzip tiledb-windows-x86_64-2.9.4-4e14c01.zip -d tiledb-windows
cp ./tiledb-windows/bin/tiledb.dll ./sources/TileDB.CSharp/runtimes/win-x64/native/
wget https://github.com/TileDB-Inc/TileDB/releases/download/2.9.4/tiledb-linux-x86_64-2.9.4-4e14c01.tar.gz
mkdir tiledb-linux
tar xvfz tiledb-linux-x86_64-2.9.4-4e14c01.tar.gz --directory tiledb-linux
cp ./tiledb-linux/lib/libtiledb.so.2.9 ./sources/TileDB.CSharp/runtimes/linux-x64/native/
wget https://github.com/TileDB-Inc/TileDB/releases/download/2.9.4/tiledb-macos-x86_64-2.9.4-4e14c01.tar.gz
mkdir tiledb-macos
tar xvfz tiledb-macos-x86_64-2.9.4-4e14c01.tar.gz --directory tiledb-macos
cp ./tiledb-macos/lib/libtiledb.dylib ./sources/TileDB.CSharp/runtimes/osx-x64/native/
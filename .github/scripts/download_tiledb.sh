set -e -x
wget https://github.com/TileDB-Inc/TileDB/releases/download/2.9.1/tiledb-windows-x86_64-2.9.1-1855f7c.zip
unzip tiledb-windows-x86_64-2.9.1-1855f7c.zip -d tiledb-windows
cp ./tiledb-windows/bin/tiledb.dll ./sources/TileDB.CSharp/runtimes/win-x64/native/
wget https://github.com/TileDB-Inc/TileDB/releases/download/2.9.1/tiledb-linux-x86_64-2.9.1-1855f7c.tar.gz
mkdir tiledb-linux
tar xvfz tiledb-linux-x86_64-2.9.1-1855f7c.tar.gz --directory tiledb-linux
cp ./tiledb-linux/lib/libtiledb.so.2.9 ./sources/TileDB.CSharp/runtimes/linux-x64/native/
wget https://github.com/TileDB-Inc/TileDB/releases/download/2.9.1/tiledb-macos-x86_64-2.9.1-1855f7c.tar.gz
mkdir tiledb-macos
tar xvfz tiledb-macos-x86_64-2.9.1-1855f7c.tar.gz --directory tiledb-macos
cp ./tiledb-macos/lib/libtiledb.dylib ./sources/TileDB.CSharp/runtimes/osx-x64/native/
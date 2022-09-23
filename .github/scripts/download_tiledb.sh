set -e -x
TAG=2.11.3
ID=a55a910

RELEASE=x86_64-$TAG-$ID
wget https://github.com/TileDB-Inc/TileDB/releases/download/$TAG/tiledb-windows-$RELEASE.zip
unzip tiledb-windows-$RELEASE.zip -d tiledb-windows
cp ./tiledb-windows/bin/tiledb.dll ./sources/TileDB.CSharp/runtimes/win-x64/native/

wget https://github.com/TileDB-Inc/TileDB/releases/download/$TAG/tiledb-linux-$RELEASE.tar.gz
mkdir tiledb-linux
tar xvfz tiledb-linux-$RELEASE.tar.gz --directory tiledb-linux
cp ./tiledb-linux/lib/libtiledb.so* ./sources/TileDB.CSharp/runtimes/linux-x64/native/

wget https://github.com/TileDB-Inc/TileDB/releases/download/$TAG/tiledb-macos-$RELEASE.tar.gz
mkdir tiledb-macos
tar xvfz tiledb-macos-$RELEASE.tar.gz --directory tiledb-macos
cp ./tiledb-macos/lib/libtiledb.dylib ./sources/TileDB.CSharp/runtimes/osx-x64/native/

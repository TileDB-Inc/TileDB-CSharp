set -e -x
TAG=2.11.0-rc1
ID=34e5dbc

RELEASE=x86_64-$TAG-$ID
wget https://github.com/TileDB-Inc/TileDB/releases/download/$TAG/tiledb-windows-$RELEASE.zip
unzip tiledb-windows-$RELEASE.zip -d tiledb-windows
cp ./tiledb-windows/bin/tiledb.dll ./sources/TileDB.CSharp/runtimes/win-x64/native/

wget https://github.com/TileDB-Inc/TileDB/releases/download/$TAG/tiledb-linux-$RELEASE.tar.gz
mkdir tiledb-linux
tar xvfz tiledb-linux-$RELEASE.tar.gz --directory tiledb-linux
cp ./tiledb-linux/lib/libtiledb.so.${TAG%.*} ./sources/TileDB.CSharp/runtimes/linux-x64/native/

wget https://github.com/TileDB-Inc/TileDB/releases/download/$TAG/tiledb-macos-$RELEASE.tar.gz
mkdir tiledb-macos
tar xvfz tiledb-macos-$RELEASE.tar.gz --directory tiledb-macos
cp ./tiledb-macos/lib/libtiledb.dylib ./sources/TileDB.CSharp/runtimes/osx-x64/native/

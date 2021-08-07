set -e -x
sudo ldconfig /home/runner/work/TileDB-CSharp/TileDB-CSharp/dist/Linux/lib
cd examples/TileDB.Example/bin/x64/Release/net5.0
dotnet TileDB.Example.dll
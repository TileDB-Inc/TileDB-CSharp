set -e -x
sudo ldconfig /home/runner/work/TileDB-CSharp/TileDB-CSharp/TileDB.CSharp/runtimes/linux-x64/native
cd examples/TileDB.Example/bin/x64/Release/net5.0
dotnet TileDB.Example.dll
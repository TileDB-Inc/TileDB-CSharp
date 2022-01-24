set -e -x
cd examples/TileDB-CSharp-Examples
dotnet build /p:Platform=x64 -c Release

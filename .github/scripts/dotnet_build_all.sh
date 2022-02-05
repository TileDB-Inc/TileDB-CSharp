set -e -x
cd sources/TileDB.CSharp
dotnet build /p:Platform=x64 -c Release

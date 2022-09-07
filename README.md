TileDB-CSharp

# Build
## Download or build tiledb core library

```bash
cd cpp
mkdir build && cd build
cmake ..
cmake --build . --target install
```
## Build TileDB.CSharp

```bash
cd sources/TileDB.CSharp
dotnet build /p:Platform=x64 -c Release
```
## Test TileDB.CSharp

```bash
cd tests/TileDB.CSharp.Test
dotnet test -c Release
```
# Old version

The SWIG-based 2.x version of this codebase is available in the
[archive](https://github.com/TileDB-Inc/TileDB-CSharp/tree/archive) branch.

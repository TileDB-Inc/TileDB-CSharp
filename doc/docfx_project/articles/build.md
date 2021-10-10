## Build from scratch for Windows
### Build c++ Wrapper tiledbcs.dll
```
cd cpp
mkdir build_win64
cd build_win64
cmake -G "Visual Studio 16 2019" -A x64 ..
cmake --build . --target install --config Release
```

### DotNet build csharp dll TileDB.CSharp.dll and benchmark
```
cd TileDB.CSharp
dotnet build /p:Platform=x64 -c Release
cd TileDB.CSharp/benchmark
dotnet build /p:Platform=x64 -c Release
```

## Build from scratch for Linux or macOS
### Build c++ Wrapper tiledbcs.so or tiledbcs.dylib
```
cd cpp
mkdir build
cd build
cmake ..
cmake --build . --target install --config Release
```
### DotNet build for csharp dll TileDB.CSharp.dll and benchmark
```
cd TileDB.CSharp
dotnet build /p:Platform=x64 -c Release
cd TileDB.CSharp/benchmark
dotnet build /p:Platform=x64 -c Release
```

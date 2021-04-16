# TileDB CSharp Bindings
This repo provides TileDB C# bindings via swig.

## Quick Links for TileDB
* Quickstart: https://docs.tiledb.com/quickstart
* Installation: https://docs.tiledb.com/installation
* Full documentation: https://docs.tiledb.com

## Quick Installation
We will upload to nuget soon.

## Build
### Supported Platforms
Currently the following platforms are supported:
* Windows
* Linux
* macOS

### Build c++ Wrapper for Windows
```
cd cpp
mkdir build_win64
cd build_win64
cmake -G "Visual Studio 14 2015" -A x64 ..
cmake --build . --target install --config Release
```
### Build c++ Wrapper for Linux or macOS
```
cd cpp
mkdir build
cd build
cmake ..
cmake --build . --target install --config Release
```
### DotNet build for Windows, Linux or macOS
```
cd csharp/TileDB.CSharp/TileDB.CSharp
dotnet build /p:Platform=x64 -c Release
cd ../TileDB.CSharp.Benchmark
dotnet build /p:Platform=x64 -c Release
```
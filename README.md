<a href="https://tiledb.com"><img src="https://github.com/TileDB-Inc/TileDB/raw/dev/doc/source/_static/tiledb-logo_color_no_margin_@4x.png" alt="TileDB logo" width="400"></a>


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
cmake -G "Visual Studio 16 2019" -A x64 ..
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

### Examples for Windows, Linux or macOS
#### Build an example
```
cd examples/TileDB.Example
dotnet build /p:Platform=x64 -c Release
```
#### Run the example on Windows
```
cd examples/TileDB.Example/bin/x64/Release/net5.0
.\TileDB.Example.exe
```
#### Run the example on Linux or macOS
```
cd examples/TileDB.Example/bin/x64/Release/net5.0
dotnet TileDB.Example.dll
```

# Building against TileDB.CSharp

To build your own project, you can add `TileDB.CSharp` as a `ProjectReference`
in your csproj file:

```
  <ItemGroup>
    <ProjectReference Include="path/to/TileDB.CSharp\TileDB.CSharp.csproj"/>
  </ItemGroup>
```

See sample usage in the [`tests/TileDBTests.csproj`](tests/TileDBTests.csproj)
project.

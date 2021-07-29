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

#### To build your own example, you can add the reference library information in you csproj file
##### For Windows
```
  <ItemGroup Condition=" '$(OS)' == 'Windows_NT' ">
    <None Include="..\..\dist\Windows\lib\tiledb.dll" Link="tiledb.dll">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
    <None Include="..\..\dist\Windows\lib\tiledbcs.dll" Link="tiledbcs.dll">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
    <None Include="..\..\dist\Windows\lib\zlib.dll" Link="zlib.dll">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
  </ItemGroup>

  <ItemGroup Condition=" '$(OS)' == 'Windows_NT' ">
    <Reference Include="TileDB.CSharp">
      <HintPath>..\..\dist\Windows\lib\net5.0\TileDB.CSharp.dll</HintPath>
    </Reference>
  </ItemGroup>
```
##### For macOS
```
  <ItemGroup Condition=" '$([System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform($([System.Runtime.InteropServices.OSPlatform]::OSX)))' ">
    <None Include="..\..\dist\Darwin\lib\libtiledb.dylib" Link="libtiledb.dylib">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
    <None Include="..\..\dist\Darwin\lib\tiledbcs.dylib" Link="tiledbcs.dylib">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
    <None Include="..\..\dist\Darwin\lib\libz.dylib" Link="libz.dylib">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
  </ItemGroup>
  <ItemGroup Condition=" '$([System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform($([System.Runtime.InteropServices.OSPlatform]::OSX)))' ">
    <Reference Include="TileDB.CSharp">
      <HintPath>..\..\dist\Darwin\lib\net5.0\TileDB.CSharp.dll</HintPath>
    </Reference>
  </ItemGroup>   
```
##### For Linux
```
  <ItemGroup Condition=" '$([System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform($([System.Runtime.InteropServices.OSPlatform]::Linux)))' ">
    <None Include="..\..\dist\Linux\lib\libtiledb.so.2.3" Link="libtiledb.so.2.3">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
    <None Include="..\..\dist\Linux\lib\tiledbcs.so" Link="tiledbcs.so">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
    <None Include="..\..\dist\Linux\lib\libz.so" Link="libz.so">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
  </ItemGroup>
  <ItemGroup Condition=" '$([System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform($([System.Runtime.InteropServices.OSPlatform]::Linux)))' ">
    <Reference Include="TileDB.CSharp">
      <HintPath>..\..\dist\Linux\lib\net5.0\TileDB.CSharp.dll</HintPath>
    </Reference>
  </ItemGroup>
```


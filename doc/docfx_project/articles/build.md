 
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
cmake --build . --target install --config Releasecd
```
### DotNet build for Windows, Linux or macOS
```
cd csharp/TileDB.CSharp/TileDB.CSharp
dotnet build /p:Platform=x64 -c Release
cd ../TileDB.CSharp.Benchmark
dotnet build /p:Platform=x64 -c Release
```

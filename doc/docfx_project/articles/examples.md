# Examples for Windows,Linux and macOS
### Build an example
```
cd examples/TileDB.Example
dotnet build /p:Platform=x64 -c Release
```

### Run the example on Windows
```
cd examples/TileDB.Example/bin/x64/Release/net5.0
.\TileDB.Example.exe
```

### Run the example on Linux
```
cd examples/TileDB.Example/bin/x64/Release/net5.0
export LD_LIBRARY_PATH=$LD_LIBRARy_PATH:./
dotnet TileDB.Example.dll
```

### Run the example on macOS
```
cd examples/TileDB.Example/bin/x64/Release/net5.0
export DYLD_LIBRARY_PATH=$DYLD_LIBRARy_PATH:./
dotnet TileDB.Example.dll
```
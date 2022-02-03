## Generated on macOS:
```
export DYLD_LIBRARY_PATH=/path/to/libclang/runtimes/osx-x64/native/
export TILEDB_DIR=/path/to/TileDB/dist
ClangSharpPInvokeGenerator -I $TILEDB_DIR/include/ --config generate-helper-types --file $TILEDB_DIR/include/tiledb/tiledb.h @scripts/generate/generate.
```

## Generated on windows(copy tiledb_export.h to tiledb/sm/c_api):
```
dotnet tool install --global ClangSharpPInvokeGenerator --version 13.0.0-beta1
cd tildb/sm/c_api
ClangSharpPInvokeGenerator -n TileDB.Interop  -f tiledb.h  -c generate-helper-types --methodClassName
libtiledb  -o LibTileDB.cs
```

## Replacements in LibTileDB.cs

1. [DllImport("libtiledb", --> [DllImport(LibDllImport.Path, 
2. Comment all  *_dump functions, 
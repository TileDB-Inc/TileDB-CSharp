## Generated on macOS:
```
export DYLD_LIBRARY_PATH=/path/to/libclang/runtimes/osx-x64/native/
export TILEDB_DIR=/path/to/TileDB/dist
ClangSharpPInvokeGenerator -I $TILEDB_DIR/include/ --config generate-helper-types --file $TILEDB_DIR/include/tiledb/tiledb.h @scripts/generate/generate.
```

## Generated on windows(copy tiledb_export.h to tiledb/sm/c_api):
```
cd tildb/sm/c_api
ClangSharpPInvokeGenerator -n TileDB  -f tiledb.h  -c generate-helper-types --methodClassName
libtiledb  -o temp.cs
```

## Replacements in LibTileDB.cs

1. [DllImport("libtiledb", --> [DllImport(LibDllImport.Path, CharSet = CharSet.Ansi,
2. [NativeTypeName("const char *")] sbyte* --> [NativeTypeName("const char *")][MarshalAs(UnmanagedType.LPStr)] string
3. [NativeTypeName("const char **")] sbyte** --> [NativeTypeName("const char **")] out IntPtr
## Generating LibTileDB.cs on macOS:

Note: prerequisite installation instructions below if needed.

```
export DYLD_LIBRARY_PATH=/path/to/libclang/runtimes/osx-x64/native/
export TILEDB_DIR=/path/to/TileDB/dist

ClangSharpPInvokeGenerator -I /Library/Developer/CommandLineTools/SDKs/MacOSX.sdk/usr/include  -I $TILEDB_DIR/include/ --file $TILEDB_DIR/include/tiledb/tiledb.h @scripts/generate/generate.rsp -l libtiledb
```

## Generated on windows (copy tiledb_export.h to tiledb/sm/c_api):

```
dotnet tool install --global ClangSharpPInvokeGenerator --version 13.0.0-beta1
cd tildb/sm/c_api
ClangSharpPInvokeGenerator -n TileDB.Interop  -f tiledb.h  -c generate-helper-types --methodClassName
libtiledb  -o LibTileDB.cs
```

## Replacements in LibTileDB.cs

1. [DllImport("libtiledb", --> [DllImport(LibDllImport.Path,
2. Comment all  *_dump functions,

# Generator Installation

## macOS

- Follow instructions [here](https://github.com/dotnet/ClangSharp#building-native) but skip first step (LLVM build) and install from homebrew instead.
    - note: if you have macOS 11+, you can use the release builds from LLVM upstream linked [here](https://releases.llvm.org/download.html) to [github release page](https://github.com/llvm/llvm-project/releases).

- Build `ClangSharpPInvokeGenerator` locally:

    ```
    cd <path>/ClangSharp/sources/ClangSharpPInvokeGenerator
    dotnet build
    ```

- Set DYLD_LIBRARY_PATH for the native `libClangSharp` lib directory:

    ```
    export DYLD_LIBRARY_PATH=<path>/ClangSharp/artifacts/bin/native/lib
    ```

- Now verify the build with:

    ```
    <path>/ClangSharp/artifacts/bin/sources/ClangSharpPInvokeGenerator/Debug/net6.0/ClangSharpPInvokeGenerator --version
    ```

    (should output 13.0)


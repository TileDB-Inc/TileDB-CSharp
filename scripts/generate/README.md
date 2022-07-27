## Generating LibTileDB.cs on macOS:

Note: prerequisite installation instructions below if needed.

```
export DYLD_LIBRARY_PATH=/path/to/libclang/runtimes/osx-x64/native/
export TILEDB_DIR=/path/to/TileDB/dist

ClangSharpPInvokeGenerator -I /Library/Developer/CommandLineTools/SDKs/MacOSX.sdk/usr/include  -I $TILEDB_DIR/include/ --file $TILEDB_DIR/include/tiledb/tiledb.h $TILEDB_DIR/include/tiledb/tiledb_experimental.h @scripts/generate/generate.rsp -l libtiledb
```

## Generated on windows (copy tiledb_export.h to tiledb/sm/c_api):

```
dotnet tool install --global ClangSharpPInvokeGenerator --version 13.0.0-beta1
cd tildb/sm/c_api
ClangSharpPInvokeGenerator -n TileDB.Interop  -f tiledb.h tiledb_experimental.h -c latest-codegen unix-types multi-file generate-helper-types --methodClassName Methods  -l libtiledb   --output sources/TileDB.Interop

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

## linux

Install `dotnet` core SDK

```bash
sudo apt-get update; \
  sudo apt-get install -y apt-transport-https && \
  sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-6.0
```

Install Generator

```bash
dotnet tool install --local ClangSharpPInvokeGenerator --version 14.0.0-beta2
```

Make an alias in `~/.zshrc`

```bash
vi ~/.zshrc
...
alias csgen=~/.dotnet/tools/ClangSharpPInvokeGenerator
...
source ~/.zshrc 
```

Install specific version of TileDB in `/usr/local`

```bash
git clone https://github.com/TileDB-Inc/TileDB.git -b 2.10.2
mv TileDB TileDB_2.10.2
cd TileDB_2.10.2
mkdir build && cd build
#cmake 3.22.2
cmake -DTILEDB_VERBOSE=OFF -DTILEDB_S3=ON -DTILEDB_SERIALIZATION=ON -DTILEDB_AZURE=ON -DCMAKE_BUILD_TYPE=Debug -DCMAKE_INSTALL_PREFIX=/usr/local ..
make -j12
sudo make -C tiledb install
```

```bash
sudo ldconfig
```

Add to `~/.zshrc`

```python
vi ~/.zshrc
...
export TILEDB_DIR=/usr/local
...
source ~/.zshrc
```

Build `libClangSharp`

ClangSharp provides a helper library, `libClangSharp`, that exposes additional functionality that is not available in `libClang`

Building this requires [CMake 3.13 or later](https://cmake.org/download/) as well as a version of MSVC or Clang that supports C++ 17.

Get `clang`

```bash
cd ~/workspace/tiledb
wget https://github.com/llvm/llvm-project/releases/download/llvmorg-14.0.0/clang+llvm-14.0.0-x86_64-linux-gnu-ubuntu-18.04.tar.xz
tar -xvf clang+llvm-14.0.0-x86_64-linux-gnu-ubuntu-18.04.tar.xz
rm clang+llvm-14.0.0-x86_64-linux-gnu-ubuntu-18.04.tar.xz
```

Clone generator

```bash
cd ~/workspace/tiledb
git clone https://github.com/dotnet/clangsharp
cd clangsharp
mkdir artifacts/bin/native
cd artifacts/bin/native
```

Build

```bash
cmake -DPATH_TO_LLVM=~/workspace/tiledb/clang+llvm-14.0.0-x86_64-linux-gnu-ubuntu-18.04 ../../..
make
#cd lib
#sudo cp libClangSharp.so libClangSharp.so.14.0.0 /usr/lib
sudo make install
```

```bash
sudo ldconfig
```

```bash
sudo cp ~/workspace/tiledb/clang+llvm-14.0.0-x86_64-linux-gnu-ubuntu-18.04/lib/libclang.so /usr/local/lib
```

```bash
sudo ldconfig
```

```bash
cd ~/workspace/tiledb/TileDB-CSharp-tmp
csgen -I $TILEDB_DIR/include/ -I ~/workspace/tiledb/clang+llvm-14.0.0-x86_64-linux-gnu-ubuntu-18.04/lib/clang/14.0.0/include/ --file $TILEDB_DIR/include/tiledb/tiledb.h $TILEDB_DIR/include/tiledb/tiledb_experimental.h @scripts/generate/generate.rsp -l libtiledb
```

`~/workspace/tiledb/TileDB-CSharp-tmp/sources/TileDB.Interop/Methods.cs` should be updated

In `~/workspace/tiledb/TileDB-CSharp-tmp/sources/TileDB.Interop/Methods.cs` do 

```bash
- Comment all *_dump functions
- Remove duplicate signatures From `MethodsExperimental.cs` (currently all signatures are duplicated)
```

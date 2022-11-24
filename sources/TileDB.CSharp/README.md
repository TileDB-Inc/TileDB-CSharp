# TileDB.CSharp

This package provides a C# interface to the [TileDB Embedded](https://tiledb.com/products/tiledb-embedded) storage engine.

## Usage

After installing the package, see [the official documentation](https://docs.tiledb.com/main/) and [the C# example project](https://github.com/TileDB-Inc/TileDB-CSharp/tree/main/examples/TileDB.CSharp.Example) to learn how to use it.

### Resolving `DllNotFoundException`s

To reduce the download size, the TileDB Embedded native library is downloaded only on [RID](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog)-specific builds. If you are getting `DllNotFoundException`s that the library `tiledb` is not found, you have to specify an RID, and you will get a warning if you don't. There are many ways to do it:

*
    __From the project file:__

    Add one of the following properties to your project:

    ```xml
    <PropertyGroup>
        <RuntimeIdentifier>win-x64</RuntimeIdentifier>
        <!-- Use this instead to use the RID of your build machine. -->
        <UseCurrentRuntimeIdentifier>true</UseCurrentRuntimeIdentifier>
    </PropertyGroup>
    ```

*
    __From the .NET CLI:__

    When building or publishing a project you can specify the `-r <your_rid>` option to provide an RID.

> Consult the .NET documentation for more information.

Native binaries are provided for the following RIDs, and all that derive from them such as `win10-x64` or `ubuntu-x64`:

* `win-x64`
* `linux-x64`
* `osx-x64`
* `osx-arm64`

### Patching TileDB Embedded

You can update to a newer patch version of TileDB Embedded by explicitly specifying a version of the [`TileDB.Native`](https://nuget.org/packages/TIleDB.Native) package in your project.

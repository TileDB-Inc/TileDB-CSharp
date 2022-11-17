# TileDB.CSharp

This package provides a C# interface to the [TileDB Embedded](https://tiledb.com/products/tiledb-embedded) storage engine.

## Usage

After installing the package, see [the official documentation](https://docs.tiledb.com/main/) and [the C# example project](https://github.com/TileDB-Inc/TileDB-CSharp/tree/main/examples/TileDB.CSharp.Example) to learn how to use it.

### Resolving `DllNotFoundException`s

For space reasons the TileDB Embedded native library is downloaded only on [RID](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog)-specific builds. If you are getting `DllNotFoundException`s that the library `tiledb` is not found, you have to specify an RID. There are many ways to do it:

*
    __From the project file:__

    Add one of the following properties to your project:

    ```xml
    <PropertyGroup>
        <!-- Use this if you are targeting only one RID. -->
        <RuntimeIdentifier>win-x64</RuntimeIdentifier>
        <!-- Use this if you are targeting multiple RIDs. -->
        <RuntimeIdentifiers>win-x64;linux-x64</RuntimeIdentifiers>
        <!-- Use this in to use an RID based on the machine that builds the project.
        Useful in development. -->
        <UseCurrentRuntimeIdentifier>true</UseCurrentRuntimeIdentifier>
    </PropertyGroup>
    ```

*
    __From the .NET CLI:__

    When building or publishing a project you can to specify the `-r <your_rid>` option to provide an RID, or the `--use-current-runtime true` option to use the RID of the building machine.

> Consult the .NET documentation for more info.

The supported RIDs are:

* `win-x64`
* `linux-x64`
* `osx-x64`
* `osx-arm64`

### Updating TileDB Embedded

TileDB Embedded is distributed in a separate NuGet package from the C# interface, which means that you can use a newer version of it by updating the `TileDB.Native` package in your project.

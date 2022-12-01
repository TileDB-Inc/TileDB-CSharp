![Licensed under the MIT License](https://img.shields.io/github/license/TileDB-Inc/TileDB-CSharp.svg)
[![NuGet](https://img.shields.io/nuget/v/TileDB.CSharp.svg)](https://nuget.org/packages/TileDB.CSharp)
[![Test](https://github.com/TileDB-Inc/TileDB-CSharp/actions/workflows/tiledb-csharp.yml/badge.svg?branch=main&event=push)](https://github.com/TileDB-Inc/TileDB-CSharp/actions/workflows/tiledb-csharp.yml)

# TileDB-CSharp

This repository contains the official C# bindings of the [TileDB Embedded](https://tiledb.com/products/tiledb-embedded) storage engine. See more information in [the README of the library project](sources/TileDB.CSharp/README.md).

## Install

The library is available on [NuGet](https://nuget.org/packages/TileDB.CSharp).

## Build

```bash
cd sources/TileDB.CSharp
dotnet build -c Release
```

## Test

```bash
cd tests/TileDB.CSharp.Test
dotnet test -c Release
```

## Old version

The SWIG-based 2.x version of this codebase is available in the [`archive`](https://github.com/TileDB-Inc/TileDB-CSharp/tree/archive) branch.

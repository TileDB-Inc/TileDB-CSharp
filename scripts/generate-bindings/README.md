# Generating TileDB native bindings

`GenerateBindings.proj` contains an MSBuild script that downloads the header files for a given version and uses `ClangSharpPInvokeGenerator` to generate the C# bindings to the TileDB C API.

## Prerequisites

You need to go to [TileDB's GitHub Releases page](https://github.com/TileDB-Inc/TileDB/releases) and find the version and 7-digit commit ID for your release.

## Automatic generation

The easiest way to generate bindings is by dispatching the [`generate-bindings` workflow](../../.github/workflows/generate-bindings.yml). It will run the script and submit a Pull Request with the changes.

## Manual generation

The script can be ran locally with the `dotnet msbuild ./scripts/generate-bindings/GenerateBindings.proj -p:Version=<version> -p:VersionTag=<commit-id> /restore` command.

Only Windows is supported.

# TileDB native NuGet package generator

This directory contains a script that:

* Downloads the Core binaries from GitHub Releases.
* Creates one NuGet package for each supported platform.
* Creates one NuGet metapackage that routes to the packages above according to the platform.

The script's source is in `GenerateNuGetPackages.proj`. The other files specify the properties of the packages.

## Usage

```
dotnet pack ./GenerateNuGetPackages.proj -p:Version=<version> -p:VersionTag=<version-tag>
```

> `version-tag` is the seven-character commit hash of the release.

The packages will be written in the `packages` subdirectory. You can change the output path with the `-p:OutDir=` option.

The script is incremental, which means that it will try to avoid doing work that is already done. To be extra sure that no stale data is being used, run `dotnet clean` before packing.

## Supporting new platforms

At the beginning of the file there is an `ItemGroup` that specifies `NativePlatform` items. To add support for a new platform you have to add a new item to this group. These are the metadata you have to define:

|Name|Value|
|----|-----|
|`Include` (referred in the script as `Identity`; technically not part of metadata)|The platform's name as written in the GitHub Releases.|
|`RuntimeId`|The platform's [.NET Runtime Identifier](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog).|
|`LibraryPath`|The native library's path, relative from the downloaded GitHub Releases artifact.|
|`ArchiveExtension`|The artifact archive's extension, without the leading dot. Optional, defaults to `tar.gz`.|

## Development packages

The script also supports generating development editions of the packages. This is used in nightly builds to allow testing with the binaries built from the `dev` and `release` branches. They have `Local.` prepended to their ID and their version is always `0.0.0-local+<branch>`. The build metadata that come after the `+` are used for informative purposes and are not a required part of the package's version.

Grabbing the development packages from the nightly build's artifacts is preferred over manually creating them, but if you want to (for example to test Core binaries from a different branch), you can follow these steps:

1. Build the Core.
2. Copy the Core's `dist` folder to the `temp` subdirectory of this repository, renaming it to the _GitHub Releases_ name of the platform.
3. Run `dotnet pack -p:DevelopmentBuild=true -p:VersionTag=<branch>`. `VersionTag` is optional.

When building development packages you don't have to include the artifacts for all platforms; the script will skip those whose artifacts do not exist.

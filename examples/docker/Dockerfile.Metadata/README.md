# Example Dockerfile for Metadata example

This Dockerfile builds example TileDB.Metadata inside the Microsoft dotnet images.


## Building

From *base* of TileDB-CSharp source tree:

```
docker build -t example_metadata:1.0 - <examples/docker/Dockerfile.Metadata/Dockerfile 
```

## Usage

```
docker run -ti example_metadata:1.0 /bin/bash
```

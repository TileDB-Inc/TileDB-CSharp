# Example Dockerfile on Alpine

This Dockerfile builds TileDB-CSharp inside the Microsoft dotnet images for Alpine.

## Usage

```
docker run -ti tiledb/tiledb-csharp:0.0.1 bash
```

## Building

From *base* of TileDB-CSharp source tree:

```
docker build -f examples/docker/Dockerfile .
```

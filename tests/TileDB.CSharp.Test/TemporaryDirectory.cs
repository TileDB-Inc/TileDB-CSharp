using System;
using System.IO;

namespace TileDB.CSharp.Test;

internal readonly struct TemporaryDirectory : IDisposable
{
    private readonly string _path;

    public TemporaryDirectory(string directoryName)
    {
        _path = Path.Join(Path.GetTempPath(), "tiledb-csharp-tests", directoryName);
        DeleteDirectory(_path);
        Directory.CreateDirectory(_path);
    }

    public void Dispose()
    {
        DeleteDirectory(_path);
    }

    public static implicit operator string(TemporaryDirectory directory) => directory._path;

    public static void DeleteDirectory(string directory)
    {
        try
        {
            Directory.Delete(directory, true);
        }
        catch (DirectoryNotFoundException)
        {
            // Don't fail if the directory does not exist.
        }
    }
}

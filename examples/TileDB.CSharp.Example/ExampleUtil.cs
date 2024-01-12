using System.IO;

namespace TileDB.CSharp.Examples;

public static class ExampleUtil
{
    static ExampleUtil()
    {
        if (!Directory.Exists(MakeExamplePath("")))
        {
            Directory.CreateDirectory(MakeExamplePath(""));
        }
    }

    /// <summary>
    /// Normalizes temp directory
    /// </summary>
    /// <param name="tempFile">Name of file to create temp path</param>
    /// <returns>A full path to the local tempFile</returns>
    public static string MakeTempPath(string tempFile) =>
        Path.Join(Path.Join(Path.GetTempPath(), "tiledb-csharp-temp"), tempFile);

    /// <summary>
    /// Normalizes temp directory for all examples
    /// </summary>
    /// <param name="fileName">Name of file to create temp path</param>
    /// <returns>A full path to a local temp directory using provided fileName</returns>
    public static string MakeExamplePath(string fileName) =>
        Path.Join(MakeTempPath("examples"), fileName);
}

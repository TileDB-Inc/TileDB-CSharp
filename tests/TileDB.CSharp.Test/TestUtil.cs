using System;
using System.IO;

namespace TileDB.CSharp.Test
{
    public static class TestUtil
    {
        static TestUtil()
        {
            if (!Directory.Exists(MakeTestPath("")))
            {
                Directory.CreateDirectory(MakeTestPath(""));
            }
        }
        /// <summary>
        /// Guard running new test cases against previous versions
        /// Returns false if TileDB core version predates `major.minor.rev`
        /// </summary>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <param name="rev"></param>
        /// <returns></returns>
        public static bool VersionMinimum(int major, int minor=0, int rev=0)
        {
            var version = TileDB.CSharp.CoreUtil.GetCoreLibVersion();

            if (version.Major < major)
            {
                return false;
            }
            else if (version.Major == major && version.Minor < minor)
            {
                return false;
            }
            else if (version.Major == major && version.Minor == minor && version.Build < rev)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Normalizes temp directory
        /// </summary>
        /// <param name="tempFile">Name of file to create temp path</param>
        /// <returns>A full path to the local tempFile</returns>
        public static string MakeTempPath(string tempFile) =>
            Path.Join(Path.Join(Path.GetTempPath(), "tiledb-csharp-temp"), tempFile);

        /// <summary>
        /// Normalizes temp directory for all tests
        /// </summary>
        /// <param name="fileName">Name of file to create temp path</param>
        /// <returns>A full path to a local temp directory using provided fileName</returns>
        public static string MakeTestPath(string fileName) => Path.Join(MakeTempPath("test"), fileName);

        /// <summary>
        /// Create a new array with a given name and schema
        /// </summary>
        /// <param name="ctx">Current TileDB Context</param>
        /// <param name="name">Name of array to create</param>
        /// <param name="schema">Schema to use for new array</param>
        public static void CreateArray(Context ctx, string name, ArraySchema schema)
        {
            if (Directory.Exists(name))
            {
                Directory.Delete(name, true);
            }
            Array.Create(ctx, name, schema);
        }

        /// <summary>
        /// Print schema of a local TileDB array
        /// </summary>
        /// <param name="arrayUri">Full path to existing TileDB array</param>
        public static void PrintLocalSchema(string arrayUri)
        {
            var context = Context.GetDefault();
            using Array array = new Array(context, arrayUri);
            array.Open(QueryType.Read);
            PrintLocalSchema(array.Schema());
            array.Close();
        }

        /// <summary>
        /// Print schema of a local TileDB array
        /// </summary>
        /// <param name="schema"></param>
        public static void PrintLocalSchema(ArraySchema schema)
        {
            foreach (var pair in schema.Dimensions())
            {
                Console.WriteLine($"Dimension: {pair.Key}; Data type: {pair.Value.Type()}");
            }
            foreach (var pair in schema.Attributes())
            {
                Console.WriteLine($"Attribute: {pair.Key}; Data type: {pair.Value.Type()}; " +
                                  $"Nullable: {pair.Value.Nullable()};");
            }
        }
    }
}

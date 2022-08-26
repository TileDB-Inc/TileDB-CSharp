using System;
using System.IO;
using System.Linq;

namespace TileDB.CSharp.Test
{
    public class TestUtil
    {
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

        public static void CreateTestArray(ArraySchema schema, LayoutType layout, string arrayName)
        {
            var ctx = Context.GetDefault();

            if (Directory.Exists(arrayName))
            {
                Directory.Delete(arrayName, true);
            }

            Array.Create(ctx, arrayName, schema);

            // Write to array
            using var array = new Array(ctx, arrayName);
            array.Open(QueryType.TILEDB_WRITE);
            using var writeQuery = new Query(ctx, array, QueryType.TILEDB_WRITE);
            writeQuery.SetLayout(layout);
            int[]? attrData = null;
            int[]? rowData = null;
            int[]? colData = null;
            attrData = Enumerable.Range(1, 16).ToArray();
            switch (schema.ArrayType())
            {
                case ArrayType.TILEDB_DENSE:
                {
                    writeQuery.SetSubarray(new[] {1, 4, 1, 4});
                    break;
                }
                case ArrayType.TILEDB_SPARSE:
                {
                    rowData = new[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 };
                    colData = new[] { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 };
                    writeQuery.SetDataBuffer("rows", rowData);
                    writeQuery.SetDataBuffer("cols", colData);
                    break;
                }
            }
            writeQuery.SetDataBuffer("a1", attrData!);
            writeQuery.Submit();
            Console.WriteLine($"Write status: {writeQuery.Status()}");
            writeQuery.FinalizeQuery();
            array.Close();
        }

        public static void ReadTestArray(ArraySchema schema, LayoutType layout, string arrayName)
        {
            var ctx = Context.GetDefault();
            var arrayType = schema.ArrayType();
            var array = new Array(ctx, arrayName);
            array.Open(QueryType.TILEDB_READ);

            int[] rowRead = new int[16];
            int[] colRead = new int[16];
            int[] attrRead = new int[16];
            var readQuery = new Query(ctx, array, QueryType.TILEDB_READ);
            readQuery.SetLayout(arrayType == ArrayType.TILEDB_DENSE ?
                LayoutType.TILEDB_ROW_MAJOR : LayoutType.TILEDB_UNORDERED);
            // readQuery.SetLayout(LayoutType.TILEDB_UNORDERED);
            switch (arrayType)
            {
                case ArrayType.TILEDB_DENSE:
                {
                    // readQuery.SetSubarray(new[] { 1, 4, 1, 4 });
                    readQuery.AddRange("rows", 1, 4);
                    readQuery.AddRange("cols", 1, 4);
                    break;
                }
                case ArrayType.TILEDB_SPARSE:
                {
                    // readQuery.SetSubarray(new[] { 1, 4, 1, 4 });
                    readQuery.AddRange("rows", 1, 4);
                    readQuery.AddRange("cols", 1, 4);
                    break;
                }
            }
            readQuery.SetDataBuffer("rows", rowRead);
            readQuery.SetDataBuffer("cols", colRead);
            readQuery.SetDataBuffer("a1", attrRead);
            readQuery.Submit();
            Console.WriteLine($"Read status: {readQuery.Status()}");
            Console.WriteLine(string.Join(", ", attrRead));
            array.Close();
        }

        /// <summary>
        /// Normalizes temp directory for all tests
        /// </summary>
        /// <param name="arrayName">Name of array to create temp path</param>
        /// <returns>A full path to a local temp directory using provided arrayName</returns>
        public static string GetTempPath(string arrayName) => Path.Join(Path.Join(Path.GetTempPath(), "tiledb-test"), arrayName);

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
            else if (!Directory.Exists(GetTempPath("")))
            {
                // Create `/tmp/tiledb-tests` if it does not exist
                Directory.CreateDirectory(GetTempPath(""));
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
            array.Open(QueryType.TILEDB_WRITE);
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
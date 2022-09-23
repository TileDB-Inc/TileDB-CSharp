using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        // TODO: Depreciate and remove this function
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
            int[] attrData = Enumerable.Range(1, 16).ToArray();
            int[]? rowData = null;
            int[]? colData = null;
            switch (schema.ArrayType())
            {
                case ArrayType.TILEDB_DENSE:
                {
                    writeQuery.SetLayout(layout);
                    writeQuery.SetSubarray(new[] {1, 4, 1, 4});
                    break;
                }
                case ArrayType.TILEDB_SPARSE:
                {
                    // Only write to sparse array using unordered or global order
                    if (layout is LayoutType.TILEDB_UNORDERED or LayoutType.TILEDB_GLOBAL_ORDER)
                    {
                        writeQuery.SetLayout(layout);
                    }
                    else
                    {
                        writeQuery.SetLayout(LayoutType.TILEDB_UNORDERED);
                    }

                    rowData = new[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 };
                    colData = new[] { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 };
                    writeQuery.SetDataBuffer("rows", rowData);
                    writeQuery.SetDataBuffer("cols", colData);
                    break;
                }
            }
            writeQuery.SetDataBuffer("a1", attrData);
            writeQuery.Submit();
            Console.WriteLine($"Write status: {writeQuery.Status()}");
            if (layout == LayoutType.TILEDB_GLOBAL_ORDER)
            {
                writeQuery.FinalizeQuery();
            }
            array.Close();
        }

        public static Query WriteArray(Array array, LayoutType layout, Dictionary<string, dynamic> buffers,
            (ulong, ulong)? timestampRange = null, Context? ctx = null, Dictionary<string, ulong[]>? offsets = null)
        {
            if (timestampRange != null)
            {
                array.SetOpenTimestampStart(timestampRange.Value.Item1);
                array.SetOpenTimestampEnd(timestampRange.Value.Item2);
            }
            array.Open(QueryType.TILEDB_WRITE);

            // Use context if provided; Else get default context
            var context = ctx ?? Context.GetDefault();

            var writeQuery = new Query(context, array, QueryType.TILEDB_WRITE);
            // Sparse arrays can't write using row major; Can read using row-major, but not preferred for performance
            if (array.Schema().ArrayType() == ArrayType.TILEDB_SPARSE && layout == LayoutType.TILEDB_ROW_MAJOR)
            {
                writeQuery.SetLayout(LayoutType.TILEDB_UNORDERED);
            }
            else
            {
                writeQuery.SetLayout(layout);
            }
            foreach (var buffer in buffers)
            {
                if (buffer.Value is byte[])
                {
                    writeQuery.SetDataBuffer<byte>(buffer.Key, buffer.Value);
                    writeQuery.SetOffsetsBuffer(buffer.Key, offsets![buffer.Key]);
                }
                else
                {
                    writeQuery.SetDataBuffer(buffer.Key, buffer.Value);
                }
            }
            writeQuery.Submit();
            Console.WriteLine($"Write query status: {writeQuery.Status()}");
            if (layout == LayoutType.TILEDB_GLOBAL_ORDER)
            {
                writeQuery.FinalizeQuery();
            }

            array.Close();

            Assert.AreEqual(QueryStatus.TILEDB_COMPLETED, writeQuery.Status());
            // Return write query for context
            return writeQuery;
        }

        public static Query ReadArray(Array array, LayoutType layout,
            Dictionary<string, dynamic> buffers, (ulong, ulong)? timestampRange = null, Context? ctx = null)
        {
            if (timestampRange != null)
            {
                array.SetOpenTimestampStart(timestampRange.Value.Item1);
                array.SetOpenTimestampEnd(timestampRange.Value.Item2);
            }

            array.Open(QueryType.TILEDB_READ);

            // Use context if provided; Else get default context
            var context = ctx ?? Context.GetDefault();

            var readQuery = new Query(context, array, QueryType.TILEDB_READ);
            readQuery.SetLayout(layout);
            foreach (var buffer in buffers)
            {
                if (buffer.Key.Contains("Offset"))
                {
                    readQuery.SetOffsetsBuffer(buffer.Key.Split("Offset")[0], buffer.Value);
                }
                else
                {
                    readQuery.SetDataBuffer(buffer.Key, buffer.Value);
                }
            }

            readQuery.Submit();
            Console.WriteLine($"Read query status: {readQuery.Status()}");

            array.Close();

            Assert.AreEqual(QueryStatus.TILEDB_COMPLETED, readQuery.Status());
            return readQuery;
        }

        public static void PrintBuffer<T>(IEnumerable<T> buffer) => Console.WriteLine(string.Join(", ", buffer));

        public static void CompareBuffers(Dictionary<string, dynamic> expected, Dictionary<string, dynamic> actual, bool dups)
        {
            foreach (var buffer in expected)
            {
                Console.WriteLine($"Expected {buffer.Key}: {string.Join(", ", buffer.Value)}" +
                                  $"\n  Actual {buffer.Key}: {string.Join(", ", actual[buffer.Key])}");
                if (dups)
                {
                    CollectionAssert.AreEquivalent(buffer.Value, actual[buffer.Key]);
                }
                else
                {
                    CollectionAssert.AreEqual(buffer.Value, actual[buffer.Key]);
                }
            }
            Console.WriteLine();
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
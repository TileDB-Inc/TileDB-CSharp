using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

namespace TileDB.CSharp.Test;

public static class TestUtil
{
    static TestUtil()
    {
        if (!Directory.Exists(MakeTestPath("")))
        {
            Directory.CreateDirectory(MakeTestPath(""));
        }
    }

    public static long GetDirectorySize(string directory)
    {
        var enumeration = new FileSystemEnumerable<long>(
            directory,
            (ref FileSystemEntry entry) => entry.Length,
            new() { RecurseSubdirectories = true }
        );

        return enumeration.Sum();
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
    /// Create a new array with a given name and schema to use for testing.
    /// If the array already exists, it will be removed and recreated.
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
    /// Writes to a TileDB array. Used for tests where we may write to several arrays many times.
    /// </summary>
    /// <param name="array">Initialized TileDB Array object to write to</param>
    /// <param name="layout">Layout to use when writing to the array</param>
    /// <param name="buffers">Dictionary using buffer name for key mapping to buffer values for writing</param>
    /// <param name="timestampRange">Optionally provide timestamp range to use for the write</param>
    /// <param name="ctx">Optionally provide a context from the caller to apply to the write</param>
    /// <param name="offsets">Dictionary using buffer name for key mapping to offsets to use for writing</param>
    /// <param name="keepOpen">If set to false, the query will be disposed at the end of the method and null will be returned.</param>
    /// <returns>Query object used to perform the write for caller context</returns>
    public static Query WriteArray(
        Array array,
        LayoutType layout,
        Dictionary<string, System.Array> buffers,
        (ulong, ulong)? timestampRange = null,
        Context? ctx = null,
        Dictionary<string, ulong[]>? offsets = null,
        bool keepOpen = true)
    {
        if (timestampRange is (ulong start, ulong end))
        {
            array.SetOpenTimestampStart(start);
            array.SetOpenTimestampEnd(end);
        }
        array.Open(QueryType.Write);

        // Use context if provided; Else get default context
        ctx ??= Context.GetDefault();

        var writeQuery = new Query(array, QueryType.Write);
        // Sparse arrays can't write using row major; Can read using row-major, but not preferred for performance
        if (array.Schema().ArrayType() == ArrayType.Sparse && layout == LayoutType.RowMajor)
        {
            writeQuery.SetLayout(LayoutType.Unordered);
        }
        else
        {
            writeQuery.SetLayout(layout);
        }

        foreach (var buffer in buffers)
        {
            // TODO: Remove cast to dynamic
            writeQuery.SetDataBuffer(buffer.Key, (dynamic)buffer.Value);
            // Set offset buffer if the attribute or dimension is variable size
            if (array.Schema().IsVarSize(buffer.Key))
            {
                writeQuery.SetOffsetsBuffer(buffer.Key, offsets![buffer.Key]);
            }
        }

        writeQuery.Submit();

        // Global order writes must finalize the query.
        if (layout == LayoutType.GlobalOrder)
        {
            writeQuery.FinalizeQuery();
        }

        Assert.AreEqual(QueryStatus.Completed, writeQuery.Status());
        array.Close();

        if (!keepOpen)
        {
            writeQuery.Dispose();
            return null!;
        }

        // Return write query for context
        return writeQuery;
    }

    /// <summary>
    /// Reads a TileDB array into buffers object. Used for tests where we may read from several arrays many times.
    /// When the read completes the buffers Dictionary will contain values storing read buffer data.
    /// </summary>
    /// <param name="array">Initialized TileDB Array object to read from</param>
    /// <param name="layout">Layout to use when reading from the array</param>
    /// <param name="buffers">Dictionary using buffer name for key mapping to container for writing to</param>
    /// <param name="timestampRange">Optionally provide timestamp range to use for the read</param>
    /// <param name="ctx">Optionally provide a context from the caller to apply to the read</param>
    /// <param name="offsets">Dictionary using buffer name for key mapping to offsets to use for writing</param>
    /// <param name="keepOpen">If set to false, the query will be disposed at the end of the method and null will be returned.</param>
    /// <returns>Query object used to perform the read for caller context</returns>
    public static Query ReadArray(
        Array array,
        LayoutType layout,
        Dictionary<string, System.Array> buffers,
        (ulong, ulong)? timestampRange = null,
        Context? ctx = null,
        Dictionary<string, ulong[]>? offsets = null,
        bool keepOpen = true)
    {
        if (timestampRange is (ulong start, ulong end))
        {
            array.SetOpenTimestampStart(start);
            array.SetOpenTimestampEnd(end);
        }
        array.Open(QueryType.Read);

        // Use context if provided; Else get default context
        ctx ??= Context.GetDefault();

        var readQuery = new Query(array, QueryType.Read);
        readQuery.SetLayout(layout);
        foreach (var buffer in buffers)
        {
            // TODO: Remove cast to dynamic
            readQuery.SetDataBuffer(buffer.Key, (dynamic)buffer.Value);
            // Set offset buffer if the attribute or dimension is variable size
            if (array.Schema().IsVarSize(buffer.Key))
            {
                readQuery.SetOffsetsBuffer(buffer.Key, offsets![buffer.Key]);
            }
        }

        readQuery.Submit();
        Assert.AreEqual(QueryStatus.Completed, readQuery.Status());
        array.Close();

        if (!keepOpen)
        {
            readQuery.Dispose();
            return null!;
        }

        return readQuery;
    }

    /// <summary>
    /// Checks expected buffer values are present in actual buffers.
    /// </summary>
    /// <param name="expected">Dictionary using buffer name for key mapping to expected contents</param>
    /// <param name="actual">Dictionary using buffer name for key mapping to actual contents</param>
    /// <param name="duplicates">True if duplicates are enabled</param>
    public static void CompareBuffers(
        Dictionary<string, System.Array> expected,
        Dictionary<string, System.Array> actual,
        bool duplicates)
    {
        foreach (var buffer in expected)
        {
            if (duplicates)
            {
                CollectionAssert.AreEquivalent(buffer.Value, actual[buffer.Key]);
            }
            else
            {
                CollectionAssert.AreEqual(buffer.Value, actual[buffer.Key]);
            }
        }
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

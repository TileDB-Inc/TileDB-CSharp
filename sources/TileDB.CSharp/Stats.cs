using System;
using TileDB.Interop;

namespace TileDB.CSharp;

/// <summary>
/// Contains methods related to collecting statistics of TileDB Embedded.
/// </summary>
public static unsafe class Stats
{
    /// <summary>
    /// Enables statistic gathering
    /// </summary>
    public static void Enable() => ErrorHandling.ThrowOnError(Methods.tiledb_stats_enable());

    /// <summary>
    /// Disables statistic gathering
    /// </summary>
    public static void Disable() => ErrorHandling.ThrowOnError(Methods.tiledb_stats_disable());

    /// <summary>
    /// Resets all previously gathered statistics.
    /// </summary>
    public static void Reset() => ErrorHandling.ThrowOnError(Methods.tiledb_stats_reset());

    /// <summary>
    /// Writes statistics to standard output.
    /// </summary>
    public static void Dump() => Console.WriteLine(Get());

    /// <summary>
    /// Writes TileDB statistics to a file.
    /// </summary>
    /// <param name="filePath">Path to file to dump statistics</param>
    public static void Dump(string filePath) => System.IO.File.WriteAllText(filePath, Get());

    /// <summary>
    /// Gets a JSON string with statistics about TileDB Embedded.
    /// </summary>
    public static string Get()
    {
        sbyte* result = null;
        try
        {
            ErrorHandling.ThrowOnError(Methods.tiledb_stats_dump_str(&result));
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }
        finally
        {
            if (result is not null)
            {
                ErrorHandling.ThrowOnError(Methods.tiledb_stats_free_str(&result));
            }
        }
    }

    /// <summary>
    /// Gets a JSON string with raw statistics about TileDB Embedded.
    /// </summary>
    public static string GetRaw()
    {
        sbyte* result = null;
        try
        {
            ErrorHandling.ThrowOnError(Methods.tiledb_stats_raw_dump_str(&result));
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }
        finally
        {
            if (result is not null)
            {
                ErrorHandling.ThrowOnError(Methods.tiledb_stats_free_str(&result));
            }
        }
    }

    /// <summary>
    /// Enables heap profiling of TileDB Embedded.
    /// </summary>
    /// <param name="fileNamePrefix">The file name prefix of the dumps. If it is
    /// <see langword="null"/> or empty, dumps will be written to stdout.</param>
    /// <param name="dumpIntervalMilliseconds">If non-zero, this spawns a dedicated
    /// thread to dump on this time interval.</param>
    /// <param name="dumpIntervalBytes">If non-zero, a dump will occur when the total
    /// number of lifetime allocated bytes is increased by more than this amount.</param>
    /// <param name="dumpThresholdBytes">If non-zero, labeled allocations with a number
    /// of bytes lower than this threshold will not be reported in the dump.</param>
    public static void EnableHeapProfiler(string? fileNamePrefix, ulong dumpIntervalMilliseconds, ulong dumpIntervalBytes, ulong dumpThresholdBytes)
    {
        using var ms_fileNamePrefix = new MarshaledString(fileNamePrefix ?? "");
        ErrorHandling.ThrowOnError(Methods.tiledb_heap_profiler_enable(ms_fileNamePrefix, dumpIntervalMilliseconds, dumpIntervalBytes, dumpThresholdBytes));
    }
}

using System;
using TileDB.Interop;

namespace TileDB.CSharp
{
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
    }
}

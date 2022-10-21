using System;
using System.Runtime.InteropServices;
using TileDB.Interop;

namespace TileDB.CSharp
{
    public static unsafe class Stats
    {
        private static readonly Context _ctx = Context.GetDefault();

        /// <summary>
        /// Enable statistic gathering
        /// </summary>
        public static void Enable() => _ctx.handle_error(Methods.tiledb_stats_enable());

        /// <summary>
        /// Disable statistic gathering
        /// </summary>
        public static void Disable() => _ctx.handle_error(Methods.tiledb_stats_disable());

        /// <summary>
        /// Reset all statistics previously gathered
        /// </summary>
        public static void Reset() => _ctx.handle_error(Methods.tiledb_stats_reset());

        /// <summary>
        /// Dump TileDB statistics to stdout
        /// </summary>
        public static void Dump() => Console.WriteLine(Get());

        /// <summary>
        /// Dump TileDB statistics to a file
        /// </summary>
        /// <param name="filePath">Path to file to dump statistics</param>
        public static void Dump(string filePath) => System.IO.File.WriteAllText(filePath, Get());

        /// <summary>
        /// Get TileDB statistics as string
        /// </summary>
        /// <returns>string object containing dumped TileDB statistics</returns>
        public static string Get()
        {
            var ms = new MarshaledStringOut();
            _ctx.handle_error(Methods.tiledb_stats_dump_str(&ms.Value));
            string stats = ms.ToString();
            _ctx.handle_error(Methods.tiledb_stats_free_str(&ms.Value));
            return stats;
        }
    }
}

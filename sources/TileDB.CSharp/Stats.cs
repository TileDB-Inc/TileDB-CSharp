using System;
using System.Runtime.InteropServices;
using TileDB.Interop;

namespace TileDB.CSharp
{
    public static unsafe class Stats
    {
        private static readonly Context _ctx;
        static Stats()
        {
            _ctx = Context.GetDefault();
        }

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
        public static void Dump()
        {
            var ms = (sbyte**)Marshal.StringToHGlobalAnsi("");
            _ctx.handle_error(Methods.tiledb_stats_dump_str(ms));
            Console.WriteLine(new string(ms[0]));
            Marshal.FreeHGlobal((IntPtr)ms);
        }

        /// <summary>
        /// Dump TileDB statistics to a file
        /// </summary>
        /// <param name="filePath"></param>
        public static void Dump(string filePath)
        {
            var ms = (sbyte**)Marshal.StringToHGlobalAnsi("");
            _ctx.handle_error(Methods.tiledb_stats_dump_str(ms));
            System.IO.File.WriteAllText(filePath, new string(ms[0]));
            Marshal.FreeHGlobal((IntPtr)ms);
        }
    }
}

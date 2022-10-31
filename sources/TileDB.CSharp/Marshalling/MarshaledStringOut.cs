using System;
using System.Text;

namespace TileDB.Interop
{
    internal static class MarshaledStringOut
    {
        /// <summary>
        /// Encodes a read-only span of bytes into a string, using the default encoding.
        /// </summary>
        public static string GetString(ReadOnlySpan<byte> span) =>
            Encoding.ASCII.GetString(span);

        /// <summary>
        /// Encodes a null-terminated pointer of bytes into a string, using the default encoding.
        /// </summary>
        public static unsafe string GetStringFromNullTerminated(sbyte* ptr)
        {
            if (ptr == null)
            {
                return string.Empty;
            }

            var span = new ReadOnlySpan<byte>(ptr, int.MaxValue);
            span = span[0..span.IndexOf((byte)0)];
            return GetString(span);
        }
    }
}

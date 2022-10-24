using System;
using System.Text;

namespace TileDB.Interop
{
    internal unsafe struct MarshaledStringOut
    {
        public sbyte* Value;

        /// <summary>
        /// Encodes a read-only span of bytes into a string, using the default encoding.
        /// </summary>
        public static string GetString(ReadOnlySpan<byte> span) =>
            Encoding.ASCII.GetString(span);

        public override string ToString()
        {
            if (Value == null) {
                return string.Empty;
            }

            var span = new ReadOnlySpan<byte>(Value, int.MaxValue);
            span = span.Slice(0, span.IndexOf((byte)'\0'));
            return GetString(span);
        }
    }
}

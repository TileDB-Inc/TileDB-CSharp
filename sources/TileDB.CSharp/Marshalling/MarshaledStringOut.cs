using System;
using System.Text;

namespace TileDB.Interop
{
    internal unsafe struct MarshaledStringOut
    {
        public sbyte* Value;

        public override string ToString()
        {
            if (Value == null) {
                return string.Empty;
            }

            var span = new ReadOnlySpan<byte>(Value, int.MaxValue);
            span = span.Slice(0, span.IndexOf((byte)'\0'));
            return Encoding.ASCII.GetString(span);
        }
    }
}

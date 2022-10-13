using System;
using System.Text;

namespace TileDB.Interop
{
    internal unsafe class MarshaledStringOut
    {
        public sbyte* Value;

        public MarshaledStringOut()
        {
            Value = null;
        }

        public static implicit operator string(MarshaledStringOut s)
        {
            if (s.Value == null) {
                return string.Empty;
            }

            var span = new ReadOnlySpan<byte>(s.Value, int.MaxValue);
            span = span.Slice(0, span.IndexOf((byte)'\0'));
            return Encoding.ASCII.GetString(span);
        }
    }
}

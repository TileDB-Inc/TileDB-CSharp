#if !NET6_0_OR_GREATER
namespace System.Runtime.InteropServices
{
    internal static unsafe class MemoryMarshalCompat
    {
        public static ReadOnlySpan<byte> CreateReadOnlySpanFromNullTerminated(byte* value)
        {
            if (value == null)
            {
                return ReadOnlySpan<byte>.Empty;
            }

            ReadOnlySpan<byte> result = new(value, int.MaxValue);
            return result[0..result.IndexOf((byte)0)];
        }
    }
}
#else
global using MemoryMarshalCompat = System.Runtime.InteropServices.MemoryMarshal;
#endif

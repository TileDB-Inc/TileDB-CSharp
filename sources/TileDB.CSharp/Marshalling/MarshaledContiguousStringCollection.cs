using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling;

internal unsafe ref struct MarshaledContiguousStringCollection
{
    public byte* Data { get; private set; }

    public ulong* Offsets { get; private set; }

    public ulong DataCount { get; private set; }

    public ulong OffsetsCount { get; private set; }

    public MarshaledContiguousStringCollection(IReadOnlyCollection<string> strings, DataType dataType = DataType.StringAscii)
    {
        Encoding encoding = MarshaledString.GetEncoding(dataType);
        try
        {
            Offsets = (ulong*)Marshal.AllocHGlobal(strings.Count * sizeof(ulong));
            OffsetsCount = (ulong)strings.Count * sizeof(ulong);
            ulong currentOffset = 0;
            DataCount = 0;
            foreach (var str in strings)
            {
                Offsets[currentOffset++] = DataCount;
                DataCount += (ulong)encoding.GetByteCount(str);
            }

            Data = (byte*)Marshal.AllocHGlobal((IntPtr)DataCount);
            int i = 0;
            foreach (var str in strings)
            {
                currentOffset = Offsets[i];
                encoding.GetBytes(str, new Span<byte>(Data + currentOffset, checked((int)(DataCount - currentOffset))));
                i++;
            }
        }
        catch
        {
            Dispose();
            throw;
        }
    }

    public void Dispose()
    {
        if (Data is not null)
        {
            Marshal.FreeHGlobal((IntPtr)Data);
            Data = null;
            DataCount = 0;
        }
        if (Offsets is not null)
        {
            Marshal.FreeHGlobal((IntPtr)Offsets);
            Offsets = null;
            OffsetsCount = 0;
        }
    }
}

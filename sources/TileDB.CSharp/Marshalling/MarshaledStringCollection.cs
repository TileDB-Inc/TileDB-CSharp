using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling;

internal unsafe ref struct MarshaledStringCollection
{
    public sbyte** Strings { get; private set; }

    public int Count { get; }

    public MarshaledStringCollection(IReadOnlyList<string> strings)
    {
        Count = strings.Count;
        Strings = (sbyte**)NativeMemory.Alloc((nuint)Count, (nuint)sizeof(sbyte*));

        try
        {
            for (int i = 0; i < Count; i++)
            {
                Strings[i] = MarshaledString.AllocNullTerminated(strings[i], out _);
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
        if (Strings is null) return;

        for (int i = 0; i < Count; i++)
        {
            MarshaledString.FreeNullTerminated(Strings[i]);
        }
        NativeMemory.Free(Strings);
        Strings = null;
    }
}

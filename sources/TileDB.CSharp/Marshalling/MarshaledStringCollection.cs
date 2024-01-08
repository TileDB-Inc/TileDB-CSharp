using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling;

internal unsafe ref struct MarshaledStringCollection
{
    public IntPtr* Strings { get; private set; }

    public int Count { get; }

    public MarshaledStringCollection(IReadOnlyList<string> strings)
    {
        Count = strings.Count;
        Strings = (IntPtr*)Marshal.AllocHGlobal(Count * sizeof(IntPtr));

        try
        {
            for (int i = 0; i < Count; i++)
            {
                Strings[i] = MarshaledString.AllocNullTerminated(strings[i]).Pointer;
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
        Marshal.FreeHGlobal((IntPtr)Strings);
        Strings = null;
    }
}

using System;
using System.Diagnostics;
using System.Text;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling;

internal unsafe struct StringHandleHolder : IDisposable
{
    internal tiledb_string_t* _handle;

    public void Dispose()
    {
        if (_handle == null)
        {
            return;
        }

        fixed (tiledb_string_t** handlePtr = &_handle)
        {
            int result = Methods.tiledb_string_free(handlePtr);
            Debug.Assert(result == (int)Status.TILEDB_OK);
        }
    }

    public override string ToString()
    {
        if (_handle == null)
        {
            return string.Empty;
        }

        byte* data;
        nuint length;
        ErrorHandling.ThrowOnError(Methods.tiledb_string_view(_handle, (sbyte**)&data, &length));

        return Encoding.UTF8.GetString(data, checked((int)length));
    }
}

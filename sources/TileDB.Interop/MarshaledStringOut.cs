using System;
using System.Runtime.InteropServices;
using System.Text;

namespace TileDB.Interop
{
    public unsafe partial class LibC
    {
        public unsafe struct handle_t {};
        [DllImport(LibDllImport.LibCPath)]
        public unsafe static extern void free(void* p);

    }

    public unsafe class MarshaledStringOut
    {
        public sbyte* Value;
        public MarshaledStringOut()
        {
            Value = null;
        }

        // We currently cannot free libtiledb-owned string returned through
        // char** out-pointer.
        //public void Dispose()
        //{
        //    Dispose(true);
        //    System.GC.SuppressFinalize(this);
        //}
        //protected virtual void Dispose(bool disposing)
        //{
        //    if (Value != null)
        //    {
        //        LibC.free(Value);
        //    }
        //    Value = null;
        //}

        //~MarshaledStringOut()
        //{
        //    Dispose(false);
        //}

        public static implicit operator string(MarshaledStringOut s)
        {
            if (s.Value == null) {
                return string.Empty;
            }
            else
            {
                var span = new ReadOnlySpan<byte>(s.Value, Int32.MaxValue);
                return span.Slice(0, span.IndexOf((byte)'\0')).AsString();
            }
        }



    }

}//namespace
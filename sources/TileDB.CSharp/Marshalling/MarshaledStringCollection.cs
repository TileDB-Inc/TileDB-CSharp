using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling
{
    internal unsafe ref struct MarshaledStringCollection
    {
        private ScratchBuffer<IntPtr> _nativeStrings;

        public ReadOnlySpan<IntPtr> Strings => _nativeStrings.Span;

        public MarshaledStringCollection(IReadOnlyCollection<string> strings, Span<IntPtr> preAllocatedSpan)
        {
            _nativeStrings = new ScratchBuffer<IntPtr>(strings.Count, preAllocatedSpan);
            Span<IntPtr> buffer = _nativeStrings.Span;
            buffer.Clear();

            int i = 0;
            try
            {
                foreach (string s in strings)
                {
                    buffer[i++] = MarshaledString.AllocNullTerminated(s).Pointer;
                }
                Debug.Assert(i == buffer.Length);
            }
            catch
            {
                Dispose();
                throw;
            }
        }

        public void Dispose()
        {
            foreach (ref IntPtr ptr in _nativeStrings.Span)
            {
                if (ptr != (IntPtr)0)
                {
                    Marshal.FreeHGlobal(ptr);
                    ptr = (IntPtr)0;
                }
            }
            _nativeStrings.Dispose();
        }
    }
}

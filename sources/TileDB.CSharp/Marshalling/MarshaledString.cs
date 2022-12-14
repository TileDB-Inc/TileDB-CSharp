// Copyright (c) .NET Foundation and Contributors. All Rights Reserved. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace TileDB.Interop
{
    internal unsafe struct MarshaledString : IDisposable
    {
        public MarshaledString(string input)
        {
            (IntPtr ptr, Length) = AllocNullTerminated(input);
            Value = (sbyte*)ptr;
        }

        public static (IntPtr Pointer, int Length) AllocNullTerminated(string str)
        {
            if (str is null)
            {
                return ((IntPtr)0, 0);
            }

            int length = Encoding.ASCII.GetByteCount(str);
            var ptr = (sbyte*)Marshal.AllocHGlobal(length + 1);
            int bytesWritten = Encoding.ASCII.GetBytes(str, new Span<byte>(ptr, length));
            Debug.Assert(bytesWritten == length);
            ptr[length] = 0;
            return ((IntPtr)ptr, length);
        }

        public static void FreeNullTerminated(IntPtr ptr) => Marshal.FreeHGlobal(ptr);

        public int Length { get; private set; }

        public sbyte* Value { get; private set; }

        public void Dispose()
        {
            if (Value != null)
            {
                Marshal.FreeHGlobal((IntPtr)Value);
                Value = null;
                Length = 0;
            }
        }

        public static implicit operator sbyte*(in MarshaledString value) => value.Value;

        public override string ToString()
        {
            var span = new ReadOnlySpan<byte>(Value, Length);
            return MarshaledStringOut.GetString(span);
        }
    }
}

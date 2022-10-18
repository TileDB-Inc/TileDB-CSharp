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
            int length;
            sbyte* value;

            if (input is null)
            {
                length = 0;
                value = null;
            }
            else
            {
                length = Encoding.ASCII.GetByteCount(input);
                value = (sbyte*)Marshal.AllocHGlobal(length + 1);
                int bytesWritten = Encoding.ASCII.GetBytes(input, new Span<byte>(value, length));
                Debug.Assert(bytesWritten == length);
                value[length] = 0;
            }

            Length = length;
            Value = value;
        }

        public ReadOnlySpan<byte> AsSpan() => new ReadOnlySpan<byte>(Value, Length);

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
            return Encoding.ASCII.GetString(span);
        }
    }
}

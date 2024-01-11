// Copyright (c) .NET Foundation and Contributors. All Rights Reserved. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using TileDB.CSharp;

namespace TileDB.Interop;

internal unsafe struct MarshaledString : IDisposable
{
    public MarshaledString(string input)
    {
        (IntPtr ptr, Length) = AllocNullTerminated(input);
        Value = (sbyte*)ptr;
    }

    internal static Encoding GetEncoding(DataType dataType)
    {
        switch (dataType)
        {
            case DataType.StringAscii:
                return Encoding.ASCII;
            case DataType.StringUtf8:
                return Encoding.UTF8;
            case DataType.StringUtf16:
                return Encoding.Unicode;
            case DataType.StringUtf32:
                return Encoding.UTF32;
        }
        ThrowHelpers.ThrowInvalidDataType(dataType);
        return null;
    }

    internal static (IntPtr Pointer, int Length) AllocNullTerminated(string str)
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

    internal static void FreeNullTerminated(IntPtr ptr) => Marshal.FreeHGlobal(ptr);

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
        return MarshaledStringOut.GetString(span);
    }
}

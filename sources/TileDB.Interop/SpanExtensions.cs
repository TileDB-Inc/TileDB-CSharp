// Copyright (c) .NET Foundation and Contributors. All Rights Reserved. Licensed under the MIT License (MIT). See License.md in the repository root for more information.
//https://github.com/dotnet/ClangSharp/blob/67c1e5243b9d58f2b28f10e3f9a82f7537fd9d88/sources/ClangSharp.Interop/Internals/SpanExtensions.cs

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace TileDB.Interop 
{
    public static unsafe class SpanExtensions 
    {
        public static string AsString(this Span<byte> self) => AsString((ReadOnlySpan<byte>)self);

        public static string AsString(this ReadOnlySpan<byte> self)
        {
            if (self.IsEmpty)
            {
                return string.Empty;
            }

            fixed (byte* pSelf = self)
            {
                return Encoding.ASCII.GetString(pSelf, self.Length);
            }
        }
    }

}//namespace
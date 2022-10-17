using System;
using System.Diagnostics.CodeAnalysis;

namespace TileDB.CSharp
{
    internal static class ThrowHelpers
    {
        [DoesNotReturn]
        public static void ThrowTypeNotSupported() =>
            throw new NotSupportedException("Type not supported");
    }
}

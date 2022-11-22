using System;
using System.Diagnostics.CodeAnalysis;

namespace TileDB.CSharp
{
    internal static class ThrowHelpers
    {
        [DoesNotReturn]
        public static void ThrowTypeNotSupported() =>
            throw new NotSupportedException("Type is not supported");

        // We don't have to specify the type in the type argument, it can be seen from the stacktrace.
        [DoesNotReturn]
        public static void ThrowTypeMismatch(DataType type) =>
            throw new InvalidOperationException($"Type is not compatible with data type {type}");

        [DoesNotReturn]
        public static void ThrowStringTypeMismatch(DataType type) =>
            throw new InvalidOperationException($"Cannot encode data type {type} into strings");
    }
}

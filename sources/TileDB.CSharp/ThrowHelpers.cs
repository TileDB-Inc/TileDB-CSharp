using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace TileDB.CSharp;

internal static class ThrowHelpers
{
    [DoesNotReturn]
    public static void ThrowArgumentNull(string? paramName) =>
        throw new ArgumentNullException(paramName);

    [DoesNotReturn]
    public static void ThrowInvalidDataType(DataType dataType, [CallerArgumentExpression(nameof(dataType))] string? paramName = null) =>
        throw new ArgumentOutOfRangeException(nameof(dataType), dataType, "Invalid data type.");

    [DoesNotReturn]
    public static void ThrowTypeNotSupported() =>
        throw new NotSupportedException("Type is not supported.");

    // We don't have to specify the type in the type argument, it can be seen from the stacktrace.
    [DoesNotReturn]
    public static void ThrowTypeMismatch(DataType type) =>
        throw new ArgumentException($"Type is not compatible with data type {type}.");

    [DoesNotReturn]
    public static void ThrowStringTypeMismatch(DataType type) =>
        throw new ArgumentException($"Cannot encode data type {type} into strings.");

    [DoesNotReturn]
    public static void ThrowTooBigSize(ulong size, [CallerArgumentExpression(nameof(size))] string? paramName = null) =>
        throw new ArgumentOutOfRangeException(paramName, size, "Size argument is too big for the type to fit.");

    [DoesNotReturn]
    public static void ThrowQueryConditionDifferentContexts() =>
        throw new InvalidOperationException("Cannot combine query conditions associated with different contexts.");

    [DoesNotReturn]
    public static void ThrowSubarrayLengthMismatch(string paramName) =>
        throw new ArgumentException("The length of the data is not equal to double the length of dimensions.", paramName);

    [DoesNotReturn]
    public static void ThrowManagedType() =>
        throw new NotSupportedException("Types with managed references are not supported.");

    [DoesNotReturn]
    public static void ThrowOperationNotAllowedOnReadQueries() =>
        throw new NotSupportedException("The operation is not allowed on read queries.");

    [DoesNotReturn]
    public static void ThrowBufferCannotBeEmpty(string paramName) =>
        throw new ArgumentException("Buffer cannot be empty.", paramName);

    [DoesNotReturn]
    public static void ThrowArgumentNullException(string paramName) =>
        throw new ArgumentNullException(paramName);

    [DoesNotReturn]
    public static void ThrowBufferUnsafelySet() =>
        throw new InvalidOperationException("Cannot get the number of elements read into a buffer set with the 'Query.UnsafeSetDataBuffer' method.");
}

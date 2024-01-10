using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using TileDB.Interop;

namespace TileDB.CSharp;

internal static class ErrorHandling
{
    public static void ThrowIfNull([NotNull] object? obj, [CallerArgumentExpression(nameof(obj))] string? paramName = null)
    {
        if (obj == null)
        {
            ThrowHelpers.ThrowArgumentNull(paramName);
        }
    }

    public static void ThrowOnError(int errorCode)
    {
        if (errorCode == (int)Status.TILEDB_OK)
        {
            return;
        }
        throw new TileDBException($"Operation failed with error code {errorCode}.") { StatusCode = errorCode };
    }

    public static unsafe void CheckLastError(tiledb_error_t** error, int status)
    {
        if (status == (int)Status.TILEDB_OK)
        {
            Debug.Assert(*error is null);
            return;
        }

        ThrowLastError(error, status);

        [DoesNotReturn]
        static void ThrowLastError(tiledb_error_t** error, int status)
        {
            sbyte* messagePtr;
            int messageResult = Methods.tiledb_error_message(*error, &messagePtr);
            Debug.Assert(messageResult == (int)Status.TILEDB_OK);
            string message = MarshaledStringOut.GetStringFromNullTerminated(messagePtr);
            Methods.tiledb_error_free(error);
            throw new TileDBException(message) { StatusCode = status };
        }
    }

    /// <exception cref="NotSupportedException"><typeparamref name="T"/> is managed.</exception>
    public static void ThrowIfManagedType<T>()
    {
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            ThrowHelpers.ThrowManagedType();
        }
    }

    /// <summary>
    /// Returns whether values of type <typeparamref name="T"/> can be stored or
    /// retrieved from a TileDB buffer of type <paramref name="dataType"/>.
    /// </summary>
    private static unsafe bool AreTypesCompatible<T>(DataType dataType)
    {
        if (EnumUtil.DataTypeToNumericType(dataType) == typeof(T))
        {
            return true;
        }
        if (typeof(T) == typeof(bool) && dataType == DataType.Boolean)
        {
            return true;
        }
        if (typeof(T) == typeof(char) && dataType == DataType.StringUtf16)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Throws if values of type <typeparamref name="T"/> cannot be stored or
    /// retrieved from a TileDB buffer of type <paramref name="dataType"/>.
    /// </summary>
    public static void CheckDataType<T>(DataType dataType)
    {
        ThrowIfManagedType<T>();
        if (!AreTypesCompatible<T>(dataType))
        {
            ThrowHelpers.ThrowTypeMismatch(dataType);
        }
    }
}

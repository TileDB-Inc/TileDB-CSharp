using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using TileDB.Interop;

namespace TileDB.CSharp
{
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
    }
}

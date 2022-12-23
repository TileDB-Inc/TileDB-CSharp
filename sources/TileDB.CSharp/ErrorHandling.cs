using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using TileDB.Interop;

namespace TileDB.CSharp
{
    internal static class ErrorHandling
    {
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
                Methods.tiledb_error_free(error);
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
    }
}

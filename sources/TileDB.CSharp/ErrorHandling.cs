using System;

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
            throw new Exception($"Operation failed with error code {errorCode}.");
        }
    }
}

#if !NET6_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System
{
    internal static class ArgumentExceptionCompat
    {
        public static void ThrowIfNullOrEmpty([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (string.IsNullOrEmpty(argument))
            {
                ThrowNullOrEmptyException(argument, paramName);
            }
        }

        [DoesNotReturn]
        [SuppressMessage("Minor Code Smell", "S3236:Caller information arguments should not be provided explicitly", Justification = "Method is a throw helper")]
        private static void ThrowNullOrEmptyException(string? argument, string? paramName)
        {
            ArgumentNullExceptionCompat.ThrowIfNull(argument, paramName);
            throw new ArgumentException("String is empty", paramName);
        }
    }
}

#else
global using ArgumentExceptionCompat = System.ArgumentException;
#endif

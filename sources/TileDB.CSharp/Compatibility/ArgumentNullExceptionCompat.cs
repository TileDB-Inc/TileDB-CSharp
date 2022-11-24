#if !NET6_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System
{
    internal static class ArgumentNullExceptionCompat
    {
        public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (argument is null)
            {
                Throw(paramName);
            }
        }

        [DoesNotReturn]
        private static void Throw(string? paramName)
        {
            throw new ArgumentNullException(paramName);
        }
    }
}

#else
global using ArgumentNullExceptionCompat = System.ArgumentNullException;
#endif

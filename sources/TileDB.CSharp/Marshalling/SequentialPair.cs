using System;
using System.Runtime.InteropServices;

namespace TileDB.CSharp.Marshalling;

/// <summary>
/// A struct containing two values of the same type that can be passed to native code.
/// Has implicit conversion operators between <see cref="ValueTuple{T,T}"/>.
/// </summary>
/// <typeparam name="T">The type of the values</typeparam>
/// <remarks>
/// We can't use value tuples in interop because they are marked with auto layout.
/// </remarks>
[StructLayout(LayoutKind.Sequential)]
internal struct SequentialPair<T>
{
    public T First;
    public T Second;

    public static implicit operator (T First, T Second)(SequentialPair<T> x) => (x.First, x.Second);

    public static implicit operator SequentialPair<T>((T First, T Second) x) => new() { First = x.First, Second = x.Second };
}

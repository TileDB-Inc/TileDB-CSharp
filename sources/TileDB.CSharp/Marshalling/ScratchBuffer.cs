using System;
using System.Buffers;
using System.Runtime.InteropServices;

#nullable enable

namespace TileDB.CSharp.Marshalling;

/// <summary>
/// Manages temporary buffers with low GC pressure.
/// </summary>
/// <remarks>
/// Objects of this type should not be passed around; instead pass their <see cref="Span"/> property.
/// </remarks>
/// <typeparam name="T">The type of items this scratch buffer stores.</typeparam>
internal ref struct ScratchBuffer<T>
{
    public Span<T> Span { get; private set; }

    private T[]? _arrayToReturn;

    /// <summary>
    /// Creates a <see cref="ScratchBuffer{T}"/> of length at least <paramref name="minimumLength"/>.
    /// </summary>
    /// <param name="minimumLength">The buffer's minimum length.</param>
    /// <param name="preAllocatedSpan">A pre-allocated <see cref="Span{T}"/> that will
    /// be preferred if it has at least <paramref name="minimumLength"/> elements.</param>
    /// <param name="exactSize">Whether to make <see cref="Span"/> be
    /// exactly of length <paramref name="minimumLength"/>. Defaults to <see langword="false"/>.</param>
    /// <remarks>
    /// <paramref name="preAllocatedSpan"/> could be allocated from the stack using
    /// the <see langword="stackalloc"/> keyword. If it is too small, <see cref="Span"/>
    /// will come from the array pool.
    /// </remarks>
    /// <seealso cref="ScratchBuffer{T}(int, bool)"/>
    public ScratchBuffer(int minimumLength, Span<T> preAllocatedSpan, bool exactSize = false)
    {
        if (preAllocatedSpan.Length >= minimumLength)
        {
            Span = preAllocatedSpan;
            if (exactSize)
            {
                Span = Span[..minimumLength];
            }
            _arrayToReturn = null;
        }
        else
            this = new ScratchBuffer<T>(minimumLength);
    }

    /// <summary>
    /// Creates a <see cref="ScratchBuffer{T}"/> of length at least <paramref name="minimumLength"/>.
    /// </summary>
    /// <param name="minimumLength">The buffer's minimum length.</param>
    /// <param name="exactSize">Whether to make <see cref="Span"/> be
    /// exactly of length <paramref name="minimumLength"/>. Defaults to <see langword="false"/>.</param>
    /// <remarks>The created instance's <see cref="Span"/>
    /// will always come from the array pool.</remarks>
    public ScratchBuffer(int minimumLength, bool exactSize = false)
    {
        Span = _arrayToReturn = ArrayPool<T>.Shared.Rent(minimumLength);
        if (exactSize)
        {
            Span = Span[..minimumLength];
        }
    }

    /// <summary>
    /// Allows directly using this <see cref="ScratchBuffer{T}"/> in a <see langword="fixed"/> statement.
    /// </summary>
    public ref T GetPinnableReference() => ref MemoryMarshal.GetReference(Span);

    /// <summary>
    /// Disposes the <see cref="ScratchBuffer{T}"/>.
    /// </summary>
    /// <remarks>
    /// This function must be always called after using a <see cref="ScratchBuffer{T}"/>
    /// to release any pooled arrays that might have been used. You can use <see langword="using"/>
    /// keyword to do it more intuitively. After disposal, the spans returned by the <see cref="Span"/>
    /// property must not be used, and accessing it will return an empty span.</remarks>
    public void Dispose()
    {
        if (_arrayToReturn == null) return;

        Span = default;
        ArrayPool<T>.Shared.Return(_arrayToReturn);
        _arrayToReturn = null;
    }
}

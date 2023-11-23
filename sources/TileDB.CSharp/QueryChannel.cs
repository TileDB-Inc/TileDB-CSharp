using System;
using TileDB.CSharp.Marshalling.SafeHandles;

namespace TileDB.CSharp;

/// <summary>
/// Represents a TileDB query channel object.
/// </summary>
public unsafe sealed class QueryChannel : IDisposable
{
    private readonly QueryChannelHandle _handle;

    internal QueryChannel(QueryChannelHandle handle)
    {
        _handle = handle;
    }

    internal QueryChannelHandle Handle => _handle;

    /// <inheritDoc/>
    public void Dispose()
    {
        _handle.Dispose();
    }
}

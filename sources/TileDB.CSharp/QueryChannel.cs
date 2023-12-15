using System;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp;

/// <summary>
/// Represents a TileDB query channel object.
/// </summary>
public unsafe sealed class QueryChannel : IDisposable
{
    private readonly Context _ctx;

    private readonly Query _query;

    private readonly QueryChannelHandle _handle;

    internal QueryChannel(Context ctx, Query query, QueryChannelHandle handle)
    {
        _ctx = ctx;
        _query = query;
        _handle = handle;
    }

    internal QueryChannelHandle Handle => _handle;

    /// <inheritDoc/>
    public void Dispose()
    {
        _handle.Dispose();
    }

    /// <summary>
    /// Adds an aggregate field to the <see cref="QueryChannel"/>.
    /// </summary>
    /// <param name="operation">The operation to apply.</param>
    /// <param name="name">The name under which the result of the operation will be available in the query.</param>
    public void ApplyAggregate(AggregateOperation operation, string name)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var ms_name = new MarshaledString(name);
        using var operationHandle = operation.CreateHandle(_ctx, _query);
        using var operationHandlePtr = operationHandle.Acquire();
        _ctx.handle_error(Methods.tiledb_channel_apply_aggregate(ctxHandle, handle, ms_name, operationHandlePtr));
    }
}

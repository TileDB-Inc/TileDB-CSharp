using System;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp;

/// <summary>
/// Represents a TileDB query field object.
/// </summary>
public unsafe sealed class QueryField : IDisposable
{
    private readonly Context _ctx;

    private readonly Query _query;

    private readonly QueryFieldHandle _handle;

    internal QueryField(Context ctx, Query query, QueryFieldHandle handle, string name)
    {
        _ctx = ctx;
        _query = query;
        _handle = handle;
        Name = name;
    }

    /// <summary>
    /// Gets the number of values per cell of the <see cref="QueryField"/>.
    /// </summary>
    public uint ValuesPerCell
    {
        get
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            uint result;
            _ctx.handle_error(Methods.tiledb_field_cell_val_num(ctxHandle, handle, &result));
            return result;
        }
    }

    /// <summary>
    /// Gets the data type of the <see cref="QueryField"/>.
    /// </summary>
    public DataType DataType
    {
        get
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            tiledb_datatype_t result;
            _ctx.handle_error(Methods.tiledb_field_datatype(ctxHandle, handle, &result));
            return (DataType)result;
        }
    }

    /// <summary>
    /// Gets the origin of the <see cref="QueryField"/>.
    /// </summary>
    public QueryFieldOrigin Origin
    {
        get
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            tiledb_field_origin_t result;
            _ctx.handle_error(Methods.tiledb_field_origin(ctxHandle, handle, &result));
            return (QueryFieldOrigin)result;
        }
    }

    /// <summary>
    /// The name of the <see cref="QueryField"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the <see cref="QueryChannel"/> corresponding to the <see cref="QueryField"/>.
    /// </summary>
    public QueryChannel GetChannel()
    {
        var handle = new QueryChannelHandle();
        var successful = false;
        tiledb_query_channel_t* channel = null;
        try
        {
            using (var ctxHandle = _ctx.Handle.Acquire())
            using (var queryHandle = _handle.Acquire())
            {
                _ctx.handle_error(Methods.tiledb_field_channel(ctxHandle, queryHandle, &channel));
            }
            successful = true;
        }
        finally
        {
            if (successful)
            {
                handle.InitHandle(_ctx, channel);
            }
            else
            {
                handle.SetHandleAsInvalid();
            }
        }

        return new QueryChannel(_ctx, _query, handle);
    }

    /// <inheritDoc/>
    public void Dispose()
    {
        _handle.Dispose();
    }
}

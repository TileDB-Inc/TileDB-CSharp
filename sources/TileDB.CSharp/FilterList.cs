using System;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp;

public sealed unsafe class FilterList : IDisposable
{
    private readonly FilterListHandle _handle;
    private readonly Context _ctx;
    private bool _disposed;

    public FilterList(Context ctx) 
    {
        _ctx = ctx;
        _handle = FilterListHandle.Create(_ctx);
    }

    internal FilterList(Context ctx, FilterListHandle handle) 
    {
        _ctx = ctx;
        _handle = handle;
    }

    public void Dispose()
    {
        Dispose(true);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing && (!_handle.IsInvalid))
        {
            _handle.Dispose();
        }

        _disposed = true;
    }

    internal FilterListHandle Handle => _handle;

    /// <summary>
    /// Add a filter.
    /// </summary>
    /// <param name="filter"></param>
    public void AddFilter(Filter filter)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var filterHandle = filter.Handle.Acquire();
        _ctx.handle_error(Methods.tiledb_filter_list_add_filter(ctxHandle, handle, filterHandle));
    }

    /// <summary>
    /// Set maximum chunk size.
    /// </summary>
    /// <param name="maxChunkSize"></param>
    public void SetMaxChunkSize(uint maxChunkSize)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_filter_list_set_max_chunk_size(ctxHandle, handle, maxChunkSize));
    }

    /// <summary>
    /// Get maximum chunk size.
    /// </summary>
    /// <returns></returns>
    public uint MaxChunkSize()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        uint result;
        _ctx.handle_error(Methods.tiledb_filter_list_get_max_chunk_size(ctxHandle, handle, &result));
        return result;
    }

    /// <summary>
    /// Get number of filter.
    /// </summary>
    /// <returns></returns>
    public uint NFilters()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        uint result;
        _ctx.handle_error(Methods.tiledb_filter_list_get_nfilters(ctxHandle, handle, &result));
        return result;
    }

    /// <summary>
    /// Get filter from index.
    /// </summary>
    /// <param name="filterIndex"></param>
    /// <returns></returns>
    public Filter Filter(uint filterIndex)
    {
        var handle = new FilterHandle();
        var successful = false;
        tiledb_filter_t* filter_p = null;
        try
        {
            using (var ctxHandle = _ctx.Handle.Acquire())
            using (var filterListHandle = _handle.Acquire())
            {
                _ctx.handle_error(Methods.tiledb_filter_list_get_filter_from_index(ctxHandle, filterListHandle, filterIndex, &filter_p));
            }
            successful = true;
        }
        finally
        {
            if (successful)
            {
                handle.InitHandle(filter_p);
            }
            else
            {
                handle.SetHandleAsInvalid();
            }
        }

        return new Filter(_ctx, handle);
    }

}

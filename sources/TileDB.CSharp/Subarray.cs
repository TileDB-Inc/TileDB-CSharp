using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TileDB.CSharp.Marshalling;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp;

/// <summary>
/// Represents a TileDB subarray object.
/// </summary>
public sealed unsafe class Subarray : IDisposable
{
    private readonly Array _array;
    private readonly Context _ctx;
    private readonly SubarrayHandle _handle;

    internal SubarrayHandle Handle => _handle;

    /// <summary>
    /// Creates a <see cref="Subarray"/>.
    /// </summary>
    /// <param name="array">The <see cref="Array"/> the subarray is associated with.</param>
    public Subarray(Array array)
    {
        _array = array;
        _ctx = array.Context();
        _handle = SubarrayHandle.Create(_ctx, array.Handle);
    }

    /// <summary>
    /// Creates a <see cref="Subarray"/> with the ability to toggle range coalescing.
    /// </summary>
    /// <param name="array">The <see cref="Array"/> the subarray is associated with.</param>
    /// <param name="coalesceRanges">Whether to coalesce adjacent ranges as they are added.
    /// Optional, defaults to <see langword="true"/>.</param>
    public Subarray(Array array, bool coalesceRanges) : this(array)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_subarray_set_coalesce_ranges(ctxHandle, handle, coalesceRanges ? 1 : 0));
    }

    internal Subarray(Context ctx, Array array, SubarrayHandle handle)
    {
        _ctx = ctx;
        _array = array;
        _handle = handle;
    }

    /// <summary>
    /// Disposes this <see cref="Subarray"/>.
    /// </summary>
    public void Dispose()
    {
        _handle.Dispose();
    }

    /// <summary>
    /// Sets a <see cref="Config"/> to this <see cref="Subarray"/>
    /// </summary>
    /// <param name="config">The config to set.</param>
    /// <remarks>
    /// The following options from <paramref name="config"/> will be considered:
    /// <list type="bullet">
    /// <item><c>sm.memory_budget</c></item>
    /// <item><c>sm.memory_budget_var</c></item>
    /// <item><c>sm.sub_partitioner_memory_budget</c></item>
    /// <item><c>sm.var_offsets.mode</c></item>
    /// <item><c>sm.var_offsets.extra_element</c></item>
    /// <item><c>sm.var_offsets.bitsize</c></item>
    /// <item><c>sm.check_coord_dups</c></item>
    /// <item><c>sm.check_coord_oob</c></item>
    /// <item><c>sm.check_global_order</c></item>
    /// <item><c>sm.dedup_coords</c></item>
    /// </list>
    /// </remarks>
    public void SetConfig(Config config)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var configHandle = config.Handle.Acquire();
        _ctx.handle_error(Methods.tiledb_subarray_set_config(ctxHandle, handle, configHandle));
    }

    private (DataType DataType, uint NumberOfDimensions) GetDomainInfo()
    {
        using var schema = _array.Schema();
        using var domain = schema.Domain();
        return (domain.Type(), domain.NDim());
    }

    /// <summary>
    /// Sets a subarray, defined in the order dimensions were added.
    /// </summary>
    /// <typeparam name="T">The type of the dimensions.</typeparam>
    /// <param name="data">An array of <c>[start,stop]</c> values per dimension.</param>
    /// <exception cref="NotSupportedException"><typeparamref name="T"/> is not supported.</exception>
    /// <exception cref="InvalidOperationException"><typeparamref name="T"/> does not match the dimension's type.</exception>
    /// <exception cref="ArgumentException"><paramref name="data"/> does not contain exactly twice as many items as the number of dimensions.</exception>
    /// <seealso cref="SetSubarray{T}(ReadOnlySpan{T})"/>
    public void SetSubarray<T>(params T[] data) where T : struct
    {
        ErrorHandling.ThrowIfNull(data);
        SetSubarray<T>(data.AsSpan());
    }

    /// <summary>
    /// Sets a subarray, defined in the order dimensions were added.
    /// </summary>
    /// <typeparam name="T">The type of the dimensions.</typeparam>
    /// <param name="data">A read-only span of <c>[start,stop]</c> values per dimension.</param>
    /// <exception cref="NotSupportedException"><typeparamref name="T"/> is not supported.</exception>
    /// <exception cref="InvalidOperationException"><typeparamref name="T"/> does not match the dimension's type.</exception>
    /// <exception cref="ArgumentException"><paramref name="data"/> does not contain exactly twice as many items as the number of dimensions.</exception>
    /// <seealso cref="SetSubarray{T}(T[])"/>
    public void SetSubarray<T>(ReadOnlySpan<T> data) where T : struct
    {
        ErrorHandling.ThrowIfManagedType<T>();
        (var domainType, var nDim) = GetDomainInfo();

        ErrorHandling.CheckDataType<T>(domainType);
        if (data.Length != nDim * 2)
        {
            ThrowHelpers.ThrowSubarrayLengthMismatch(nameof(data));
        }

        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        fixed (T* dataPtr = &MemoryMarshal.GetReference(data))
        {
            _ctx.handle_error(Methods.tiledb_subarray_set_subarray(ctxHandle, handle, dataPtr));
        }
    }

    private void ValidateType<T>(string name) where T : struct
    {
        ErrorHandling.ThrowIfManagedType<T>();
        using var schema = _array.Schema();
        using var domain = schema.Domain();
        using var dimension = domain.Dimension(name);
        ErrorHandling.CheckDataType<T>(dimension.Type());
    }

    private void ValidateType<T>(uint index) where T : struct
    {
        ErrorHandling.ThrowIfManagedType<T>();
        using var schema = _array.Schema();
        using var domain = schema.Domain();
        using var dimension = domain.Dimension(index);
        ErrorHandling.CheckDataType<T>(dimension.Type());
    }

    /// <summary>
    /// Adds a 1D range along a subarray dimension index, in the form (start, end).
    /// </summary>
    /// <typeparam name="T">The dimension's type.</typeparam>
    /// <param name="dimensionIndex">The dimension's index.</param>
    /// <param name="start">The start of the dimension's range.</param>
    /// <param name="end">The end of the dimension's range.</param>
    /// <exception cref="NotSupportedException"><typeparamref name="T"/> is not supported.</exception>
    public void AddRange<T>(uint dimensionIndex, T start, T end) where T : struct
    {
        ValidateType<T>(dimensionIndex);
        AddRange(dimensionIndex, &start, &end, null);
    }

    /// <summary>
    /// Adds a 1D range along a subarray dimension name, specified by its name, in the form (start, end).
    /// </summary>
    /// <typeparam name="T">The dimension's type.</typeparam>
    /// <param name="dimensionName">The dimension's name.</param>
    /// <param name="start">The start of the dimension's range.</param>
    /// <param name="end">The end of the dimension's range.</param>
    /// <exception cref="NotSupportedException"><typeparamref name="T"/> is not supported.</exception>
    public void AddRange<T>(string dimensionName, T start, T end) where T : struct
    {
        ValidateType<T>(dimensionName);
        AddRange(dimensionName, &start, &end, null);
    }

    // TODO: Make it public once the Core supports strides.
    /// <summary>
    /// Adds a 1D range along a subarray dimension index, in the form (start, end, stride).
    /// </summary>
    /// <typeparam name="T">The dimension's type.</typeparam>
    /// <param name="dimensionIndex">The dimension's index.</param>
    /// <param name="start">The start of the dimension's range.</param>
    /// <param name="end">The end of the dimension's range.</param>
    /// <param name="stride">The stride between dimension range values</param>
    /// <exception cref="NotSupportedException"><typeparamref name="T"/> is not supported.</exception>
    private void AddRange<T>(uint dimensionIndex, T start, T end, T stride) where T : struct
    {
        ValidateType<T>(dimensionIndex);
        AddRange(dimensionIndex, &start, &end, &stride);
    }

    // TODO: Make it public once the Core supports strides.
    /// <summary>
    /// Adds a 1D range along a subarray dimension name, specified by its name, in the form(start, end, stride).
    /// </summary>
    /// <typeparam name="T">The dimension's type.</typeparam>
    /// <param name="dimensionName">The dimension's name.</param>
    /// <param name="start">The start of the dimension's range.</param>
    /// <param name="end">The end of the dimension's range.</param>
    /// <param name="stride">The stride between dimension range values</param>
    /// <exception cref="NotSupportedException"><typeparamref name="T"/> is not supported.</exception>
    private void AddRange<T>(string dimensionName, T start, T end, T stride) where T : struct
    {
        ValidateType<T>(dimensionName);
        AddRange(dimensionName, &start, &end, &stride);
    }

    private void AddRange(uint dimensionIndex, void* start, void* end, void* stride)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_subarray_add_range(ctxHandle, handle, dimensionIndex, start, end, stride));
    }

    private void AddRange(string dimensionName, void* start, void* end, void* stride)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var ms_name = new MarshaledString(dimensionName);
        _ctx.handle_error(Methods.tiledb_subarray_add_range_by_name(ctxHandle, handle, ms_name, start, end, stride));
    }

    /// <summary>
    /// Adds a 1D string range along a subarray dimension index, in the form (start, end).
    /// </summary>
    /// <param name="dimensionIndex">The dimension's index.</param>
    /// <param name="start">The start of the dimension's range.</param>
    /// <param name="end">The end of the dimension's range.</param>
    public void AddRange(uint dimensionIndex, string start, string end)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var ms_start = new MarshaledString(start);
        using var ms_end = new MarshaledString(end);
        _ctx.handle_error(Methods.tiledb_subarray_add_range_var(ctxHandle, handle, dimensionIndex, ms_start, (ulong)ms_start.Length, ms_end, (ulong)ms_end.Length));
    }

    /// <summary>
    /// Adds a 1D string range along a subarray dimension name, in the form (start, end).
    /// </summary>
    /// <param name="dimensionName">The dimension's name.</param>
    /// <param name="start">The start of the dimension's range.</param>
    /// <param name="end">The end of the dimension's range.</param>
    public void AddRange(string dimensionName, string start, string end)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var ms_name = new MarshaledString(dimensionName);
        using var ms_start = new MarshaledString(start);
        using var ms_end = new MarshaledString(end);
        _ctx.handle_error(Methods.tiledb_subarray_add_range_var_by_name(ctxHandle, handle, ms_name, ms_start, (ulong)ms_start.Length, ms_end, (ulong)ms_end.Length));
    }

    /// <summary>
    /// Gets the number of ranges for a given dimension index.
    /// </summary>
    /// <param name="dimensionIndex">The dimension's index.</param>
    public ulong GetRangeCount(uint dimensionIndex)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        ulong result;
        _ctx.handle_error(Methods.tiledb_subarray_get_range_num(ctxHandle, handle, dimensionIndex, &result));
        return result;
    }

    /// <summary>
    /// Gets the number of ranges for a given dimension name.
    /// </summary>
    /// <param name="dimensionName">The dimension's name.</param>
    public ulong GetRangeCount(string dimensionName)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var ms_dimensionName = new MarshaledString(dimensionName);
        ulong result;
        _ctx.handle_error(Methods.tiledb_subarray_get_range_num_from_name(ctxHandle, handle, ms_dimensionName, &result));
        return result;
    }

    /// <summary>
    /// Gets the range for a given dimension index and range index.
    /// </summary>
    /// <typeparam name="T">The dimension's type.</typeparam>
    /// <param name="dimensionIndex">The dimension's index.</param>
    /// <param name="rangeIndex">The range's index.</param>
    /// <returns>The dimension's start and end values.</returns>
    /// <exception cref="NotSupportedException"><typeparamref name="T"/> is not supported.</exception>
    public (T Start, T End) GetRange<T>(uint dimensionIndex, uint rangeIndex) where T : struct
    {
        ValidateType<T>(dimensionIndex);
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        void* startPtr, endPtr, stridePtr_unused;
        _ctx.handle_error(Methods.tiledb_subarray_get_range(ctxHandle, handle, dimensionIndex, rangeIndex, &startPtr, &endPtr, &stridePtr_unused));
        return (Unsafe.ReadUnaligned<T>(startPtr), Unsafe.ReadUnaligned<T>(endPtr));
    }

    /// <summary>
    /// Gets the range for a given dimension name and range index.
    /// </summary>
    /// <typeparam name="T">The dimension's type.</typeparam>
    /// <param name="dimensionName">The dimension's name.</param>
    /// <param name="rangeIndex">The range's index.</param>
    /// <returns>The dimension's start and end values.</returns>
    /// <exception cref="NotSupportedException"><typeparamref name="T"/> is not supported.</exception>
    public (T Start, T End) GetRange<T>(string dimensionName, uint rangeIndex) where T : struct
    {
        ValidateType<T>(dimensionName);
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var ms_dimensionName = new MarshaledString(dimensionName);
        void* startPtr, endPtr, stridePtr_unused;
        _ctx.handle_error(Methods.tiledb_subarray_get_range_from_name(ctxHandle, handle, ms_dimensionName, rangeIndex, &startPtr, &endPtr, &stridePtr_unused));
        return (Unsafe.ReadUnaligned<T>(startPtr), Unsafe.ReadUnaligned<T>(endPtr));
    }

    /// <summary>
    /// Gets the string range for a given dimension index and range index.
    /// </summary>
    /// <param name="dimensionIndex">The dimension's index.</param>
    /// <param name="rangeIndex">The range's index.</param>
    /// <returns>The dimension's start and end values.</returns>
    public (string Start, string End) GetStringRange(uint dimensionIndex, uint rangeIndex)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        ulong start_size;
        ulong end_size;
        _ctx.handle_error(Methods.tiledb_subarray_get_range_var_size(ctxHandle, handle, dimensionIndex, rangeIndex, &start_size, &end_size));

        using var startBuffer = new ScratchBuffer<byte>(checked((int)start_size), stackalloc byte[128]);
        using var endBuffer = new ScratchBuffer<byte>(checked((int)end_size), stackalloc byte[128]);
        fixed (byte* startPtr = startBuffer, endPtr = endBuffer)
        {
            _ctx.handle_error(Methods.tiledb_subarray_get_range_var(ctxHandle, handle, dimensionIndex, rangeIndex, startPtr, endPtr));
        }

        var startStr = MarshaledStringOut.GetString(startBuffer.Span);
        var endStr = MarshaledStringOut.GetString(endBuffer.Span);
        return (startStr, endStr);
    }

    /// <summary>
    /// Gets the string range for a given dimension name and range index.
    /// </summary>
    /// <param name="dimensionName">The dimension's name.</param>
    /// <param name="rangeIndex">The range's index.</param>
    /// <returns>The dimension's start and end values.</returns>
    public (string Start, string End) GetStringRange(string dimensionName, uint rangeIndex)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var ms_dimensionName = new MarshaledString(dimensionName);
        ulong start_size;
        ulong end_size;
        _ctx.handle_error(Methods.tiledb_subarray_get_range_var_size_from_name(ctxHandle, handle, ms_dimensionName, rangeIndex, &start_size, &end_size));

        using var startBuffer = new ScratchBuffer<byte>(checked((int)start_size), stackalloc byte[128]);
        using var endBuffer = new ScratchBuffer<byte>(checked((int)end_size), stackalloc byte[128]);
        fixed (byte* startPtr = startBuffer, endPtr = endBuffer)
        {
            _ctx.handle_error(Methods.tiledb_subarray_get_range_var_from_name(ctxHandle, handle, ms_dimensionName, rangeIndex, startPtr, endPtr));
        }

        var startStr = MarshaledStringOut.GetString(startBuffer.Span);
        var endStr = MarshaledStringOut.GetString(endBuffer.Span);
        return (startStr, endStr);
    }
}

using System;
using System.Runtime.CompilerServices;
using TileDB.CSharp.Marshalling;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp;

/// <summary>
/// Represents a TileDB fragment info object.
/// </summary>
public unsafe sealed class FragmentInfo : IDisposable
{
    private readonly Context _ctx;
    private readonly FragmentInfoHandle _handle;

    internal FragmentInfoHandle Handle => _handle;

    /// <summary>
    /// Creates a <see cref="FragmentInfo"/> object.
    /// </summary>
    /// <param name="ctx">The <see cref="Context"/> associated with this object.</param>
    /// <param name="uri">The URI of the array to load the fragment info for.</param>
    public FragmentInfo(Context ctx, string uri)
    {
        _ctx = ctx;
        using var uri_ms = new MarshaledString(uri);
        _handle = FragmentInfoHandle.Create(ctx, uri_ms);
    }

    /// <summary>
    /// Disposes this <see cref="FragmentInfo"/> object.
    /// </summary>
    public void Dispose()
    {
        _handle.Dispose();
    }

    /// <summary>
    /// Loads the fragment info.
    /// </summary>
    public void Load()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_fragment_info_load(ctxHandle, handle));
    }

    /// <summary>
    /// Creates a <see cref="Config"/> object that contains this <see cref="FragmentInfo"/>'s configuration.
    /// </summary>
    public Config GetConfig()
    {
        var handle = new ConfigHandle();
        bool successful = false;
        tiledb_config_t* config = null;
        try
        {
            using (var ctx = _ctx.Handle.Acquire())
            using (var fragmentInfoHandle = _handle.Acquire())
            {
                _ctx.handle_error(Methods.tiledb_fragment_info_get_config(ctx, fragmentInfoHandle, &config));
            }
            successful = true;
        }
        finally
        {
            if (successful)
            {
                handle.InitHandle(config);
            }
            else
            {
                handle.SetHandleAsInvalid();
            }
        }

        return new Config(handle);
    }

    /// <summary>
    /// Set the fragment info object's <see cref="Config"/>.
    /// </summary>
    /// <param name="config">The <see cref="Config"/> object to set.</param>
    public void SetConfig(Config config)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var configHandle = config.Handle.Acquire();
        _ctx.handle_error(Methods.tiledb_fragment_info_set_config(ctxHandle, handle, configHandle));
    }

    /// <summary>
    /// The number of fragments in the array.
    /// </summary>
    public uint FragmentCount
    {
        get
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            uint result;
            _ctx.handle_error(Methods.tiledb_fragment_info_get_fragment_num(ctxHandle, handle, &result));
            return result;
        }
    }

    /// <summary>
    /// The number of fragments to vacuum in the array.
    /// </summary>
    public uint FragmentToVacuumCount
    {
        get
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            uint result;
            _ctx.handle_error(Methods.tiledb_fragment_info_get_to_vacuum_num(ctxHandle, handle, &result));
            return result;
        }
    }

    /// <summary>
    /// The number of fragments with unconsolidated metadata in the array.
    /// </summary>
    public uint FragmentWithUnconsolidatedMetadataCount
    {
        get
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            uint result;
            _ctx.handle_error(Methods.tiledb_fragment_info_get_unconsolidated_metadata_num(ctxHandle, handle, &result));
            return result;
        }
    }

    /// <summary>
    /// The number of cells written to the fragments by the user.
    /// </summary>
    public ulong TotalCellCount
    {
        get
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            ulong result;
            _ctx.handle_error(Methods.tiledb_fragment_info_get_total_cell_num(ctxHandle, handle, &result));
            return result;
        }
    }

    /// <summary>
    /// Gets the <see cref="ArraySchema"/> of a fragment.
    /// </summary>
    /// <param name="fragmentIndex">The index of the fragment of interest.</param>
    /// <remarks>
    /// The maximum value <paramref name="fragmentIndex"/> can take
    /// is determined by the <see cref="FragmentCount"/> property.
    /// </remarks>
    public ArraySchema GetSchema(uint fragmentIndex)
    {
        var handle = new ArraySchemaHandle();
        var successful = false;
        tiledb_array_schema_t* schema = null;
        var ctx = _ctx;
        try
        {
            using var ctxHandle = ctx.Handle.Acquire();
            using var fragmentInfoHandle = _handle.Acquire();
            ctx.handle_error(Methods.tiledb_fragment_info_get_array_schema(ctxHandle, fragmentInfoHandle, fragmentIndex, &schema));
            successful = true;
        }
        finally
        {
            if (successful)
            {
                handle.InitHandle(schema);
            }
            else
            {
                handle.SetHandleAsInvalid();
            }
        }

        return new ArraySchema(ctx, handle);
    }

    /// <summary>
    /// Gets the schema name of a fragment.
    /// </summary>
    /// <param name="fragmentIndex">The index of the fragment of interest.</param>
    /// <remarks>
    /// The maximum value <paramref name="fragmentIndex"/> can take
    /// is determined by the <see cref="FragmentCount"/> property.
    /// </remarks>
    public string GetSchemaName(uint fragmentIndex)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        sbyte* name;
        _ctx.handle_error(Methods.tiledb_fragment_info_get_array_schema_name(ctxHandle, handle, fragmentIndex, &name));
        return MarshaledStringOut.GetStringFromNullTerminated(name);
    }

    /// <summary>
    /// Gets the number of cells written in a fragment.
    /// </summary>
    /// <param name="fragmentIndex">The index of the fragment of interest.</param>
    /// <remarks>
    /// The maximum value <paramref name="fragmentIndex"/> can take
    /// is determined by the <see cref="FragmentCount"/> property.
    /// </remarks>
    public ulong GetCellsWritten(uint fragmentIndex)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        ulong result;
        _ctx.handle_error(Methods.tiledb_fragment_info_get_cell_num(ctxHandle, handle, fragmentIndex, &result));
        return result;
    }

    /// <summary>
    /// Gets the URI of a fragment to vacuum.
    /// </summary>
    /// <param name="fragmentIndex">The index of the fragment of interest.</param>
    /// <remarks>
    /// The maximum value <paramref name="fragmentIndex"/> can take
    /// is determined by the <see cref="FragmentToVacuumCount"/> property.
    /// </remarks>
    public string GetFragmentToVacuumUri(uint fragmentIndex)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        sbyte* uri;
        _ctx.handle_error(Methods.tiledb_fragment_info_get_to_vacuum_uri(ctxHandle, handle, fragmentIndex, &uri));
        return MarshaledStringOut.GetStringFromNullTerminated(uri);
    }

    /// <summary>
    /// Gets the name of a fragment.
    /// </summary>
    /// <param name="fragmentIndex">The index of the fragment of interest.</param>
    /// <remarks>
    /// The maximum value <paramref name="fragmentIndex"/> can take
    /// is determined by the <see cref="FragmentCount"/> property.
    /// </remarks>
    /// <seealso cref="Array.ConsolidateFragments"/>
    public string GetFragmentName(uint fragmentIndex)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var nameHolder = new StringHandleHolder();
        _ctx.handle_error(Methods.tiledb_fragment_info_get_fragment_name_v2(ctxHandle, handle, fragmentIndex, &nameHolder._handle));
        return nameHolder.ToString();
    }

    /// <summary>
    /// Gets the URI of a fragment.
    /// </summary>
    /// <param name="fragmentIndex">The index of the fragment of interest.</param>
    /// <remarks>
    /// The maximum value <paramref name="fragmentIndex"/> can take
    /// is determined by the <see cref="FragmentCount"/> property.
    /// </remarks>
    public string GetFragmentUri(uint fragmentIndex)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        sbyte* uri;
        _ctx.handle_error(Methods.tiledb_fragment_info_get_fragment_uri(ctxHandle, handle, fragmentIndex, &uri));
        return MarshaledStringOut.GetStringFromNullTerminated(uri);
    }

    /// <summary>
    /// Gets the format version of a fragment.
    /// </summary>
    /// <param name="fragmentIndex">The index of the fragment of interest.</param>
    /// <remarks>
    /// The maximum value <paramref name="fragmentIndex"/> can take
    /// is determined by the <see cref="FragmentCount"/> property.
    /// </remarks>
    public uint GetFormatVersion(uint fragmentIndex)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        uint result;
        _ctx.handle_error(Methods.tiledb_fragment_info_get_version(ctxHandle, handle, fragmentIndex, &result));
        return result;
    }

    /// <summary>
    /// Gets the size in bytes of a fragment.
    /// </summary>
    /// <param name="fragmentIndex">The index of the fragment of interest.</param>
    /// <remarks>
    /// The maximum value <paramref name="fragmentIndex"/> can take
    /// is determined by the <see cref="FragmentCount"/> property.
    /// </remarks>
    public ulong GetFragmentSize(uint fragmentIndex)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        ulong result;
        _ctx.handle_error(Methods.tiledb_fragment_info_get_fragment_size(ctxHandle, handle, fragmentIndex, &result));
        return result;
    }

    /// <summary>
    /// Checks whether a fragment has consolidated metadata.
    /// </summary>
    /// <param name="fragmentIndex">The index of the fragment of interest.</param>
    /// <remarks>
    /// The maximum value <paramref name="fragmentIndex"/> can take
    /// is determined by the <see cref="FragmentCount"/> property.
    /// </remarks>
    public bool HasConsolidatedMetadata(uint fragmentIndex)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        int result;
        _ctx.handle_error(Methods.tiledb_fragment_info_has_consolidated_metadata(ctxHandle, handle, fragmentIndex, &result));
        return result == 1;
    }

    /// <summary>
    /// Checks whether a fragment is dense.
    /// </summary>
    /// <param name="fragmentIndex">The index of the fragment of interest.</param>
    /// <remarks>
    /// The maximum value <paramref name="fragmentIndex"/> can take
    /// is determined by the <see cref="FragmentCount"/> property.
    /// </remarks>
    public bool IsDense(uint fragmentIndex)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        int result;
        _ctx.handle_error(Methods.tiledb_fragment_info_get_dense(ctxHandle, handle, fragmentIndex, &result));
        return result == 1;
    }

    /// <summary>
    /// Checks whether a fragment is sparse.
    /// </summary>
    /// <param name="fragmentIndex">The index of the fragment of interest.</param>
    /// <remarks>
    /// The maximum value <paramref name="fragmentIndex"/> can take
    /// is determined by the <see cref="FragmentCount"/> property.
    /// </remarks>
    public bool IsSparse(uint fragmentIndex)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        int result;
        _ctx.handle_error(Methods.tiledb_fragment_info_get_sparse(ctxHandle, handle, fragmentIndex, &result));
        return result == 1;
    }

    /// <summary>
    /// Gets the timestamp range of a fragment.
    /// </summary>
    /// <param name="fragmentIndex">The index of the fragment of interest.</param>
    /// <returns>The start and end timestamps of the fragment.</returns>
    /// <remarks>
    /// The maximum value <paramref name="fragmentIndex"/> can take
    /// is determined by the <see cref="FragmentCount"/> property.
    /// </remarks>
    public (ulong Start, ulong End) GetTimestampRange(uint fragmentIndex)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        ulong start, end;
        _ctx.handle_error(Methods.tiledb_fragment_info_get_timestamp_range(ctxHandle, handle, fragmentIndex, &start, &end));
        return (start, end);
    }

    /// <summary>
    /// Gets the number of Minimum Bounded Rectangles (MBRs) of a fragment.
    /// </summary>
    /// <param name="fragmentIndex">The index of the fragment of interest.</param>
    /// <remarks>
    /// The maximum value <paramref name="fragmentIndex"/> can take
    /// is determined by the <see cref="FragmentCount"/> property.
    /// </remarks>
    public ulong GetMinimumBoundedRectangleCount(uint fragmentIndex)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        ulong result;
        _ctx.handle_error(Methods.tiledb_fragment_info_get_mbr_num(ctxHandle, handle, fragmentIndex, &result));
        return result;
    }

    /// <summary>
    /// Gets the Minimum Bounded Rectangle (MBR) from one of a fragment's dimensions, identified by its index.
    /// </summary>
    /// <typeparam name="T">The dimension's data type.</typeparam>
    /// <param name="fragmentIndex">The index of the fragment of interest.</param>
    /// <param name="minimumBoundedRectangleIndex">The index of the MBR of interest.</param>
    /// <param name="dimensionIndex">The index of the dimension of interest, following
    /// the order as it was defined in the domain of the array schema.</param>
    /// <returns>The start and end values of the MBR, inclusive.</returns>
    /// <exception cref="NotSupportedException"><typeparamref name="T"/> is unsupported.</exception>
    /// <exception cref="InvalidOperationException"><typeparamref name="T"/> is incompatible
    /// with the dimension's data type.</exception>
    public (T Start, T End) GetMinimumBoundedRectangle<T>(uint fragmentIndex, uint minimumBoundedRectangleIndex, uint dimensionIndex)
    {
        DataType dataType = GetDimensionType(fragmentIndex, dimensionIndex);
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            if (typeof(T) == typeof(string) && EnumUtil.IsStringType(dataType))
            {
                (string startStr, string endStr) =
                    GetStringMinimumBoundedRectangle(fragmentIndex, minimumBoundedRectangleIndex, dimensionIndex, dataType);
                return ((T)(object)startStr, (T)(object)endStr);
            }
            ThrowHelpers.ThrowTypeMismatch(dataType);
            return default;
        }
        ValidateDomainType<T>(dataType);

        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        // We always allocate twice the maximum possible data type to avoid buffer overflows.
        // If the dimension is of type uint64 and we pass a buffer of two bytes, it will write past the buffer's boundaries.
        byte* mbr = stackalloc byte[sizeof(ulong) * 2];
        _ctx.handle_error(Methods.tiledb_fragment_info_get_mbr_from_index(ctxHandle, handle, fragmentIndex, minimumBoundedRectangleIndex, dimensionIndex, mbr));

        var start = Unsafe.ReadUnaligned<T>(mbr);
        var end = Unsafe.ReadUnaligned<T>(mbr + Unsafe.SizeOf<T>());
        return (start, end);
    }

    /// <summary>
    /// Gets the Minimum Bounded Rectangle (MBR) from one of a fragment's dimensions, identified by its name.
    /// </summary>
    /// <typeparam name="T">The dimension's data type.</typeparam>
    /// <param name="fragmentIndex">The index of the fragment of interest.</param>
    /// <param name="minimumBoundedRectangleIndex">The index of the MBR of interest.</param>
    /// <param name="dimensionName">The name of the dimension of interest.</param>
    /// <returns>The start and end values of the MBR, inclusive.</returns>
    /// <exception cref="NotSupportedException"><typeparamref name="T"/> is unsupported.</exception>
    /// <exception cref="InvalidOperationException"><typeparamref name="T"/> is incompatible
    /// with the dimension's data type.</exception>
    public (T Start, T End) GetMinimumBoundedRectangle<T>(uint fragmentIndex, uint minimumBoundedRectangleIndex, string dimensionName)
    {
        DataType dataType = GetDimensionType(fragmentIndex, dimensionName);
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            if (typeof(T) == typeof(string) && EnumUtil.IsStringType(dataType))
            {
                (string startStr, string endStr) =
                    GetStringMinimumBoundedRectangle(fragmentIndex, minimumBoundedRectangleIndex, dimensionName, dataType);
                return ((T)(object)startStr, (T)(object)endStr);
            }
            ThrowHelpers.ThrowTypeMismatch(dataType);
            return default;
        }
        ValidateDomainType<T>(dataType);

        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var ms_dimensionName = new MarshaledString(dimensionName);
        // We always allocate twice the maximum possible data type to avoid buffer overflows.
        // If the dimension is of type uint64 and we pass a buffer of two bytes, it will write past the buffer's boundaries.
        byte* mbr = stackalloc byte[sizeof(ulong) * 2];
        _ctx.handle_error(Methods.tiledb_fragment_info_get_mbr_from_name(ctxHandle,
            handle, fragmentIndex, minimumBoundedRectangleIndex, ms_dimensionName, mbr));

        var start = Unsafe.ReadUnaligned<T>(mbr);
        var end = Unsafe.ReadUnaligned<T>(mbr + Unsafe.SizeOf<T>());
        return (start, end);
    }

    private (string Start, string End) GetStringMinimumBoundedRectangle(uint fragmentIndex, uint minimumBoundedRectangleIndex, uint dimensionIndex, DataType dataType)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        ulong startSize64, endSize64;
        _ctx.handle_error(Methods.tiledb_fragment_info_get_mbr_var_size_from_index(ctxHandle,
            handle, fragmentIndex, minimumBoundedRectangleIndex, dimensionIndex, &startSize64, &endSize64));
        int startSize = checked((int)startSize64);
        int endSize = checked((int)endSize64);
        using var startBuf = new ScratchBuffer<byte>(startSize, stackalloc byte[128], exactSize: true);
        using var endBuf = new ScratchBuffer<byte>(endSize, stackalloc byte[128], exactSize: true);
        fixed (byte* startBufPtr = startBuf, endBufPtr = endBuf)
        {
            _ctx.handle_error(Methods.tiledb_fragment_info_get_mbr_var_from_index(ctxHandle,
                handle, fragmentIndex, minimumBoundedRectangleIndex, dimensionIndex, startBufPtr, endBufPtr));
        }

        string startStr = MarshaledStringOut.GetString(startBuf.Span, dataType);
        string endStr = MarshaledStringOut.GetString(endBuf.Span, dataType);
        return (startStr, endStr);
    }

    private (string Start, string End) GetStringMinimumBoundedRectangle(uint fragmentIndex, uint minimumBoundedRectangleIndex, string dimensionName, DataType dataType)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var ms_dimensionName = new MarshaledString(dimensionName);
        ulong startSize64, endSize64;
        _ctx.handle_error(Methods.tiledb_fragment_info_get_mbr_var_size_from_name(ctxHandle,
            handle, fragmentIndex, minimumBoundedRectangleIndex, ms_dimensionName, &startSize64, &endSize64));
        int startSize = checked((int)startSize64);
        int endSize = checked((int)endSize64);
        using var startBuf = new ScratchBuffer<byte>(startSize, stackalloc byte[128], exactSize: true);
        using var endBuf = new ScratchBuffer<byte>(endSize, stackalloc byte[128], exactSize: true);
        fixed (byte* startBufPtr = startBuf, endBufPtr = endBuf)
        {
            _ctx.handle_error(Methods.tiledb_fragment_info_get_mbr_var_from_name(ctxHandle, handle,
                fragmentIndex, minimumBoundedRectangleIndex, ms_dimensionName, startBufPtr, endBufPtr));
        }

        string startStr = MarshaledStringOut.GetString(startBuf.Span, dataType);
        string endStr = MarshaledStringOut.GetString(endBuf.Span, dataType);
        return (startStr, endStr);
    }

    /// <summary>
    /// Gets the non-empty domain from one of a fragment's dimensions, identified by its index.
    /// </summary>
    /// <typeparam name="T">The dimension's data type.</typeparam>
    /// <param name="fragmentIndex">The index of the fragment of interest.</param>
    /// <param name="dimensionIndex">The index of the dimension of interest, following
    /// the order as it was defined in the domain of the array schema.</param>
    /// <returns>The start and end values of the domain, inclusive.</returns>
    /// <exception cref="NotSupportedException"><typeparamref name="T"/> is unsupported.</exception>
    /// <exception cref="InvalidOperationException"><typeparamref name="T"/> is incompatible
    /// with the dimension's data type.</exception>
    public (T Start, T End) GetNonEmptyDomain<T>(uint fragmentIndex, uint dimensionIndex)
    {
        DataType dataType = GetDimensionType(fragmentIndex, dimensionIndex);
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            if (typeof(T) == typeof(string) && EnumUtil.IsStringType(dataType))
            {
                (string startStr, string endStr) = GetStringNonEmptyDomain(fragmentIndex, dimensionIndex, dataType);
                return ((T)(object)startStr, (T)(object)endStr);
            }
            ThrowHelpers.ThrowTypeMismatch(dataType);
            return default;
        }
        ValidateDomainType<T>(dataType);

        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        // We always allocate twice the maximum possible data type to avoid buffer overflows.
        // If the dimension is of type uint64 and we pass a buffer of two bytes, it will write past the buffer's boundaries.
        byte* domain = stackalloc byte[sizeof(ulong) * 2];
        _ctx.handle_error(Methods.tiledb_fragment_info_get_non_empty_domain_from_index(ctxHandle,
            handle, fragmentIndex, dimensionIndex, domain));

        var start = Unsafe.ReadUnaligned<T>(domain);
        var end = Unsafe.ReadUnaligned<T>(domain + Unsafe.SizeOf<T>());
        return (start, end);
    }

    /// <summary>
    /// Gets the non-empty domain from one of a fragment's dimensions, identified by its index.
    /// </summary>
    /// <typeparam name="T">The dimension's data type.</typeparam>
    /// <param name="fragmentIndex">The index of the fragment of interest.</param>
    /// <param name="dimensionName">The name of the dimension of interest.</param>
    /// <returns>The start and end values of the domain, inclusive.</returns>
    /// <exception cref="NotSupportedException"><typeparamref name="T"/> is unsupported.</exception>
    /// <exception cref="InvalidOperationException"><typeparamref name="T"/> is incompatible
    /// with the dimension's data type.</exception>
    public (T Start, T End) GetNonEmptyDomain<T>(uint fragmentIndex, string dimensionName)
    {
        DataType dataType = GetDimensionType(fragmentIndex, dimensionName);
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            if (typeof(T) == typeof(string) && EnumUtil.IsStringType(dataType))
            {
                (string startStr, string endStr) = GetStringNonEmptyDomain(fragmentIndex, dimensionName, dataType);
                return ((T)(object)startStr, (T)(object)endStr);
            }
            ThrowHelpers.ThrowTypeMismatch(dataType);
            return default;
        }
        ValidateDomainType<T>(dataType);

        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var ms_dimensionName = new MarshaledString(dimensionName);
        // We always allocate twice the maximum possible data type to avoid buffer overflows.
        // If the dimension is of type uint64 and we pass a buffer of two bytes, it will write past the buffer's boundaries.
        byte* domain = stackalloc byte[sizeof(ulong) * 2];
        _ctx.handle_error(Methods.tiledb_fragment_info_get_non_empty_domain_from_name(ctxHandle,
            handle, fragmentIndex, ms_dimensionName, domain));

        var start = Unsafe.ReadUnaligned<T>(domain);
        var end = Unsafe.ReadUnaligned<T>(domain + Unsafe.SizeOf<T>());
        return (start, end);
    }

    private (string Start, string End) GetStringNonEmptyDomain(uint fragmentIndex, uint dimensionIndex, DataType dataType)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        ulong startSize64, endSize64;
        _ctx.handle_error(Methods.tiledb_fragment_info_get_non_empty_domain_var_size_from_index(ctxHandle,
            handle, fragmentIndex, dimensionIndex, &startSize64, &endSize64));
        int startSize = checked((int)startSize64);
        int endSize = checked((int)endSize64);
        using var startBuf = new ScratchBuffer<byte>(startSize, stackalloc byte[128], exactSize: true);
        using var endBuf = new ScratchBuffer<byte>(endSize, stackalloc byte[128], exactSize: true);
        fixed (byte* startBufPtr = startBuf, endBufPtr = endBuf)
        {
            _ctx.handle_error(Methods.tiledb_fragment_info_get_non_empty_domain_var_from_index(ctxHandle,
                handle, fragmentIndex, dimensionIndex, startBufPtr, endBufPtr));
        }

        string startStr = MarshaledStringOut.GetString(startBuf.Span, dataType);
        string endStr = MarshaledStringOut.GetString(endBuf.Span, dataType);
        return (startStr, endStr);
    }

    private (string Start, string End) GetStringNonEmptyDomain(uint fragmentIndex, string dimensionName, DataType dataType)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var ms_dimensionName = new MarshaledString(dimensionName);
        ulong startSize64, endSize64;
        _ctx.handle_error(Methods.tiledb_fragment_info_get_non_empty_domain_var_size_from_name(ctxHandle,
            handle, fragmentIndex, ms_dimensionName, &startSize64, &endSize64));
        int startSize = checked((int)startSize64);
        int endSize = checked((int)endSize64);
        using var startBuf = new ScratchBuffer<byte>(startSize, stackalloc byte[128], exactSize: true);
        using var endBuf = new ScratchBuffer<byte>(endSize, stackalloc byte[128], exactSize: true);
        fixed (byte* startBufPtr = startBuf, endBufPtr = endBuf)
        {
            _ctx.handle_error(Methods.tiledb_fragment_info_get_non_empty_domain_var_from_name(ctxHandle,
                handle, fragmentIndex, ms_dimensionName, startBufPtr, endBufPtr));
        }

        string startStr = MarshaledStringOut.GetString(startBuf.Span, dataType);
        string endStr = MarshaledStringOut.GetString(endBuf.Span, dataType);
        return (startStr, endStr);
    }

    private static void ValidateDomainType<T>(DataType dataType)
    {
        if (Unsafe.SizeOf<T>() > sizeof(ulong))
        {
            ThrowHelpers.ThrowTypeNotSupported();
        }
        if (typeof(T) == typeof(string) && !EnumUtil.IsStringType(dataType))
        {
            ThrowHelpers.ThrowStringTypeMismatch(dataType);
        }
        ErrorHandling.CheckDataType<T>(dataType);
    }

    private DataType GetDimensionType(uint fragmentIndex, uint dimensionIndex)
    {
        using var schema = GetSchema(fragmentIndex);
        using var domain = schema.Domain();
        using var dimension = domain.Dimension(dimensionIndex);
        return dimension.Type();
    }

    private DataType GetDimensionType(uint fragmentIndex, string dimensionName)
    {
        using var schema = GetSchema(fragmentIndex);
        using var domain = schema.Domain();
        using var dimension = domain.Dimension(dimensionName);
        return dimension.Type();
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using TileDB.CSharp.Marshalling;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp;

/// <summary>
/// Represents the non-empty domain of all dimensions of an <see cref="Array"/>.
/// </summary>
public class NonEmptyDomain
{
    private readonly Dictionary<string, object> _dict = new();

    /// <summary>
    /// Adds a value to the <see cref="NonEmptyDomain"/>.
    /// </summary>
    /// <typeparam name="T">The dimension's type. Usually is <see cref="Tuple{T1, T2}"/>
    /// of a primitive integer type or <see cref="string"/>.</typeparam>
    /// <param name="key">The dimension's name.</param>
    /// <param name="value">The dimension's non-empty domain.</param>
    public void Add<T>(string key, T? value)
    {
        if (value != null) _dict.Add(key, value);
    }

    /// <summary>
    /// Gets the non-empty domain of a dimension.
    /// </summary>
    /// <typeparam name="T">The dimension's type. Usually is <see cref="Tuple{T1, T2}"/>
    /// of a primitive integer type or <see cref="string"/>.</typeparam>
    /// <param name="key">The dimension's name.</param>
    public T Get<T>(string key)
    {
        return (T)_dict[key];
    }

    /// <summary>
    /// Tries to get the non-empty domain of a dimension, if it exists.
    /// </summary>
    /// <typeparam name="T">The dimension's type. Usually is <see cref="Tuple{T1, T2}"/>
    /// of a primitive integer type or <see cref="string"/>.</typeparam>
    /// <param name="key">The dimension's name.</param>
    /// <param name="value">A reference where the dimension's non-empty domain will be written to.</param>
    /// <returns>Whether a dimension named <paramref name="key"/> has a non-empty domain.</returns>
    public bool TryGet<T>(string key, [NotNullWhen(true)] out T? value)
    {
        if (_dict.TryGetValue(key, out var result) && result is T value1)
        {
            value = value1;
            return true;
        }
        value = default;
        return false;
    }
}

/// <summary>
/// Represents a TileDB array object.
/// </summary>
public sealed unsafe class Array : IDisposable
{
    private readonly ArrayHandle _handle;
    private readonly Context _ctx;
    private readonly string _uri;

    private readonly ArrayMetadata _metadata;

    /// <summary>
    /// Creates an <see cref="Array"/>.
    /// </summary>
    /// <param name="ctx">The <see cref="CSharp.Context"/> associated with the array.</param>
    /// <param name="uri">The array's URI.</param>
    public Array(Context ctx, string uri)
    {
        _ctx = ctx;
        _uri = uri;
        using var ms_uri = new MarshaledString(_uri);
        _handle = ArrayHandle.Create(_ctx, ms_uri);
        _metadata = new ArrayMetadata(this);
    }

    internal Array(Context ctx, ArrayHandle handle)
    {
        _ctx = ctx;
        _handle = handle;
        _uri = Uri();
        _metadata = new ArrayMetadata(this);
    }

    /// <summary>
    /// Disposes the <see cref="Array"/>.
    /// </summary>
    public void Dispose()
    {
        _handle.Dispose();
    }

    internal ArrayHandle Handle => _handle;

    /// <summary>
    /// Gets the <see cref="CSharp.Context"/> associated with this <see cref="Array"/>.
    /// </summary>
    public Context Context()
    {
        return _ctx;
    }

    /// <summary>
    /// Gets the <see cref="ArrayMetadata"/> object for this <see cref="Array"/>.
    /// </summary>
    public ArrayMetadata Metadata()
    {
        return _metadata;
    }

    /// <summary>
    /// Sets the starting timestamp to use when <see cref="Open"/>ing (and <see cref="Reopen"/>ing) the <see cref="Array"/>.
    /// </summary>
    /// <param name="timestampStart">The starting timestamp, inclusive.</param>
    /// <remarks>
    /// If not set, the default value is zero.
    /// </remarks>
    /// <seealso cref="OpenTimestampStart"/>
    public void SetOpenTimestampStart(ulong timestampStart)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_set_open_timestamp_start(ctxHandle, handle, timestampStart));
    }

    /// <summary>
    /// Sets the ending timestamp to use when <see cref="Open"/>ing (and <see cref="Reopen"/>ing) the <see cref="Array"/>.
    /// </summary>
    /// <param name="timestampEnd">The ending timestamp, inclusive.</param>
    /// <remarks>
    /// If not set, the default value is <see cref="ulong.MaxValue"/> which signifies the current time.
    /// </remarks>
    /// <seealso cref="OpenTimestampEnd"/>
    public void SetOpenTimestampEnd(ulong timestampEnd)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_set_open_timestamp_end(ctxHandle, handle, timestampEnd));
    }

    /// <summary>
    /// Gets the starting timestamp to use when <see cref="Open"/>ing (and <see cref="Reopen"/>ing) the <see cref="Array"/>.
    /// </summary>
    /// <seealso cref="SetOpenTimestampStart"/>
    public ulong OpenTimestampStart()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        ulong timestamp;
        _ctx.handle_error(Methods.tiledb_array_get_open_timestamp_start(ctxHandle, handle, &timestamp));
        return timestamp;
    }

    /// <summary>
    /// Sets the ending timestamp to use when <see cref="Open"/>ing (and <see cref="Reopen"/>ing) the <see cref="Array"/>.
    /// </summary>
    /// <seealso cref="SetOpenTimestampEnd"/>
    public ulong OpenTimestampEnd()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        ulong timestamp;
        _ctx.handle_error(Methods.tiledb_array_get_open_timestamp_end(ctxHandle, handle, &timestamp));
        return timestamp;
    }

    /// <summary>
    /// Opens the <see cref="Array"/>.
    /// </summary>
    /// <param name="queryType">The default query type if <see cref="Query(CSharp.Context, Array)"/> is called with this array.</param>
    public void Open(QueryType queryType)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        var tiledb_query_type = (tiledb_query_type_t)queryType;
        _ctx.handle_error(Methods.tiledb_array_open(ctxHandle, handle, tiledb_query_type));
    }

    /// <summary>
    /// Reopens the <see cref="Array"/>.
    /// </summary>
    public void Reopen()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_reopen(ctxHandle, handle));
    }

    /// <summary>
    /// Checks whether the <see cref="Array"/> is open or not.
    /// </summary>
    public bool IsOpen()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        int int_open;
        _ctx.handle_error(Methods.tiledb_array_is_open(ctxHandle, handle, &int_open));
        return int_open > 0;
    }

    /// <summary>
    /// Sets the <see cref="CSharp.Config"/> to the <see cref="Array"/>.
    /// </summary>
    /// <param name="config">The config to use</param>
    public void SetConfig(Config config)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var configHandle = config.Handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_set_config(ctxHandle, handle, configHandle));
    }

    /// <summary>
    /// Gets the <see cref="CSharp.Config"/> of the <see cref="Array"/>.
    /// </summary>
    public Config Config()
    {
        var handle = new ConfigHandle();
        tiledb_config_t* config = null;
        var successful = false;
        try
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var arrayHandle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_array_get_config(ctxHandle, arrayHandle, &config));
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
    /// Closes the <see cref="Array"/>.
    /// </summary>
    public void Close()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_close(ctxHandle, handle));
    }

    /// <summary>
    /// Gets the <see cref="Array"/>'s <see cref="ArraySchema"/>.
    /// </summary>
    public ArraySchema Schema()
    {
        var handle = new ArraySchemaHandle();
        var successful = false;
        tiledb_array_schema_t* array_schema_p = null;
        try
        {
            using (var ctxHandle = _ctx.Handle.Acquire())
            using (var arrayHandle = _handle.Acquire())
            {
                _ctx.handle_error(Methods.tiledb_array_get_schema(ctxHandle, arrayHandle, &array_schema_p));
            }
            successful = true;
        }
        finally
        {
            if (successful)
            {
                handle.InitHandle(array_schema_p);
            }
            else
            {
                handle.SetHandleAsInvalid();
            }
        }
        return new ArraySchema(_ctx, handle);
    }

    /// <summary>
    /// Gets the <see cref="CSharp.QueryType"/> with which the <see cref="Array"/> was <see cref="Open"/>ed.
    /// </summary>
    public QueryType QueryType()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        tiledb_query_type_t tiledb_query_type;
        _ctx.handle_error(Methods.tiledb_array_get_query_type(ctxHandle, handle, &tiledb_query_type));
        return (QueryType)tiledb_query_type;
    }

    /// <summary>
    /// Creates an <see cref="Array"/> at a given URI.
    /// </summary>
    /// <param name="ctx">The <see cref="CSharp.Context"/> to use.</param>
    /// <param name="uri">The array's URI.</param>
    /// <param name="schema">The array's schema.</param>
    /// <seealso cref="Create(ArraySchema)"/>
    public static void Create(Context ctx, string uri, ArraySchema schema)
    {
        using var ms_uri = new MarshaledString(uri);
        using var ctxHandle = ctx.Handle.Acquire();
        using var schemaHandle = schema.Handle.Acquire();
        ctx.handle_error(Methods.tiledb_array_create(ctxHandle, ms_uri, schemaHandle));
    }

    /// <summary>
    /// Creates the <see cref="Array"/> at <see cref="Uri"/>.
    /// </summary>
    /// <param name="schema">The array's <see cref="ArraySchema"/>.</param>
    /// <seealso cref="Create(CSharp.Context, string, ArraySchema)"/>
    public void Create(ArraySchema schema)
    {
        Create(_ctx, _uri, schema);
    }

    /// <summary>
    /// Applies an <see cref="ArraySchemaEvolution"/> to the schema of an array.
    /// </summary>
    /// <param name="ctx">The <see cref="CSharp.Context"/> to use.</param>
    /// <param name="uri">The array's URI.</param>
    /// <param name="schemaEvolution">The schema evolution to use.</param>
    /// <seealso cref="Evolve(ArraySchemaEvolution)"/>
    public static void Evolve(Context ctx, string uri, ArraySchemaEvolution schemaEvolution)
    {
        using var msUri = new MarshaledString(uri);
        using var ctxHandle = ctx.Handle.Acquire();
        using var schemaEvolutionHandle = schemaEvolution.Handle.Acquire();
        ctx.handle_error(Methods.tiledb_array_evolve(ctxHandle, msUri, schemaEvolutionHandle));
    }

    /// <summary>
    /// Applies an <see cref="ArraySchemaEvolution"/> to the schema of this <see cref="Array"/>.
    /// </summary>
    /// <param name="ctx">The <see cref="CSharp.Context"/> to use.</param>
    /// <param name="schemaEvolution">The schema evolution to use.</param>
    /// <seealso cref="Evolve(ArraySchemaEvolution)"/>
    /// <seealso cref="Evolve(CSharp.Context, string, ArraySchemaEvolution)"/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Evolve(Context ctx, ArraySchemaEvolution schemaEvolution)
    {
        Evolve(ctx, _uri, schemaEvolution);
    }

    /// <summary>
    /// Applies an <see cref="ArraySchemaEvolution"/> to the schema of this <see cref="Array"/>.
    /// </summary>
    /// <param name="schemaEvolution">The schema evolution to use.</param>
    /// <seealso cref="Evolve(CSharp.Context, string, ArraySchemaEvolution)"/>
    public void Evolve(ArraySchemaEvolution schemaEvolution)
    {
        Evolve(_ctx, _uri, schemaEvolution);
    }

    /// <summary>
    /// Consolidates an array.
    /// </summary>
    /// <param name="ctx">The <see cref="CSharp.Context"/> to use.</param>
    /// <param name="uri">The array's URI.</param>
    /// <param name="config">Configuration parameters for the consolidation. Optional.</param>
    public static void Consolidate(Context ctx, string uri, Config? config = null)
    {
        using var ms_uri = new MarshaledString(uri);
        using var ctxHandle = ctx.Handle.Acquire();
        using var configHandle = config?.Handle.Acquire() ?? default;
        ctx.handle_error(Methods.tiledb_array_consolidate(ctxHandle, ms_uri, configHandle));
    }

    /// <summary>
    /// Consolidates the given fragments of an array into a single fragment.
    /// </summary>
    /// <param name="ctx">The <see cref="CSharp.Context"/> to use.</param>
    /// <param name="uri">The array's URI.</param>
    /// <param name="fragments">The names of the fragments to consolidate. They can be retrieved using <see cref="FragmentInfo.GetFragmentName"/>.</param>
    /// <param name="config">Configuration parameters for the consolidation. Optional.</param>
    public static void ConsolidateFragments(Context ctx, string uri, IReadOnlyList<string> fragments, Config? config = null)
    {
        using var ctxHandle = ctx.Handle.Acquire();
        using var ms_uri = new MarshaledString(uri);
        using var msc_fragments = new MarshaledStringCollection(fragments);
        using var configHandle = config?.Handle.Acquire() ?? default;
        ctx.handle_error(Methods.tiledb_array_consolidate_fragments(ctxHandle, ms_uri, (sbyte**)msc_fragments.Strings, (ulong)msc_fragments.Count, configHandle));
    }

    /// <summary>
    /// Vacuums an array.
    /// </summary>
    /// <param name="ctx">The <see cref="CSharp.Context"/> to use.</param>
    /// <param name="uri">The array's URI.</param>
    /// <param name="config">Configuration parameters for the consolidation. Optional.</param>
    public static void Vacuum(Context ctx, string uri, Config? config = null)
    {
        using var ms_uri = new MarshaledString(uri);
        using var ctxHandle = ctx.Handle.Acquire();
        using var configHandle = config?.Handle.Acquire() ?? default;
        ctx.handle_error(Methods.tiledb_array_vacuum(ctxHandle, ms_uri, configHandle));
    }

    /// <summary>
    /// Gets the non-empty domain of all dimensions of the <see cref="Array"/>.
    /// </summary>
    public (NonEmptyDomain, bool) NonEmptyDomain()
    {
        NonEmptyDomain nonEmptyDomain = new();
        using ArraySchema schema = Schema();
        using Domain domain = schema.Domain();

        var ndim = domain.NDim();
        if (ndim == 0)
        {
            return (nonEmptyDomain, true);
        }

        bool isEmptyDomain = true;
        for (uint i = 0; i < ndim; ++i)
        {
            using var dim = domain.Dimension(i);
            var dimName = dim.Name();
            var dimType = EnumUtil.DataTypeToType(dim.Type());

            switch (Type.GetTypeCode(dimType))
            {
                case TypeCode.SByte:
                    GetDomain<sbyte>(this, dimName, i, nonEmptyDomain, ref isEmptyDomain);
                    break;
                case TypeCode.Int16:
                    GetDomain<short>(this, dimName, i, nonEmptyDomain, ref isEmptyDomain);
                    break;
                case TypeCode.Int32:
                    GetDomain<int>(this, dimName, i, nonEmptyDomain, ref isEmptyDomain);
                    break;
                case TypeCode.Int64:
                    GetDomain<long>(this, dimName, i, nonEmptyDomain, ref isEmptyDomain);
                    break;
                case TypeCode.Byte:
                    GetDomain<byte>(this, dimName, i, nonEmptyDomain, ref isEmptyDomain);
                    break;
                case TypeCode.UInt16:
                    GetDomain<ushort>(this, dimName, i, nonEmptyDomain, ref isEmptyDomain);
                    break;
                case TypeCode.UInt32:
                    GetDomain<uint>(this, dimName, i, nonEmptyDomain, ref isEmptyDomain);
                    break;
                case TypeCode.UInt64:
                    GetDomain<ulong>(this, dimName, i, nonEmptyDomain, ref isEmptyDomain);
                    break;
                case TypeCode.Single:
                    GetDomain<float>(this, dimName, i, nonEmptyDomain, ref isEmptyDomain);
                    break;
                case TypeCode.Double:
                    GetDomain<double>(this, dimName, i, nonEmptyDomain, ref isEmptyDomain);
                    break;
                case TypeCode.String:
                    {
                        (string data0, string data1, bool isEmpty) = NonEmptyDomainVar(i);
                        if (!isEmpty)
                        {
                            isEmptyDomain = false;
                        }

                        nonEmptyDomain.Add(dimName, new Tuple<string, string>(data0, data1));
                    }
                    break;
            }
        }

        return (nonEmptyDomain, isEmptyDomain);

        static void GetDomain<T>(Array array, string dimName, uint i, NonEmptyDomain nonEmptyDomain, ref bool isEmptyDomain) where T : struct
        {
            (T data0, T data1, bool isEmpty) = array.NonEmptyDomain<T>(i);
            if (!isEmpty)
            {
                isEmptyDomain = false;
            }
            nonEmptyDomain.Add(dimName, new Tuple<T, T>(data0, data1));
        }
    }

    /// <summary>
    /// Gets the non-empty domain from a dimension of the <see cref="Array"/>, identified by its index.
    /// </summary>
    /// <typeparam name="T">The dimension's type.</typeparam>
    /// <param name="index">The dimension's index.</param>
    /// <returns>The domain's start and end values, along with whether it is empty.</returns>
    /// <exception cref="ArgumentException"><typeparamref name="T"/> is not the dimension's type.</exception>
    public (T Start, T End, bool IsEmpty) NonEmptyDomain<T>(uint index) where T : struct
    {
        using (var schema = Schema())
        using (var domain = schema.Domain())
        using (var dimension = domain.Dimension(index))
        {
            ErrorHandling.CheckDataType<T>(dimension.Type());
        }

        SequentialPair<T> data;
        int int_empty;
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_get_non_empty_domain_from_index(ctxHandle, handle, index,
            &data, &int_empty));

        return int_empty > 0 ? (default(T), default(T), true) : (data.First, data.Second, false);
    }

    /// <summary>
    /// Gets the non-empty domain from a dimension of the <see cref="Array"/>, identified by its name.
    /// </summary>
    /// <typeparam name="T">The dimension's type.</typeparam>
    /// <param name="name">The dimension's name.</param>
    /// <returns>The domain's start and end values, along with whether it is empty.</returns>
    /// <exception cref="ArgumentException"><typeparamref name="T"/> is not the dimension's type.</exception>
    public (T Start, T End, bool IsEmpty) NonEmptyDomain<T>(string name) where T : struct
    {
        using (var schema = Schema())
        using (var domain = schema.Domain())
        using (var dimension = domain.Dimension(name))
        {
            ErrorHandling.CheckDataType<T>(dimension.Type());
        }

        using var ms_name = new MarshaledString(name);
        SequentialPair<T> data;
        int int_empty;
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_get_non_empty_domain_from_name(ctxHandle, handle, ms_name, &data, &int_empty));

        return int_empty > 0 ? (default(T), default(T), true) : (data.First, data.Second, false);
    }

    /// <summary>
    /// Gets the non-empty domain from a variable-sized dimension of the <see cref="Array"/>, identified by its index.
    /// </summary>
    /// <param name="index">The dimension's index.</param>
    /// <returns>The domain's start and end values, along with whether it is empty.</returns>
    public (string Start, string End, bool IsEmpty) NonEmptyDomainVar(uint index)
    {
        ulong start_size64;
        ulong end_size64;
        int int_empty;
        using (var ctxHandle = _ctx.Handle.Acquire())
        using (var handle = _handle.Acquire())
        {
            _ctx.handle_error(Methods.tiledb_array_get_non_empty_domain_var_size_from_index(ctxHandle, handle, index, &start_size64, &end_size64, &int_empty));
        }
        if (int_empty > 0)
        {
            return (string.Empty, string.Empty, true);
        }

        int start_size = checked((int)start_size64);
        int end_size = checked((int)end_size64);

        using var start = new ScratchBuffer<byte>(start_size, stackalloc byte[128], exactSize: true);
        using var end = new ScratchBuffer<byte>(end_size, stackalloc byte[128], exactSize: true);

        using (var ctxHandle = _ctx.Handle.Acquire())
        using (var handle = _handle.Acquire())
        {
            fixed (byte* startPtr = start, endPtr = end)
                _ctx.handle_error(Methods.tiledb_array_get_non_empty_domain_var_from_index(ctxHandle, handle, index,
                    startPtr, endPtr, &int_empty));
        }

        return (MarshaledStringOut.GetString(start.Span), MarshaledStringOut.GetString(end.Span), false);
    }

    /// <summary>
    /// Gets the non-empty domain from a variable-sized dimension of the <see cref="Array"/>, identified by its name.
    /// </summary>
    /// <param name="name">The dimension's name.</param>
    /// <returns>The domain's start and end values, along with whether it is empty.</returns>
    public (string Start, string End, bool IsEmpty) NonEmptyDomainVar(string name)
    {
        ulong start_size64;
        ulong end_size64;
        int int_empty;
        using var ms_name = new MarshaledString(name);

        using (var ctxHandle = _ctx.Handle.Acquire())
        using (var handle = _handle.Acquire())
        {
            _ctx.handle_error(Methods.tiledb_array_get_non_empty_domain_var_size_from_name(ctxHandle, handle, ms_name, &start_size64, &end_size64, &int_empty));
        }

        if (int_empty > 0)
        {
            return (string.Empty, string.Empty, true);
        }

        int start_size = checked((int)start_size64);
        int end_size = checked((int)end_size64);

        using var start = new ScratchBuffer<byte>(start_size, stackalloc byte[128], exactSize: true);
        using var end = new ScratchBuffer<byte>(end_size, stackalloc byte[128], exactSize: true);

        using (var ctxHandle = _ctx.Handle.Acquire())
        using (var handle = _handle.Acquire())
        {
            fixed (byte* startPtr = start, endPtr = end)
                _ctx.handle_error(Methods.tiledb_array_get_non_empty_domain_var_from_name(ctxHandle, handle, ms_name,
                    startPtr, endPtr, &int_empty));
        }

        return new(MarshaledStringOut.GetString(start.Span), MarshaledStringOut.GetString(end.Span), false);
    }

    /// <summary>
    /// Gets the array's URI.
    /// </summary>
    public string Uri()
    {
        sbyte* result;
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_get_uri(ctxHandle, handle, &result));

        return MarshaledStringOut.GetStringFromNullTerminated(result);
    }

    /// <summary>
    /// Gets the <see cref="CSharp.EncryptionType"/> with which an array was encrypted.
    /// </summary>
    /// <param name="ctx">The <see cref="CSharp.Context"/> to use.</param>
    /// <param name="uri">The array's URI.</param>
    public static EncryptionType EncryptionType(Context ctx, string uri)
    {
        using var ms_uri = new MarshaledString(uri);
        tiledb_encryption_type_t tiledb_encryption_type;
        using var ctxHandle = ctx.Handle.Acquire();
        ctx.handle_error(Methods.tiledb_array_encryption_type(ctxHandle, ms_uri, &tiledb_encryption_type));
        return (EncryptionType)tiledb_encryption_type;
    }

    /// <summary>
    /// Gets an <see cref="Enumeration"/> from the <see cref="Array"/> by name.
    /// </summary>
    /// <param name="name">The enumeration's name.</param>
    /// <seealso cref="LoadAllEnumerations"/>
    public Enumeration GetEnumeration(string name)
    {
        var handle = new EnumerationHandle();
        var successful = false;
        tiledb_enumeration_t* enumeration_p = null;
        try
        {
            using (var ctxHandle = _ctx.Handle.Acquire())
            using (var arrayHandle = _handle.Acquire())
            using (var ms_name = new MarshaledString(name))
            {
                _ctx.handle_error(Methods.tiledb_array_get_enumeration(ctxHandle, arrayHandle, ms_name, &enumeration_p));
            }
            successful = true;
        }
        finally
        {
            if (successful)
            {
                handle.InitHandle(enumeration_p);
            }
            else
            {
                handle.SetHandleAsInvalid();
            }
        }

        return new Enumeration(_ctx, handle);
    }

    /// <summary>
    /// Loads all enumerations of the <see cref="Array"/>.
    /// </summary>
    /// <seealso cref="GetEnumeration"/>
    public void LoadAllEnumerations()
    {
        using (var ctxHandle = _ctx.Handle.Acquire())
        using (var arrayHandle = _handle.Acquire())
        {
            _ctx.handle_error(Methods.tiledb_array_load_all_enumerations(ctxHandle, arrayHandle));
        }
    }

    /// <summary>
    /// Puts a multi-value metadata to the <see cref="Array"/>.
    /// </summary>
    /// <typeparam name="T">The metadata's type.</typeparam>
    /// <param name="key">THe metadata's key.</param>
    /// <param name="data">The metadata's value.</param>
    public void PutMetadata<T>(string key, T[] data) where T : struct
    {
        _metadata.PutMetadata<T>(key, data);
    }

    /// <summary>
    /// Puts a single-value metadata to the <see cref="Array"/>.
    /// </summary>
    /// <typeparam name="T">The metadata's type.</typeparam>
    /// <param name="key">THe metadata's key.</param>
    /// <param name="v">The metadata's value.</param>
    public void PutMetadata<T>(string key, T v) where T : struct
    {
        _metadata.PutMetadata<T>(key, v);
    }

    /// <summary>
    /// Puts a string-typed metadata to the <see cref="Array"/>.
    /// </summary>
    /// <param name="key">THe metadata's key.</param>
    /// <param name="value">The metadata's value.</param>
    public void PutMetadata(string key, string value)
    {
        _metadata.PutMetadata(key, value);
    }

    /// <summary>
    /// Deletes a metadata from the <see cref="Array"/>.
    /// </summary>
    /// <param name="key">The metadata's ket.</param>
    public void DeleteMetadata(string key)
    {
        _metadata.DeleteMetadata(key);
    }

    /// <summary>
    /// Deletes fragments from an array that fall within the given timestamp range.
    /// </summary>
    /// <param name="ctx">The <see cref="Context"/> to use.</param>
    /// <param name="uri">The URI to the array.</param>
    /// <param name="timestampStart">The start of the timestamp range, inclusive.</param>
    /// <param name="timestampEnd">The end of the timestamp range, inclusive.</param>
    public static void DeleteFragments(Context ctx, string uri, ulong timestampStart, ulong timestampEnd)
    {
        using var ms_uri = new MarshaledString(uri);
        using var ctxHandle = ctx.Handle.Acquire();
        ctx.handle_error(Methods.tiledb_array_delete_fragments_v2(ctxHandle, ms_uri, timestampStart, timestampEnd));
    }

    /// <summary>
    /// Deletes fragments from an array that fall within the given timestamp range.
    /// </summary>
    /// <param name="ctx">The <see cref="Context"/> to use.</param>
    /// <param name="uri">The URI to the array.</param>
    /// <param name="fragmentUris">A list with the URIs of the fragments to delete.</param>
    public static void DeleteFragments(Context ctx, string uri, IReadOnlyList<string> fragmentUris)
    {
        using var ms_uri = new MarshaledString(uri);
        using var msc_fragmentUris = new MarshaledStringCollection(fragmentUris);
        using var ctxHandle = ctx.Handle.Acquire();
        ctx.handle_error(Methods.tiledb_array_delete_fragments_list(ctxHandle, ms_uri, (sbyte**)msc_fragmentUris.Strings, (nuint)msc_fragmentUris.Count));
    }

    /// <summary>
    /// Gets metadata from the <see cref="Array"/>.
    /// </summary>
    /// <typeparam name="T">The metadata's type.</typeparam>
    /// <param name="key">The metadata's key.</param>
    /// <returns>An array with the metadata values.</returns>
    public T[] GetMetadata<T>(string key) where T : struct
    {
        return _metadata.GetMetadata<T>(key);
    }

    /// <summary>
    /// Gets string-typed metadata from the <see cref="Array"/>.
    /// </summary>
    /// <param name="key">The metadata's key.</param>
    public string GetMetadata(string key)
    {
        return _metadata.GetMetadata(key);
    }

    /// <summary>
    /// Gets the number of metadata of the <see cref="Array"/>.
    /// </summary>
    public ulong MetadataNum()
    {
        return _metadata.MetadataNum();
    }

    /// <summary>
    /// Gets the <see cref="Array"/>'s metadata key and value by index.
    /// </summary>
    /// <typeparam name="T">The metadata's type.</typeparam>
    /// <param name="index">The metadata's index. Must be between zero and <see cref="MetadataNum"/> minus one.</param>
    /// <returns>A tuple with the metadata's key and value.</returns>
    public (string key, T[] data) GetMetadataFromIndex<T>(ulong index) where T : struct
    {
        return _metadata.GetMetadataFromIndex<T>(index);
    }

    /// <summary>
    /// Gets the <see cref="Array"/>'s metadata keys.
    /// </summary>
    public string[] MetadataKeys()
    {
        return _metadata.MetadataKeys();
    }

    /// <summary>
    /// Checks whether a metadata with a given key exists in the <see cref="Array"/>.
    /// </summary>
    /// <param name="key">The metadata's key</param>
    /// <returns>A tuple of whether the metadata exists, and its type.</returns>
    public (bool has_key, DataType datatype) HasMetadata(string key)
    {
        return _metadata.HasMetadata(key);
    }

    /// <summary>
    /// Loads the <see cref="ArraySchema"/> of an array at the given path.
    /// </summary>
    /// <param name="ctx">The <see cref="CSharp.Context"/> to use.</param>
    /// <param name="path">The array's path.</param>
    public static ArraySchema LoadArraySchema(Context ctx, string path) => ArraySchema.Load(ctx, path);
}

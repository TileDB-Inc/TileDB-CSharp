using System;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp;

public sealed unsafe class Group : IDisposable
{
    private readonly GroupHandle _handle;
    private readonly Context _ctx;
    private bool _disposed;

    private readonly GroupMetadata _metadata;

    public Group(Context ctx, string uri)
    {
        _ctx = ctx;
        using var ms_uri = new MarshaledString(uri);
        _handle = GroupHandle.Create(_ctx, ms_uri);
        _disposed = false;
        _metadata = new GroupMetadata(this);
    }

    internal Group(Context ctx, GroupHandle handle)
    {
        _ctx = ctx;
        _handle = handle;
        _disposed = false;
        _metadata = new GroupMetadata(this);
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

    internal GroupHandle Handle => _handle;

    /// <summary>
    /// Get context.
    /// </summary>
    /// <returns></returns>
    public Context Context()
    {
        return _ctx;
    }

    #region capi functions
    public static void Create(Context ctx, string uri)
    {
        using var ctxHandle = ctx.Handle.Acquire();
        using var ms_uri = new MarshaledString(uri);
        ctx.handle_error(Methods.tiledb_group_create(ctxHandle, ms_uri));
    }

    /// <summary>
    /// Open the group.
    /// </summary>
    /// <param name="queryType"></param>
    public void Open(QueryType queryType)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        var tiledb_query_type = (tiledb_query_type_t)queryType;
        _ctx.handle_error(Methods.tiledb_group_open(ctxHandle, handle, tiledb_query_type));
    }

    /// <summary>
    /// Close the group.
    /// </summary>
    public void Close()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_group_close(ctxHandle, handle));
    }

    /// <summary>
    /// Set config.
    /// </summary>
    /// <param name="config"></param>
    public void SetConfig(Config config)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var configHandle = config.Handle.Acquire();
        _ctx.handle_error(Methods.tiledb_group_set_config(ctxHandle, handle, configHandle));
    }

    /// <summary>
    /// Get config.
    /// </summary>
    /// <returns></returns>
    public Config Config()
    {
        var handle = new ConfigHandle();
        tiledb_config_t* config = null;
        var successful = false;
        try
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var groupHandle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_group_get_config(ctxHandle, groupHandle, &config));
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
    /// Put metadata array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="data"></param>
    public void PutMetadata<T>(string key, T[] data) where T : struct
    {
        _metadata.PutMetadata<T>(key, data);
    }

    /// <summary>
    /// Put a sigle value metadata.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="v"></param>
    public void PutMetadata<T>(string key, T v) where T : struct
    {
        _metadata.PutMetadata<T>(key, v);
    }

    /// <summary>
    /// Put string metadata.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void PutMetadata(string key, string value)
    {
        _metadata.PutMetadata(key, value);
    }

    /// <summary>
    /// Delete metadata.
    /// </summary>
    /// <param name="key"></param>
    public void DeleteMetadata(string key)
    {
        _metadata.DeleteMetadata(key);
    }

    /// <summary>
    /// Get metadata list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public T[] GetMetadata<T>(string key) where T : struct
    {
        return _metadata.GetMetadata<T>(key);
    }

    /// <summary>
    /// Get string metadata
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetMetadata(string key)
    {
        return _metadata.GetMetadata(key);
    }

    /// <summary>
    /// Get number of metadata.
    /// </summary>
    /// <returns></returns>
    public ulong MetadataNum()
    {
        return _metadata.MetadataNum();
    }

    /// <summary>
    /// Get metadata from index.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="index"></param>
    /// <returns></returns>
    public (string key, T[] data) GetMetadataFromIndex<T>(ulong index) where T : struct
    {
        return _metadata.GetMetadataFromIndex<T>(index);
    }

    /// <summary>
    /// Get metadata keys.
    /// </summary>
    /// <returns></returns>
    public string[] MetadataKeys()
    {
        return _metadata.MetadataKeys();
    }

    /// <summary>
    /// Test if a metadata with key exists or not.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public (bool has_key, DataType datatype) HasMetadata(string key)
    {
        return _metadata.HasMetadata(key);
    }

    /// <summary>
    /// Add a member to a group
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="relative"></param>
    public void AddMember(string uri, bool relative, string name)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var ms_uri = new MarshaledString(uri);
        using var ms_name = new MarshaledString(name);
        byte byte_relative = relative ? (byte)1 : (byte)0;
        _ctx.handle_error(Methods.tiledb_group_add_member(
            ctxHandle, handle, ms_uri, byte_relative, ms_name));
    }

    /// <summary>
    /// Remove a member of the group
    /// </summary>
    /// <param name="name">Name of member to remove. If the member has no name, this
    /// parameter should be set to the URI of the member.In that case, only the
    /// unnamed member with the given URI will be removed.</param>
    public void RemoveMember(string name)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var ms_name = new MarshaledString(name);
        _ctx.handle_error(Methods.tiledb_group_remove_member(ctxHandle, handle, ms_name));
    }

    /// <summary>
    /// Get count of members.
    /// </summary>
    /// <returns></returns>
    public ulong MemberCount()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        ulong num;
        _ctx.handle_error(Methods.tiledb_group_get_member_count(ctxHandle, handle, &num));
        return num;
    }

    /// <summary>
    /// Get member by index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    (string uri, ObjectType object_type, string name) MemberByIndex(ulong index)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        sbyte* uriPtr;
        sbyte* namePtr;
        tiledb_object_t tiledb_objecttype;
        _ctx.handle_error(Methods.tiledb_group_get_member_by_index(
            ctxHandle, handle, index, &uriPtr, &tiledb_objecttype, &namePtr));

        string uri = MarshaledStringOut.GetStringFromNullTerminated(uriPtr);
        string name = MarshaledStringOut.GetStringFromNullTerminated(namePtr);
        return (uri, (ObjectType)tiledb_objecttype, name);
    }

    /// <summary>
    /// Returns if this group is open or not.
    /// </summary>
    public bool IsOpen()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        int int_open;
        _ctx.handle_error(Methods.tiledb_group_is_open(ctxHandle, handle, &int_open));
        return int_open > 0;
    }

    /// <summary>
    /// Returns if the given URI is relative to this group.
    /// </summary>
    /// <param name="uri">The URI to test.</param>
    public bool IsUriRelative(string uri)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var ms_uri = new MarshaledString(uri);
        byte result;
        _ctx.handle_error(Methods.tiledb_group_get_is_relative_uri_by_name(ctxHandle, handle, ms_uri, &result));
        return result > 0;
    }

    /// <summary>
    /// Get uri.
    /// </summary>
    /// <returns></returns>
    public string Uri()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        sbyte* result;
        _ctx.handle_error(Methods.tiledb_group_get_uri(ctxHandle, handle, &result));

        return MarshaledStringOut.GetStringFromNullTerminated(result);
    }

    /// <summary>
    /// Get query type.
    /// </summary>
    /// <returns></returns>
    public QueryType QueryType()
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        tiledb_query_type_t tiledb_query_type;
        _ctx.handle_error(Methods.tiledb_group_get_query_type(ctxHandle, handle, &tiledb_query_type));
        return (QueryType)tiledb_query_type;
    }

    /// <summary>
    /// Dump to string
    /// </summary>
    /// <param name="recursive"></param>
    /// <returns></returns>
    public string Dump(bool recursive)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        sbyte* result;
        byte int_recursive = (byte)(recursive ? 1 : 0);
        _ctx.handle_error(Methods.tiledb_group_dump_str(ctxHandle, handle, &result, int_recursive));

        return MarshaledStringOut.GetStringFromNullTerminated(result);
    }
    #endregion
}

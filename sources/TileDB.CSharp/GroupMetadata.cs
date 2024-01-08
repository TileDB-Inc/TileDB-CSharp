using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using TileDB.Interop;

namespace TileDB.CSharp;

public unsafe class GroupMetadata : IDictionary<string, byte[]>
{
    protected Group _group;

    #region Constructors
    public GroupMetadata(Group group)
    {
        _group = group;
    }
    #endregion

    #region User api
    /// <summary>
    /// Put metadata array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="data"></param>
    public void PutMetadata<T>(string key, T[] data) where T : struct
    {
        var tiledb_datatype = (tiledb_datatype_t)EnumUtil.TypeToDataType(typeof(T));
        put_metadata(key, data, tiledb_datatype);
    }

    /// <summary>
    /// Put a single value metadata.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="v"></param>
    public void PutMetadata<T>(string key, T v) where T : struct
    {
        T[] data = [v];
        PutMetadata(key, data);
    }

    /// <summary>
    /// Put string metadata.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void PutMetadata(string key, string value)
    {
        var tiledb_datatype = tiledb_datatype_t.TILEDB_STRING_UTF8;
        var data = Encoding.UTF8.GetBytes(value);
        put_metadata(key, data, tiledb_datatype);
    }

    /// <summary>
    /// Delete metadata.
    /// </summary>
    /// <param name="key"></param>
    public void DeleteMetadata(string key)
    {
        var ctx = _group.Context();
        using var ctxHandle = ctx.Handle.Acquire();
        using var groupHandle = _group.Handle.Acquire();
        using var ms_key = new MarshaledString(key);
        ctx.handle_error(Methods.tiledb_group_delete_metadata(ctxHandle, groupHandle, ms_key));
    }

    /// <summary>
    /// Get metadata list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public T[] GetMetadata<T>(string key) where T : struct
    {
        var (k, value, dataType, valueNum) = get_metadata(key);
        Span<byte> valueSpan = value;

        var span = MemoryMarshal.Cast<byte, T>(valueSpan);
        return span.ToArray();
    }

    /// <summary>
    /// Get string metadata
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetMetadata(string key)
    {
        var (_, byte_array, datatype, _) = get_metadata(key);
        return MarshaledStringOut.GetString(byte_array, (DataType)datatype);
    }

    /// <summary>
    /// Get number of metadata.
    /// </summary>
    /// <returns></returns>
    public ulong MetadataNum()
    {
        var ctx = _group.Context();
        using var ctxHandle = ctx.Handle.Acquire();
        using var groupHandle = _group.Handle.Acquire();
        ulong num;
        ctx.handle_error(Methods.tiledb_group_get_metadata_num(ctxHandle, groupHandle, &num));
        return num;
    }

    /// <summary>
    /// Get metadata from index.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="index"></param>
    /// <returns></returns>
    public (string key, T[] data) GetMetadataFromIndex<T>(ulong index) where T : struct
    {
        var (key, value, dataType, valueNum) = get_metadata_from_index(index);
        Span<byte> valueSpan = value;

        var span = MemoryMarshal.Cast<byte, T>(valueSpan);
        return (key, span.ToArray());

    }

    /// <summary>
    /// Get metadata keys.
    /// </summary>
    /// <returns></returns>
    public string[] MetadataKeys()
    {
        var num = MetadataNum();
        var ret = new string[(int)num];
        for (ulong i = 0; i < num; ++i)
        {
            var (key, _, _, _) = get_metadata_from_index(i);
            ret[i] = key;
        }
        return ret;
    }

    /// <summary>
    /// Test if a metadata with key exists or not.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public (bool has_key, DataType datatype) HasMetadata(string key)
    {
        var ctx = _group.Context();
        using var ctxHandle = ctx.Handle.Acquire();
        using var groupHandle = _group.Handle.Acquire();
        using var ms_key = new MarshaledString(key);
        tiledb_datatype_t tiledb_datatype;
        var has_key = 0;
        ctx.handle_error(Methods.tiledb_group_has_metadata_key(ctxHandle, groupHandle, ms_key, &tiledb_datatype, &has_key));

        return (has_key > 0, (DataType)tiledb_datatype);
    }

    /// <summary>
    /// Consolidate metadata.
    /// </summary>
    /// <param name="ctx"></param>
    /// <param name="uri"></param>
    /// <param name="config"></param>
    public static void ConsolidateMetadata(Context ctx, string uri, Config config)
    {
        using var ctxHandle = ctx.Handle.Acquire();
        using var ms_uri = new MarshaledString(uri);
        using var configHandle = config.Handle.Acquire();
        ctx.handle_error(Methods.tiledb_group_consolidate_metadata(ctxHandle, ms_uri, configHandle));
    }
    #endregion

    #region IDictionary interface
    public byte[] this[string key]
    {
        get
        {
            var ctx = _group.Context();
            using var ctxHandle = ctx.Handle.Acquire();
            using var groupHandle = _group.Handle.Acquire();
            void* value_p;
            using var ms_key = new MarshaledString(key);
            tiledb_datatype_t datatype;

            int has_key = 0;
            ctx.handle_error(Methods.tiledb_group_has_metadata_key(ctxHandle, groupHandle, ms_key, &datatype, &has_key));
            if (has_key <= 0)
            {
                return new byte[0];
            }

            uint value_num = 0;
            ctx.handle_error(Methods.tiledb_group_get_metadata(ctxHandle, groupHandle, ms_key, &datatype, &value_num,
                &value_p));
            var size = (int)(value_num * Methods.tiledb_datatype_size(datatype));
            var fill_span = new ReadOnlySpan<byte>(value_p, size);
            return fill_span.ToArray();
        }
        set
        {
            Add(key, value);
        }
    }

    public ICollection<string> Keys
    {
        get
        {
            List<string> ret = new List<string>();
            var ctx = _group.Context();
            using var ctxHandle = ctx.Handle.Acquire();
            using var groupHandle = _group.Handle.Acquire();
            ulong num;
            ctx.handle_error(Methods.tiledb_group_get_metadata_num(ctxHandle, groupHandle, &num));

            for (ulong i = 0; i < num; ++i)
            {
                var metadata = get_metadata_from_index(i);
                ret.Add(metadata.key);
            }

            return ret;
        }

    }

    public ICollection<byte[]> Values
    {
        get
        {
            List<byte[]> ret = new List<byte[]>();
            ulong num = MetadataNum();
            for (ulong i = 0; i < num; ++i)
            {
                var metadata = get_metadata_from_index(i);
                ret.Add(metadata.data);
            }

            return ret;
        }
    }

    public int Count
    {
        get
        {
            ulong num = MetadataNum();
            return (int)num;
        }
    }

    public bool IsReadOnly
    {
        get
        {
            QueryType querytype = _group.QueryType();
            return querytype == QueryType.Read;
        }
    }

    public void Add(string key, byte[] value)
    {
        put_metadata(key, value, tiledb_datatype_t.TILEDB_UINT8);
    }

    public void Add(KeyValuePair<string, byte[]> item)
    {
        Add(item.Key, item.Value);
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool Contains(KeyValuePair<string, byte[]> item)
    {
        return ContainsKey(item.Key);
    }

    public bool ContainsKey(string key)
    {
        var (has_key, _) = HasMetadata(key);
        return has_key;
    }

    public void CopyTo(KeyValuePair<string, byte[]>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<KeyValuePair<string, byte[]>> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public bool Remove(string key)
    {
        DeleteMetadata(key);
        return true;
    }

    public bool Remove(KeyValuePair<string, byte[]> item)
    {
        return Remove(item.Key);
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out byte[] value)
    {
        if (!ContainsKey(key))
        {
            value = null;
            return false;
        }
        var metadata = get_metadata(key);
        value = metadata.data;
        return true;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
    #endregion

    #region Private methods
    private void put_metadata<T>(string key, T[] value, tiledb_datatype_t tiledb_datatype) where T : struct
    {
        ErrorHandling.CheckDataType<T>((DataType)tiledb_datatype);
        if (string.IsNullOrEmpty(key) || value.Length == 0)
        {
            throw new ArgumentException("ArrayMetadata.put_metadata, null or empty key-value!");
        }
        var ctx = _group.Context();
        using var ms_key = new MarshaledString(key);
        using var ctxHandle = ctx.Handle.Acquire();
        using var groupHandle = _group.Handle.Acquire();
        fixed (T* dataPtr = &value[0])
            ctx.handle_error(Methods.tiledb_group_put_metadata(ctxHandle, groupHandle, ms_key, tiledb_datatype, (uint)value.Length, dataPtr));
    }

    private (string key, byte[] data, tiledb_datatype_t tiledb_datatype, uint value_num) get_metadata(string key)
    {
        var ctx = _group.Context();
        using var ctxHandle = ctx.Handle.Acquire();
        using var groupHandle = _group.Handle.Acquire();
        void* value_p;
        using var ms_key = new MarshaledString(key);
        tiledb_datatype_t datatype;
        uint value_num = 0;
        ctx.handle_error(Methods.tiledb_group_get_metadata(ctxHandle, groupHandle, ms_key, &datatype, &value_num,
            &value_p));
        var size = (int)(value_num * Methods.tiledb_datatype_size(datatype));
        var fill_span = new ReadOnlySpan<byte>(value_p, size);
        return (key, fill_span.ToArray(), datatype, value_num);
    }

    private (string key, byte[] data, tiledb_datatype_t tiledb_datatype, uint value_num) get_metadata_from_index(ulong index)
    {
        var ctx = _group.Context();
        using var ctxHandle = ctx.Handle.Acquire();
        using var groupHandle = _group.Handle.Acquire();
        void* value_p;
        sbyte* keyPtr;
        tiledb_datatype_t dataType;
        uint valueNum = 0;
        uint key_len;
        ctx.handle_error(Methods.tiledb_group_get_metadata_from_index(ctxHandle, groupHandle, index, &keyPtr,
            &key_len, &dataType, &valueNum, &value_p));
        var size = (int)(valueNum * Methods.tiledb_datatype_size(dataType));
        var fill_span = new ReadOnlySpan<byte>(value_p, size);
        string key = MarshaledStringOut.GetStringFromNullTerminated(keyPtr);
        return (key, fill_span.ToArray(), dataType, valueNum);
    }
    #endregion
}

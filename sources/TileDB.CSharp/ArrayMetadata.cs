using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using TileDB.Interop;

namespace TileDB.CSharp;

/// <summary>
/// Provides a view of an <see cref="Array"/>'s metadata.
/// </summary>
public unsafe class ArrayMetadata : IDictionary<string, byte[]>
{
    /// <summary>
    /// The <see cref="Array"/> to get the metadata from.
    /// </summary>
    protected Array _array;

    /// <summary>
    /// Creates an <see cref="ArrayMetadata"/> object from an <see cref="Array"/>.
    /// </summary>
    /// <param name="array">The array to get the metadata from.</param>
    public ArrayMetadata(Array array)
    {
        _array = array;
    }

    /// <inheritdoc cref="Array.PutMetadata{T}(string, T[])"/>
    public void PutMetadata<T>(string key, T[] data) where T : struct
    {
        var tiledb_datatype = (tiledb_datatype_t)EnumUtil.TypeToDataType(typeof(T));
        put_metadata(key, data, tiledb_datatype);
    }

    /// <inheritdoc cref="Array.PutMetadata{T}(string, T)"/>
    public void PutMetadata<T>(string key, T v) where T : struct
    {
        T[] data = [v];
        PutMetadata<T>(key, data);
    }

    /// <inheritdoc cref="Array.PutMetadata(string, string)"/>
    public void PutMetadata(string key, string value)
    {
        var data = Encoding.UTF8.GetBytes(value);
        put_metadata(key, data, tiledb_datatype_t.TILEDB_STRING_UTF8);
    }

    /// <inheritdoc cref="Array.DeleteMetadata"/>
    public void DeleteMetadata(string key)
    {
        var ctx = _array.Context();
        using var ctxHandle = ctx.Handle.Acquire();
        using var arrayHandle = _array.Handle.Acquire();
        using var ms_key = new MarshaledString(key);
        ctx.handle_error(Methods.tiledb_array_delete_metadata(ctxHandle, arrayHandle, ms_key));
    }

    /// <inheritdoc cref="Array.GetMetadata{T}(string)"/>
    public T[] GetMetadata<T>(string key) where T : struct
    {
        var (k, value, dataType, valueNum) = get_metadata(key);
        Span<byte> valueSpan = value;

        var span = MemoryMarshal.Cast<byte, T>(valueSpan);
        return span.ToArray();
    }

    /// <inheritdoc cref="Array.GetMetadata(string)"/>
    public string GetMetadata(string key)
    {
        var (_, byte_array, datatype, _) = get_metadata(key);
        return MarshaledStringOut.GetString(byte_array, (DataType)datatype);
    }

    /// <inheritdoc cref="Array.MetadataNum"/>
    public ulong MetadataNum()
    {
        var ctx = _array.Context();
        ulong num;
        using var ctxHandle = ctx.Handle.Acquire();
        using var arrayHandle = _array.Handle.Acquire();
        ctx.handle_error(Methods.tiledb_array_get_metadata_num(ctxHandle, arrayHandle, &num));
        return num;
    }

    /// <inheritdoc cref="Array.GetMetadataFromIndex"/>
    public (string key, T[] data) GetMetadataFromIndex<T>(ulong index) where T : struct
    {
        var (key, value, dataType, valueNum) = get_metadata_from_index(index);
        Span<byte> valueSpan = value;

        var span = MemoryMarshal.Cast<byte, T>(valueSpan);
        return (key, span.ToArray());

    }

    /// <inheritdoc cref="Array.MetadataKeys"/>
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

    /// <inheritdoc cref="Array.HasMetadata"/>
    public (bool has_key, DataType datatype) HasMetadata(string key)
    {
        var ctx = _array.Context();
        using var ctxHandle = ctx.Handle.Acquire();
        using var arrayHandle = _array.Handle.Acquire();
        using var ms_key = new MarshaledString(key);
        tiledb_datatype_t tiledb_datatype;
        var has_key = 0;
        ctx.handle_error(Methods.tiledb_array_has_metadata_key(ctxHandle, arrayHandle, ms_key, &tiledb_datatype, &has_key));

        return (has_key > 0, (DataType)tiledb_datatype);
    }

    /// <inheritdoc cref="P:System.Collections.Generic.IDictionary`2.Item(`0)"/>
    public byte[] this[string key]
    {
        get
        {
            var ctx = _array.Context();
            void* value_p;
            using var ms_key = new MarshaledString(key);
            tiledb_datatype_t datatype;

            int has_key = 0;

            using (var ctxHandle = ctx.Handle.Acquire())
            using (var arrayHandle = _array.Handle.Acquire())
            {
                ctx.handle_error(Methods.tiledb_array_has_metadata_key(ctxHandle, arrayHandle, ms_key, &datatype, &has_key));
            }
            if (has_key <= 0)
            {

                return new byte[0];
            }

            uint value_num = 0;
            using (var ctxHandle = ctx.Handle.Acquire())
            using (var arrayHandle = _array.Handle.Acquire())
            {
                ctx.handle_error(Methods.tiledb_array_get_metadata(ctxHandle, arrayHandle, ms_key, &datatype, &value_num, &value_p));
            }
            var size = (int)(value_num * Methods.tiledb_datatype_size(datatype));
            var fill_span = new ReadOnlySpan<byte>(value_p, size);
            return fill_span.ToArray();
        }
        set
        {
            Add(key, value);
        }
    }

    /// <inheritdoc cref="IDictionary{TKey, TValue}.Keys"/>
    public ICollection<string> Keys
    {
        get
        {
            List<string> ret = new List<string>();
            var ctx = _array.Context();
            ulong num;

            using (var ctxHandle = ctx.Handle.Acquire())
            using (var arrayHandle = _array.Handle.Acquire())
            {
                ctx.handle_error(Methods.tiledb_array_get_metadata_num(ctxHandle, arrayHandle, &num));
            }

            for (ulong i = 0; i < num; ++i)
            {
                var metadata = get_metadata_from_index(i);
                ret.Add(metadata.key);
            }
            return ret;
        }
    }

    /// <inheritdoc cref="IDictionary{TKey, TValue}.Values"/>
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

    /// <inheritdoc cref="ICollection{T}.Count"/>
    public int Count
    {
        get
        {
            ulong num = MetadataNum();
            return (int)num;
        }
    }

    /// <inheritdoc cref="ICollection{T}.IsReadOnly"/>
    public bool IsReadOnly
    {
        get
        {
            QueryType querytype = _array.QueryType();
            return querytype == QueryType.Read;
        }
    }

    /// <inheritdoc cref="IDictionary{TKey, TValue}.Add"/>
    public void Add(string key, byte[] value)
    {
        put_metadata(key, value, tiledb_datatype_t.TILEDB_UINT8);
    }

    /// <inheritdoc cref="ICollection{T}.Add"/>
    public void Add(KeyValuePair<string, byte[]> item)
    {
        Add(item.Key, item.Value);
    }

    /// <inheritdoc cref="ICollection{T}.IsReadOnly"/>
    public void Clear()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ICollection{T}.IsReadOnly"/>
    public bool Contains(KeyValuePair<string, byte[]> item)
    {
        return ContainsKey(item.Key);
    }

    /// <inheritdoc cref="ICollection{T}.IsReadOnly"/>
    public bool ContainsKey(string key)
    {
        var (has_key, _) = HasMetadata(key);
        return has_key;
    }

    /// <inheritdoc cref="ICollection{T}.CopyTo"/>
    public void CopyTo(KeyValuePair<string, byte[]>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public IEnumerator<KeyValuePair<string, byte[]>> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IDictionary{TKey, TValue}.Remove"/>
    public bool Remove(string key)
    {
        DeleteMetadata(key);
        return true;
    }

    /// <inheritdoc cref="ICollection{T}.Remove"/>
    public bool Remove(KeyValuePair<string, byte[]> item)
    {
        return Remove(item.Key);
    }

    /// <inheritdoc cref="IDictionary{TKey, TValue}.TryGetValue"/>
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

    private void put_metadata<T>(string key, T[] value, tiledb_datatype_t tiledb_datatype) where T : struct
    {
        ErrorHandling.CheckDataType<T>((DataType)tiledb_datatype);
        if (string.IsNullOrEmpty(key) || value.Length == 0)
        {
            throw new ArgumentException("ArrayMetadata.put_metadata, null or empty key-value!");
        }
        var ctx = _array.Context();
        using var ms_key = new MarshaledString(key);
        using var ctxHandle = ctx.Handle.Acquire();
        using var arrayHandle = _array.Handle.Acquire();
        fixed (T* dataPtr = &value[0])
            ctx.handle_error(Methods.tiledb_array_put_metadata(ctxHandle, arrayHandle, ms_key, tiledb_datatype, (uint)value.Length, dataPtr));
    }

    private (string key, byte[] data, tiledb_datatype_t tiledb_datatype, uint value_num) get_metadata(string key)
    {
        var ctx = _array.Context();
        void* value_p;
        using var ms_key = new MarshaledString(key);
        tiledb_datatype_t datatype;
        uint value_num = 0;
        using (var ctxHandle = ctx.Handle.Acquire())
        using (var arrayHandle = _array.Handle.Acquire())
        {
            ctx.handle_error(Methods.tiledb_array_get_metadata(ctxHandle, arrayHandle, ms_key, &datatype, &value_num, &value_p));
        }
        var size = (int)(value_num * Methods.tiledb_datatype_size(datatype));
        var fill_span = new ReadOnlySpan<byte>(value_p, size);
        return (key, fill_span.ToArray(), datatype, value_num);
    }

    private (string key, byte[] data, tiledb_datatype_t tiledb_datatype, uint value_num) get_metadata_from_index(ulong index)
    {
        var ctx = _array.Context();
        void* value_p;
        sbyte* key;
        tiledb_datatype_t dataType;
        uint valueNum = 0;
        uint key_len;
        using var ctxHandle = ctx.Handle.Acquire();
        using var arrayHandle = _array.Handle.Acquire();
        ctx.handle_error(Methods.tiledb_array_get_metadata_from_index(ctxHandle, arrayHandle, index, &key, &key_len, &dataType, &valueNum, &value_p));
        var size = (int)(valueNum * Methods.tiledb_datatype_size(dataType));
        var fill_span = new ReadOnlySpan<byte>(value_p, size);
        return (MarshaledStringOut.GetStringFromNullTerminated(key), fill_span.ToArray(), dataType, valueNum);
    }
}

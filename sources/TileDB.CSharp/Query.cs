using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp
{
    /// <summary>
    /// Contains estimates for the result's size.
    /// </summary>
    public class ResultSize
    {
        /// <summary>
        /// The data size in bytes.
        /// </summary>
        public ulong DataBytesSize;

        /// <summary>
        /// The offsets size in bytes.
        /// </summary>
        public ulong? OffsetsBytesSize;

        /// <summary>
        /// The validities size in bytes.
        /// </summary>
        public ulong? ValidityBytesSize;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dataSize"></param>
        /// <param name="offsetsSize"></param>
        /// <param name="validitySize"></param>
        public ResultSize(ulong dataSize, ulong? offsetsSize = null, ulong? validitySize = null)
        {
            DataBytesSize = dataSize;
            OffsetsBytesSize = offsetsSize;
            ValidityBytesSize = validitySize;
        }

        /// <summary>
        /// Returns if the result is about a variable-length attribute or dimension.
        /// </summary>
        public bool IsVarSize() => OffsetsBytesSize.HasValue;

        /// <summary>
        /// Returns if the result is about a nullable attribute.
        /// </summary>
        public bool IsNullable() => ValidityBytesSize.HasValue;

        /// <summary>
        /// Gets the number of data elements.
        /// </summary>
        /// <param name="dataType">The data type of the attribute or dimension.</param>
        public ulong DataSize(DataType dataType) =>
            DataBytesSize / Methods.tiledb_datatype_size((tiledb_datatype_t)dataType);

        /// <summary>
        /// Gets the number of offsets.
        /// </summary>
        public ulong OffsetsSize() => OffsetsBytesSize is ulong size ? size / sizeof(ulong) : 0;

        /// <summary>
        /// Gets the number of validities.
        /// </summary>
        public ulong ValiditySize() => ValidityBytesSize.GetValueOrDefault();
    }

    /// <summary>
    /// Used in <see cref="Query.SubmitAsync"/>.
    /// </summary>
    [Obsolete(Obsoletions.QuerySubmitAsyncMessage, DiagnosticId = Obsoletions.QuerySubmitAsyncDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
    public class QueryEventArgs : EventArgs
    {
        public QueryEventArgs(int status, string message)
        {
            Status = status;
            Message = message;
        }

        public int Status { get; set; }

        public string Message { get; set; }
    }

    /// <summary>
    /// Represents a TileDB query object.
    /// </summary>
    public sealed unsafe class Query : IDisposable
    {
        private readonly Array _array;
        private readonly Context _ctx;
        private readonly QueryHandle _handle;
        private bool _disposed;

        private readonly Dictionary<string, BufferHandle> _dataBufferHandles = new Dictionary<string, BufferHandle>();
        private readonly Dictionary<string, BufferHandle> _offsetsBufferHandles = new Dictionary<string, BufferHandle>();
        private readonly Dictionary<string, BufferHandle> _validityBufferHandles = new Dictionary<string, BufferHandle>();

        /// <summary>
        /// Creates a <see cref="Query"/>.
        /// </summary>
        /// <param name="ctx">The context associated with this query.</param>
        /// <param name="array">The array on which the query will operate.</param>
        /// <param name="queryType">The query's type.</param>
        public Query(Context ctx, Array array, QueryType queryType)
        {
            _ctx = ctx;
            _array = array;
            _handle = QueryHandle.Create(ctx, array.Handle, (tiledb_query_type_t)queryType);
        }

        /// <summary>
        /// Creates a <see cref="Query"/> with an implicit <see cref="CSharp.QueryType"/>.
        /// </summary>
        /// <param name="ctx">The context associated with this query.</param>
        /// <param name="array">The array on which the query will operate. Its <see cref="Array.QueryType"/> will be used for the query.</param>
        public Query(Context ctx, Array array)
        {
            _ctx = ctx;
            _array = array;
            _handle = QueryHandle.Create(ctx, array.Handle, (tiledb_query_type_t)array.QueryType());
        }

        /// <summary>
        /// Disposes this <see cref="Query"/>.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            _handle.Dispose();
            FreeAllBufferHandles();
            _disposed = true;
        }

        internal QueryHandle Handle => _handle;

        /// <summary>
        /// Gets a JSON string with statistics about the query.
        /// </summary>
        public string Stats()
        {
            sbyte* result = null;
            try
            {
                using var ctxHandle = _ctx.Handle.Acquire();
                using var handle = _handle.Acquire();
                ErrorHandling.ThrowOnError(Methods.tiledb_query_get_stats(ctxHandle, handle, &result));

                return MarshaledStringOut.GetStringFromNullTerminated(result);
            }
            finally
            {
                if (result is not null)
                {
                    ErrorHandling.ThrowOnError(Methods.tiledb_stats_free_str(&result));
                }
            }
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
            _ctx.handle_error(Methods.tiledb_query_set_config(ctxHandle, handle, configHandle));
        }

        /// <summary>
        /// Get config.
        /// </summary>
        /// <returns></returns>
        public Config Config()
        {
            var handle = new ConfigHandle();
            var successful = false;
            tiledb_config_t* config = null;
            try
            {
                using var ctxHandle = _ctx.Handle.Acquire();
                using var queryHandle = _handle.Acquire();
                _ctx.handle_error(Methods.tiledb_query_get_config(ctxHandle, queryHandle, &config));
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
        /// Indicates that the query will write to or read from a <see cref="Subarray"/>, and provides the appropriate information.
        /// </summary>
        /// <param name="subarray">The subarray to use.</param>
        public void SetSubarray(Subarray subarray)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var subarrayHandle = subarray.Handle.Acquire();
            _ctx.handle_error(Methods.tiledb_query_set_subarray_t(ctxHandle, handle, subarrayHandle));
        }

        /// <summary>
        /// Creates a <see cref="Subarray"/> corresponding to this query.
        /// </summary>
        public Subarray GetSubarray()
        {
            var handle = new SubarrayHandle();
            var successful = false;
            tiledb_subarray_t* subarray = null;
            try
            {
                using (var ctxHandle = _ctx.Handle.Acquire())
                using (var queryHandle = _handle.Acquire())
                {
                    _ctx.handle_error(Methods.tiledb_query_get_subarray_t(ctxHandle, queryHandle, &subarray));
                }
                successful = true;
            }
            finally
            {
                if (successful)
                {
                    handle.InitHandle(subarray);
                }
                else
                {
                    handle.SetHandleAsInvalid();
                }
            }

            return new Subarray(_ctx, _array, handle);
        }

        /// <summary>
        /// Deprecated, use <see cref="Subarray.SetSubarray{T}(T[])"/> instead and assign the subarray with <see cref="SetSubarray(Subarray)"/>.
        /// </summary>
        [Obsolete(Obsoletions.QuerySubarrayMessage + " Use Subarray.SetSubarray on the query's assigned Subarray instead.", DiagnosticId = Obsoletions.QuerySubarrayDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetSubarray<T>(T[] data) where T : struct
        {
            var dim_datatype = _array.Schema().Domain().Type();
            if (EnumUtil.TypeToDataType(typeof(T)) != dim_datatype)
            {
                throw new ArgumentException("Query.SetSubarray, datatype mismatch!");
            }

            var expected_size = _array.Schema().Domain().NDim() * 2;
            if (data == null || expected_size != data.Length)
            {
                throw new ArgumentException("Query.SetSubarray, the length of data is not equal to num_dims*2!");
            }

            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            var dataGcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                _ctx.handle_error(Methods.tiledb_query_set_subarray(ctxHandle, handle,
                    (void*)dataGcHandle.AddrOfPinnedObject()));
            }
            finally
            {
                dataGcHandle.Free();
            }
        }

        /// <summary>
        /// Sets the data buffer for an attribute or dimension to an array.
        /// </summary>
        /// <typeparam name="T">The buffer's type.</typeparam>
        /// <param name="name">The name of the attribute or the dimension.</param>
        /// <param name="data">An array where the data will be read or written.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="data"/> is empty or <typeparamref name="T"/>
        /// does not match the excepted data type.</exception>
        public void SetDataBuffer<T>(string name, T[] data) where T : struct
        {
            if (data is null)
            {
                ThrowHelpers.ThrowArgumentNullException(nameof(data));
            }

            SetDataBuffer(name, data.AsMemory());
        }

        /// <summary>
        /// Sets the data buffer for an attribute or dimension to a <see cref="Memory{T}"/>.
        /// </summary>
        /// <typeparam name="T">The buffer's type.</typeparam>
        /// <param name="name">The name of the attribute or the dimension.</param>
        /// <param name="data">An <see cref="Memory{T}"/> where the data will be read or written.</param>
        /// <exception cref="ArgumentException"><paramref name="data"/> is empty or <typeparamref name="T"/>
        /// does not match the excepted data type.</exception>
        public void SetDataBuffer<T>(string name, Memory<T> data) where T : struct
        {
            // check datatype
            using (var schema = _array.Schema())
            using (var domain = schema.Domain())
            {
                CheckDataType<T>(GetDataType(name, schema, domain));
            }

            if (data.IsEmpty)
            {
                ThrowHelpers.ThrowBufferCannotBeEmpty(nameof(data));
            }

            UnsafeSetDataBuffer(name, data.Pin(), (ulong)data.Length * (ulong)sizeof(T), sizeof(T));
        }

        /// <summary>
        /// Sets the data buffer for an attribute or dimension to an unmanaged memory buffer.
        /// </summary>
        /// <param name="name">The name of the attribute or the dimension.</param>
        /// <param name="data">A pointer to the memory buffer.</param>
        /// <param name="size">The buffer's size <em>in <typeparamref name="T"/> values</em>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="size"/> is zero or <typeparamref name="T"/>
        /// does not match the excepted data type.</exception>
        public void SetDataBuffer<T>(string name, T* data, ulong size) where T : struct
        {
            if (data is null)
            {
                ThrowHelpers.ThrowArgumentNullException(nameof(data));
            }

            if (size == 0)
            {
                ThrowHelpers.ThrowBufferCannotBeEmpty(nameof(size));
            }

            // check datatype
            using (var schema = _array.Schema())
            using (var domain = schema.Domain())
            {
                CheckDataType<T>(GetDataType(name, schema, domain));
            }

            UnsafeSetDataBuffer(name, new MemoryHandle(data), size * (ulong)sizeof(T), sizeof(T));
        }

        /// <summary>
        /// Sets the data buffer for an attribute or dimension to a <see cref="ReadOnlyMemory{T}"/>.
        /// Not supported for <see cref="QueryType.Read"/> queries.
        /// </summary>
        /// <typeparam name="T">The buffer's type.</typeparam>
        /// <param name="name">The name of the attribute or the dimension.</param>
        /// <param name="data">A <see cref="ReadOnlyMemory{T}"/> from where the data will be written.</param>
        /// <exception cref="ArgumentException"><paramref name="data"/> is empty or <typeparamref name="T"/>
        /// does not match the excepted data type.</exception>
        /// <exception cref="NotSupportedException">The query's type is <see cref="QueryType.Read"/></exception>
        public void SetDataReadOnlyBuffer<T>(string name, ReadOnlyMemory<T> data) where T : struct
        {
            if (QueryType() == CSharp.QueryType.Read)
            {
                ThrowHelpers.ThrowOperationNotAllowedOnReadQueries();
            }

            SetDataBuffer(name, MemoryMarshal.AsMemory(data));
        }

        /// <summary>
        /// Sets the data buffer for an attribute or dimension to an
        /// unmanaged memory buffer without performing type validation.
        /// </summary>
        /// <param name="name">The name of the attribute or the dimension.</param>
        /// <param name="data">A pointer to the memory buffer.</param>
        /// <param name="byteSize">The buffer's size <em>in bytes</em>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="byteSize"/> is zero.</exception>
        public void UnsafeSetDataBuffer(string name, void* data, ulong byteSize)
        {
            if (data is null)
            {
                ThrowHelpers.ThrowArgumentNullException(nameof(data));
            }

            if (byteSize == 0)
            {
                ThrowHelpers.ThrowBufferCannotBeEmpty(nameof(byteSize));
            }

            UnsafeSetDataBuffer(name, new MemoryHandle(data), byteSize);
        }

        /// <summary>
        /// Sets the data buffer for an attribute or dimension
        /// to a pinned memory buffer pointed by a <see cref="MemoryHandle"/>.
        /// This method does not perform type validation.
        /// </summary>
        /// <param name="name">The name of the attribute or the dimension.</param>
        /// <param name="memoryHandle">A <see cref="MemoryHandle"/> pointing to the buffer.</param>
        /// <param name="byteSize">The buffer's size <em>in bytes</em>.</param>
        /// <exception cref="ArgumentException"><paramref name="byteSize"/> is zero.</exception>
        /// <remarks>
        /// <para>After calling this method, user code must not use <paramref name="memoryHandle"/>;
        /// its ownership is transferred to this <see cref="Query"/> object.</para>
        /// <para><paramref name="memoryHandle"/> will be disposed when one of the following happens:
        /// <list type="bullet">
        /// <item>The <see cref="Dispose"/> method is called.</item>
        /// <item>The buffer for <paramref name="name"/> is reassigned through subsequent calls to this method.</item>
        /// <item>This method call throws an exception.</item>
        /// </list></para>
        /// </remarks>
        public void UnsafeSetDataBuffer(string name, MemoryHandle memoryHandle, ulong byteSize) =>
            UnsafeSetDataBuffer(name, memoryHandle, byteSize, 0);

        private void UnsafeSetDataBuffer(string name, MemoryHandle memoryHandle, ulong byteSize, int elementSize)
        {
            BufferHandle? handle = null;
            bool successful = false;
            try
            {
                handle = new BufferHandle(ref memoryHandle, byteSize, elementSize);

                SetDataBufferCore(name, handle.DataPointer, handle.SizePointer);

                AddOrReplaceBufferHandle(_dataBufferHandles, name, handle);
                successful = true;
            }
            finally
            {
                if (!successful)
                {
                    memoryHandle.Dispose();
                    handle?.Dispose();
                }
            }
        }

        private void SetDataBufferCore(string name, void* data, ulong* size)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(name);
            _ctx.handle_error(Methods.tiledb_query_set_data_buffer(ctxHandle, handle, ms_name, data, size));
        }

        /// <summary>
        /// Sets the offsets buffer for a variable-sized attribute or dimension to an array.
        /// </summary>
        /// <param name="name">The name of the attribute or the dimension.</param>
        /// <param name="data">An array where the offsets will be read or written.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="data"/> is empty.</exception>
        public void SetOffsetsBuffer(string name, ulong[] data)
        {
            if (data is null)
            {
                ThrowHelpers.ThrowArgumentNullException(nameof(data));
            }

            SetOffsetsBuffer(name, data.AsMemory());
        }

        /// <summary>
        /// Sets the offsets buffer for a variable-sized attribute or dimension to a <see cref="Memory{T}"/>.
        /// </summary>
        /// <param name="name">The name of the attribute or the dimension.</param>
        /// <param name="data">A <see cref="Memory{T}"/> where the offsets will be read or written.</param>
        /// <exception cref="ArgumentException"><paramref name="data"/> is empty.</exception>
        public void SetOffsetsBuffer(string name, Memory<ulong> data)
        {
            if (data.IsEmpty)
            {
                ThrowHelpers.ThrowBufferCannotBeEmpty(nameof(data));
            }

            UnsafeSetOffsetsBuffer(name, data.Pin(), (ulong)data.Length);
        }

        /// <summary>
        /// Sets the offsets buffer for a variable-sized attribute or dimension to an unmanaged memory buffer.
        /// </summary>
        /// <param name="name">The name of the attribute or the dimension.</param>
        /// <param name="data">A pointer to the memory buffer.</param>
        /// <param name="size">The buffer's size <em>in 64-bit integers</em>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="size"/> is zero.</exception>
        public void SetOffsetsBuffer(string name, ulong* data, ulong size)
        {
            if (data is null)
            {
                ThrowHelpers.ThrowArgumentNullException(nameof(data));
            }

            if (size == 0)
            {
                ThrowHelpers.ThrowBufferCannotBeEmpty(nameof(size));
            }

            UnsafeSetOffsetsBuffer(name, new MemoryHandle(data), size);
        }

        /// <summary>
        /// Sets the offsets buffer for a variable-sized attribute or dimension
        /// to a pinned memory buffer pointed by a <see cref="MemoryHandle"/>.
        /// </summary>
        /// <param name="name">The name of the attribute or the dimension.</param>
        /// <param name="memoryHandle">A <see cref="MemoryHandle"/> pointing to the buffer.</param>
        /// <param name="size">The buffer's size <em>in 64-bit integers</em>.</param>
        /// <exception cref="ArgumentException"><paramref name="size"/> is zero.</exception>
        /// <remarks>
        /// <para>After calling this method, user code must not use <paramref name="memoryHandle"/>;
        /// its ownership is transferred to this <see cref="Query"/> object.</para>
        /// <para><paramref name="memoryHandle"/> will be disposed when one of the following happens:
        /// <list type="bullet">
        /// <item>The <see cref="Dispose"/> method is called.</item>
        /// <item>The buffer for <paramref name="name"/> is reassigned through subsequent calls to this method.</item>
        /// <item>This method call throws an exception.</item>
        /// </list></para>
        /// </remarks>
        private void UnsafeSetOffsetsBuffer(string name, MemoryHandle memoryHandle, ulong size)
        {
            BufferHandle? handle = null;
            bool successful = false;
            try
            {
                handle = new BufferHandle(ref memoryHandle, size * sizeof(ulong), sizeof(ulong));

                SetOffsetsBufferCore(name, (ulong*)handle.DataPointer, handle.SizePointer);

                AddOrReplaceBufferHandle(_offsetsBufferHandles, name, handle);
                successful = true;
            }
            finally
            {
                if (!successful)
                {
                    memoryHandle.Dispose();
                    handle?.Dispose();
                }
            }
        }

        private void SetOffsetsBufferCore(string name, ulong* data, ulong* size)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(name);
            _ctx.handle_error(Methods.tiledb_query_set_offsets_buffer(ctxHandle, handle, ms_name, data, size));
        }

        /// <summary>
        /// Sets the offsets buffer for a variable-sized attribute or dimension to a <see cref="ReadOnlyMemory{T}"/>.
        /// Not supported for <see cref="QueryType.Read"/> queries.
        /// </summary>
        /// <param name="name">The name of the attribute or the dimension.</param>
        /// <param name="data">A <see cref="ReadOnlyMemory{T}"/> from where the offsets will be written.</param>
        /// <exception cref="ArgumentException"><paramref name="data"/> is empty.</exception>
        /// <exception cref="NotSupportedException">The query's type is <see cref="QueryType.Read"/></exception>
        public void SetOffsetsReadOnlyBuffer(string name, ReadOnlyMemory<ulong> data)
        {
            if (QueryType() != CSharp.QueryType.Read)
            {
                ThrowHelpers.ThrowOperationNotAllowedOnReadQueries();
            }

            SetOffsetsBuffer(name, MemoryMarshal.AsMemory(data));
        }

        /// <summary>
        /// Sets the validity buffer for a nullable attribute to an array.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="data">An array where the validities will be read or written.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="data"/> is empty.</exception>
        /// <remarks>
        /// Each value corresponds to whether an element exists (1) or not (0).
        /// </remarks>
        public void SetValidityBuffer(string name, byte[] data)
        {
            if (data is null)
            {
                ThrowHelpers.ThrowArgumentNullException(nameof(data));
            }

            SetValidityBuffer(name, data.AsMemory());
        }

        /// <summary>
        /// Sets the validity buffer for a nullable attribute to a <see cref="Memory{T}"/>.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="data">A <see cref="Memory{T}"/> where the validities will be read or written.</param>
        /// <exception cref="ArgumentException"><paramref name="data"/> is empty.</exception>
        /// <remarks>
        /// Each value corresponds to whether an element exists (1) or not (0).
        /// </remarks>
        public void SetValidityBuffer(string name, Memory<byte> data)
        {
            if (data.IsEmpty)
            {
                ThrowHelpers.ThrowBufferCannotBeEmpty(nameof(data));
            }

            UnsafeSetValidityBuffer(name, data.Pin(), (ulong)data.Length);
        }

        /// <summary>
        /// Sets the validity buffer for a nullable attribute to an unmanaged memory buffer.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="data">A pointer to the memory buffer.</param>
        /// <param name="size">The buffer's size.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="size"/> is zero.</exception>
        /// <remarks>
        /// Each value corresponds to whether an element exists (1) or not (0).
        /// </remarks>
        public void SetValidityBuffer(string name, byte* data, ulong size)
        {
            if (data is null)
            {
                ThrowHelpers.ThrowArgumentNullException(nameof(data));
            }

            if (size == 0)
            {
                ThrowHelpers.ThrowBufferCannotBeEmpty(nameof(size));
            }

            UnsafeSetValidityBuffer(name, new MemoryHandle(data), size);
        }

        /// <summary>
        /// Sets the validity buffer for a nullable attribute to a <see cref="ReadOnlyMemory{T}"/>.
        /// Not supported for <see cref="QueryType.Read"/> queries.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="data">A <see cref="ReadOnlyMemory{T}"/> from where the validities will be written.</param>
        /// <exception cref="ArgumentException"><paramref name="data"/> is empty.</exception>
        /// <exception cref="NotSupportedException">The query's type is <see cref="QueryType.Read"/></exception>
        /// <remarks>
        /// Each value corresponds to whether an element exists (1) or not (0).
        /// </remarks>
        public void SetValidityReadOnlyBuffer(string name, ReadOnlyMemory<byte> data)
        {
            if (QueryType() == CSharp.QueryType.Read)
            {
                ThrowHelpers.ThrowOperationNotAllowedOnReadQueries();
            }

            SetValidityBuffer(name, MemoryMarshal.AsMemory(data));
        }

        /// <summary>
        /// Sets the validity buffer for a nullable attribute
        /// to a pinned memory buffer pointed by a <see cref="MemoryHandle"/>.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="memoryHandle">A <see cref="MemoryHandle"/> pointing to the buffer.</param>
        /// <param name="size">The buffer's size.</param>
        /// <exception cref="ArgumentException"><paramref name="size"/> is zero.</exception>
        /// <remarks>
        /// <para>Each value corresponds to whether an element exists (1) or not (0).</para>
        /// <para>After calling this method, user code must not use <paramref name="memoryHandle"/>;
        /// its ownership is transferred to this <see cref="Query"/> object.</para>
        /// <para><paramref name="memoryHandle"/> will be disposed when one of the following happens:
        /// <list type="bullet">
        /// <item>The <see cref="Dispose"/> method is called.</item>
        /// <item>The buffer for <paramref name="name"/> is reassigned through subsequent calls to this method.</item>
        /// <item>This method call throws an exception.</item>
        /// </list></para>
        /// </remarks>
        private void UnsafeSetValidityBuffer(string name, MemoryHandle memoryHandle, ulong size)
        {
            BufferHandle? handle = null;
            bool successful = false;
            try
            {
                handle = new BufferHandle(ref memoryHandle, size * sizeof(byte), sizeof(byte));

                SetValidityBufferCore(name, (byte*)handle.DataPointer, handle.SizePointer);

                AddOrReplaceBufferHandle(_validityBufferHandles, name, handle);
                successful = true;
            }
            finally
            {
                if (!successful)
                {
                    memoryHandle.Dispose();
                    handle?.Dispose();
                }
            }
        }

        private void SetValidityBufferCore(string name, byte* data, ulong* size)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(name);
            _ctx.handle_error(Methods.tiledb_query_set_validity_buffer(ctxHandle, handle, ms_name, data, size));
        }

        /// <summary>
        /// Sets the layout of the cells to be written or read.
        /// </summary>
        /// <param name="layouttype"></param>
        public void SetLayout(LayoutType layouttype)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_query_set_layout(ctxHandle, handle, (tiledb_layout_t)layouttype));
        }

        /// <summary>
        /// Sets the layout of the cells to be written or read.
        /// </summary>
        /// <returns></returns>
        public LayoutType QueryLayout()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            tiledb_layout_t layout;
            _ctx.handle_error(Methods.tiledb_query_get_layout(ctxHandle, handle, &layout));
            return (LayoutType)layout;
        }

        /// <summary>
        /// Sets the query condition to be applied on a read.
        /// </summary>
        /// <param name="condition"></param>
        public void SetCondition(QueryCondition condition)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var conditionHandle = condition.Handle.Acquire();
            _ctx.handle_error(Methods.tiledb_query_set_condition(ctxHandle, handle, conditionHandle));
        }

        /// <summary>
        /// Flushes all internal state of a query object and finalizes the query, only applicable to global layout writes.
        /// </summary>
        public void FinalizeQuery()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_query_finalize(ctxHandle, handle));
        }

        /// <summary>
        /// Submits the query. Call will block until query is complete.
        /// </summary>
        /// <returns></returns>
        public QueryStatus Submit()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_query_submit(ctxHandle, handle));
            return QueryStatus.Completed;
        }

        /// <summary>
        /// Submits and finalizes the query.
        /// </summary>
        /// <seealso cref="Submit"/>
        /// <seealso cref="FinalizeQuery"/>
        public void SubmitAndFinalize()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            _ctx.handle_error(Methods.tiledb_query_submit_and_finalize(ctxHandle, handle));
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        [Obsolete(Obsoletions.QuerySubmitAsyncMessage, DiagnosticId = Obsoletions.QuerySubmitAsyncDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        private static void QueryCallback(void* ptr)
        {
            GCHandle gcHandle = GCHandle.FromIntPtr((IntPtr)ptr);
            var query = (Query)gcHandle.Target!;
            gcHandle.Free();
            // Fire the event on the thread pool.
            // This yields the TileDB native thread and prevents any excpetions by the callback to reach it.
            Task.Run(() =>
            {
                var args = new QueryEventArgs((int)QueryStatus.Completed, "query completed");
                query.QueryCompleted?.Invoke(query, args);
            });
        }

        /// <summary>
        /// Submits the query asynchronously.
        /// </summary>
        [Obsolete(Obsoletions.QuerySubmitAsyncMessage, DiagnosticId = Obsoletions.QuerySubmitAsyncDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        public void SubmitAsync()
        {
            GCHandle gcHandle = GCHandle.Alloc(this);
            bool successful = false;
            try
            {
                using var ctxHandle = _ctx.Handle.Acquire();
                using var handle = _handle.Acquire();
                _ctx.handle_error(Methods.tiledb_query_submit_async(ctxHandle, handle, &QueryCallback, (void*)GCHandle.ToIntPtr(gcHandle)));
                successful = true;
            }
            finally
            {
                if (!successful)
                {
                    gcHandle.Free();
                }
            }
        }

        /// <summary>
        /// Default event handler is empty
        /// </summary>
        [Obsolete(Obsoletions.QuerySubmitAsyncMessage, DiagnosticId = Obsoletions.QuerySubmitAsyncDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        public event EventHandler<QueryEventArgs>? QueryCompleted;

        /// <summary>
        /// Returns `true` if the query has results. Applicable only to read; false for write queries.
        /// </summary>
        /// <returns></returns>
        public bool HasResults()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            int ret;
            _ctx.handle_error(Methods.tiledb_query_has_results(ctxHandle, handle, &ret));
            return ret > 0;
        }

        /// <summary>
        /// Returns the query status.
        /// </summary>
        /// <returns></returns>
        public QueryStatus Status()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            tiledb_query_status_t query_status;
            _ctx.handle_error(Methods.tiledb_query_get_status(ctxHandle, handle, &query_status));
            return (QueryStatus)query_status;
        }

        /// <summary>
        /// Returns the query type (read or write).
        /// </summary>
        /// <returns></returns>
        public QueryType QueryType()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            tiledb_query_type_t query_type;
            _ctx.handle_error(Methods.tiledb_query_get_type(ctxHandle, handle, &query_type));
            return (QueryType)query_type;
        }

        /// <summary>
        /// Returns the array of the query.
        /// </summary>
        /// <returns></returns>
        public Array Array()
        {
            return _array;
        }

        /// <summary>
        /// Deprecated, use <see cref="Subarray.AddRange{T}(uint, T, T)"/> on the query's assigned <see cref="Subarray"/> instead.
        /// </summary>
        [Obsolete(Obsoletions.QuerySubarrayMessage + " Use Subarray.AddRange on the query's assigned Subarray instead.", DiagnosticId = Obsoletions.QuerySubarrayDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void AddRange<T>(uint index, T start, T end) where T : struct
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            T[] startData = new T[1] { start };
            T[] endData = new T[1] { end };
            var startDataGcHandle = GCHandle.Alloc(startData, GCHandleType.Pinned);
            var endDataGcHandle = GCHandle.Alloc(endData, GCHandleType.Pinned);
            try
            {
                _ctx.handle_error(Methods.tiledb_query_add_range(ctxHandle, handle, index,
                    (void*)startDataGcHandle.AddrOfPinnedObject(),
                    (void*)endDataGcHandle.AddrOfPinnedObject(),
                    null
                ));
            }
            finally
            {
                startDataGcHandle.Free();
                endDataGcHandle.Free();
            }
        }

        /// <summary>
        /// Deprecated, use <see cref="Subarray.AddRange{T}(string, T, T)"/> on the query's assigned <see cref="Subarray"/> instead.
        /// </summary>
        [Obsolete(Obsoletions.QuerySubarrayMessage + " Use Subarray.AddRange on the query's assigned Subarray instead.", DiagnosticId = Obsoletions.QuerySubarrayDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void AddRange<T>(string name, T start, T end) where T : struct
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(name);
            T[] startData = new T[1] { start };
            T[] endData = new T[1] { end };
            var startDataGcHandle = GCHandle.Alloc(startData, GCHandleType.Pinned);
            var endDataGcHandle = GCHandle.Alloc(endData, GCHandleType.Pinned);
            try
            {
                _ctx.handle_error(Methods.tiledb_query_add_range_by_name(ctxHandle, handle, ms_name,
                    (void*)startDataGcHandle.AddrOfPinnedObject(),
                    (void*)endDataGcHandle.AddrOfPinnedObject(),
                    null
                ));
            }
            finally
            {
                startDataGcHandle.Free();
                endDataGcHandle.Free();
            }
        }

        /// <summary>
        /// Deprecated, use <see cref="Subarray.AddRange(uint, string, string)"/> on the query's assigned <see cref="Subarray"/> instead.
        /// </summary>
        [Obsolete(Obsoletions.QuerySubarrayMessage + " Use Subarray.AddRange on the query's assigned Subarray instead.", DiagnosticId = Obsoletions.QuerySubarrayDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void AddRange(uint index, string start, string end)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            byte[] startData = Encoding.ASCII.GetBytes(start);
            byte[] endData = Encoding.ASCII.GetBytes(end);
            var startDataGcHandle = GCHandle.Alloc(startData, GCHandleType.Pinned);
            var endDataGcHandle = GCHandle.Alloc(endData, GCHandleType.Pinned);
            try
            {
                _ctx.handle_error(Methods.tiledb_query_add_range_var(ctxHandle, handle, index,
                    (void*)startDataGcHandle.AddrOfPinnedObject(), (ulong)startData.Length,
                    (void*)endDataGcHandle.AddrOfPinnedObject(), (ulong)endData.Length));
            }
            finally
            {
                startDataGcHandle.Free();
                endDataGcHandle.Free();
            }
        }

        /// <summary>
        /// Deprecated, use <see cref="Subarray.AddRange(string, string, string)"/> on the query's assigned <see cref="Subarray"/> instead.
        /// </summary>
        [Obsolete(Obsoletions.QuerySubarrayMessage + " Use Subarray.AddRange on the query's assigned Subarray instead.", DiagnosticId = Obsoletions.QuerySubarrayDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void AddRange(string name, string start, string end)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(name);
            byte[] startData = Encoding.ASCII.GetBytes(start);
            byte[] endData = Encoding.ASCII.GetBytes(end);
            var startDataGcHandle = GCHandle.Alloc(startData, GCHandleType.Pinned);
            var endDataGcHandle = GCHandle.Alloc(endData, GCHandleType.Pinned);
            try
            {
                _ctx.handle_error(Methods.tiledb_query_add_range_var_by_name(ctxHandle, handle, ms_name,
                    (void*)startDataGcHandle.AddrOfPinnedObject(), (ulong)startData.Length,
                    (void*)endDataGcHandle.AddrOfPinnedObject(), (ulong)endData.Length));
            }
            finally
            {
                startDataGcHandle.Free();
                endDataGcHandle.Free();
            }
        }

        /// <summary>
        /// Deprecated, use <see cref="Subarray.GetRangeCount(uint)"/> instead on the query's assigned <see cref="Subarray"/>.
        /// </summary>
        [Obsolete(Obsoletions.QuerySubarrayMessage + " Use Subarray.GetRangeCount on the query's assigned Subarray instead.", DiagnosticId = Obsoletions.QuerySubarrayDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ulong RangeNum(uint index)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            ulong range_num;
            _ctx.handle_error(Methods.tiledb_query_get_range_num(ctxHandle, handle, index, &range_num));
            return range_num;
        }

        /// <summary>
        /// Deprecated, use <see cref="Subarray.GetRangeCount(string)"/> instead on the query's assigned <see cref="Subarray"/>.
        /// </summary>
        [Obsolete(Obsoletions.QuerySubarrayMessage + " Use Subarray.GetRangeCount on the query's assigned Subarray instead.", DiagnosticId = Obsoletions.QuerySubarrayDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ulong RangeNumFromName(string name)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            ulong range_num;
            using var ms_name = new MarshaledString(name);
            _ctx.handle_error(Methods.tiledb_query_get_range_num_from_name(ctxHandle, handle, ms_name, &range_num));
            return range_num;
        }

        [Obsolete(Obsoletions.QuerySubarrayMessage, DiagnosticId = Obsoletions.QuerySubarrayDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        private (byte[] start_bytes, byte[] end_bytes, byte[] stride_bytes) get_range<T>(uint dim_idx, uint range_idx) where T : struct
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            void* start_p;
            void* end_p;
            void* stride_p;
            ulong size = EnumUtil.DataTypeSize(EnumUtil.TypeToDataType(typeof(T)));
            _ctx.handle_error(Methods.tiledb_query_get_range(ctxHandle, handle, dim_idx, range_idx, &start_p, &end_p, &stride_p));

            var start_span = new ReadOnlySpan<byte>(start_p, (int)size);
            var end_span = new ReadOnlySpan<byte>(end_p, (int)size);
            var stride_span = new ReadOnlySpan<byte>(stride_p, (int)size);
            return (start_span.ToArray(), end_span.ToArray(), stride_span.ToArray());
        }

        /// <summary>
        /// Deprecated, use <see cref="Subarray.GetRange{T}(uint, uint)"/> instead on the query's assigned <see cref="Subarray"/>.
        /// </summary>
        [Obsolete(Obsoletions.QuerySubarrayMessage + " Use Subarray.GetRange on the query's assigned Subarray instead. Note that it does not return a stride.", DiagnosticId = Obsoletions.QuerySubarrayDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Tuple<T, T, T> Range<T>(uint dim_idx, uint range_idx) where T : struct
        {
            var (start_bytes, end_bytes, stride_bytes) = get_range<T>(dim_idx, range_idx);
            var start_span = MemoryMarshal.Cast<byte, T>(start_bytes);
            var end_span = MemoryMarshal.Cast<byte, T>(end_bytes);
            var stride_span = MemoryMarshal.Cast<byte, T>(stride_bytes);

            return new Tuple<T, T, T>(start_span.ToArray()[0], end_span.ToArray()[0], stride_span.ToArray()[0]);
        }

        [Obsolete(Obsoletions.QuerySubarrayMessage, DiagnosticId = Obsoletions.QuerySubarrayDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        private (byte[] start_bytes, byte[] end_bytes, byte[] stride_bytes) get_range<T>(string dim_name, uint range_idx) where T : struct
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(dim_name);
            void* start_p;
            void* end_p;
            void* stride_p;
            ulong size = EnumUtil.DataTypeSize(EnumUtil.TypeToDataType(typeof(T)));
            _ctx.handle_error(Methods.tiledb_query_get_range_from_name(ctxHandle, handle, ms_name, range_idx, &start_p, &end_p, &stride_p));

            var start_span = new ReadOnlySpan<byte>(start_p, (int)size);
            var end_span = new ReadOnlySpan<byte>(end_p, (int)size);
            var stride_span = new ReadOnlySpan<byte>(stride_p, (int)size);
            return (start_span.ToArray(), end_span.ToArray(), stride_span.ToArray());
        }

        /// <summary>
        /// Deprecated, use <see cref="Subarray.GetRange{T}(string, uint)"/> instead and assign the subarray with <see cref="SetSubarray(Subarray)"/>.
        /// </summary>
        [Obsolete(Obsoletions.QuerySubarrayMessage + " Use Subarray.GetRange on the query's assigned Subarray instead. Note that it does not return a stride.", DiagnosticId = Obsoletions.QuerySubarrayDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Tuple<T, T, T> Range<T>(string dim_name, uint range_idx) where T : struct
        {
            var (start_bytes, end_bytes, stride_bytes) = get_range<T>(dim_name, range_idx);
            var start_span = MemoryMarshal.Cast<byte, T>(start_bytes);
            var end_span = MemoryMarshal.Cast<byte, T>(end_bytes);
            var stride_span = MemoryMarshal.Cast<byte, T>(stride_bytes);

            return new Tuple<T, T, T>(start_span.ToArray()[0], end_span.ToArray()[0], stride_span.ToArray()[0]);
        }

        /// <summary>
        /// Deprecated, use <see cref="Subarray.GetStringRange(uint, uint)"/> instead on the query's assigned <see cref="Subarray"/>.
        /// </summary>
        [Obsolete(Obsoletions.QuerySubarrayMessage + " Use Subarray.GetStringRange on the query's assigned Subarray instead.", DiagnosticId = Obsoletions.QuerySubarrayDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Tuple<string, string> RangeVar(uint dim_idx, uint range_idx)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            ulong start_size;
            ulong end_size;
            _ctx.handle_error(Methods.tiledb_query_get_range_var_size(ctxHandle, handle, dim_idx, range_idx, &start_size, &end_size));

            byte[] startData = Enumerable.Repeat(default(byte), (int)start_size).ToArray();
            byte[] endData = Enumerable.Repeat(default(byte), (int)end_size).ToArray();

            var startDataGcHandle = GCHandle.Alloc(startData, GCHandleType.Pinned);
            var endDataGcHandle = GCHandle.Alloc(endData, GCHandleType.Pinned);

            try
            {
                _ctx.handle_error(Methods.tiledb_query_get_range_var(ctxHandle, handle, dim_idx, range_idx,
                    (void*)startDataGcHandle.AddrOfPinnedObject(),
                    (void*)endDataGcHandle.AddrOfPinnedObject()));
            }
            finally
            {
                startDataGcHandle.Free();
                endDataGcHandle.Free();
            }

            return new Tuple<string, string>(MarshaledStringOut.GetString(startData), MarshaledStringOut.GetString(endData));

        }

        /// <summary>
        /// Deprecated, use <see cref="Subarray.GetStringRange(string, uint)"/> instead on the query's assigned <see cref="Subarray"/>.
        /// </summary>
        [Obsolete(Obsoletions.QuerySubarrayMessage + " Use Subarray.GetStringRange on the query's assigned Subarray instead.", DiagnosticId = Obsoletions.QuerySubarrayDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Tuple<string, string> RangeVar(string dim_name, uint range_idx)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(dim_name);
            ulong start_size;
            ulong end_size;
            _ctx.handle_error(Methods.tiledb_query_get_range_var_size_from_name(ctxHandle, handle, ms_name, range_idx, &start_size, &end_size));

            byte[] startData = Enumerable.Repeat(default(byte), (int)start_size).ToArray();
            byte[] endData = Enumerable.Repeat(default(byte), (int)end_size).ToArray();

            var startDataGcHandle = GCHandle.Alloc(startData, GCHandleType.Pinned);
            var endDataGcHandle = GCHandle.Alloc(endData, GCHandleType.Pinned);

            try
            {
                _ctx.handle_error(Methods.tiledb_query_get_range_var_from_name(ctxHandle, handle, ms_name, range_idx,
                    (void*)startDataGcHandle.AddrOfPinnedObject(),
                    (void*)endDataGcHandle.AddrOfPinnedObject()));
            }
            finally
            {
                startDataGcHandle.Free();
                endDataGcHandle.Free();
            }

            return new Tuple<string, string>(MarshaledStringOut.GetString(startData), MarshaledStringOut.GetString(endData));

        }

        private ulong GetEstimatedResultSize(string name)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(name);
            ulong size;
            _ctx.handle_error(Methods.tiledb_query_get_est_result_size(ctxHandle, handle, ms_name, &size));
            return size;
        }

        private (ulong OffsetSize, ulong DataSize) GetEstimatedResultSizeVar(string name)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(name);
            ulong size_off;
            ulong size_val;
            _ctx.handle_error(Methods.tiledb_query_get_est_result_size_var(ctxHandle, handle, ms_name, &size_off, &size_val));

            return (size_off, size_val);
        }

        private (ulong DataSize, ulong ValiditySize) GetEstimatedResultSizeNullable(string name)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(name);
            ulong size_val;
            ulong size_validity;
            _ctx.handle_error(Methods.tiledb_query_get_est_result_size_nullable(ctxHandle, handle, ms_name, &size_val, &size_validity));
            return (size_val, size_validity);
        }

        private (ulong OffsetsSize, ulong DataSize, ulong ValiditySize) GetEstimatedResultSizeVarNullable(string name)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            using var ms_name = new MarshaledString(name);
            ulong size_off;
            ulong size_val;
            ulong size_validity;

            _ctx.handle_error(Methods.tiledb_query_get_est_result_size_var_nullable(ctxHandle, handle, ms_name, &size_off, &size_val, &size_validity));

            return (size_off, size_val, size_validity);
        }

        /// <summary>
        /// Estimates the result size for an attribute or dimension.
        /// </summary>
        /// <param name="name">The name of the attribute or dimension.</param>
        public ResultSize EstResultSize(string name)
        {
            bool isVar, isNullable;
            using (var schema = _array.Schema())
            {
                isVar = schema.IsVarSize(name);
                isNullable = schema.IsNullable(name);
            }
            switch (isVar, isNullable)
            {
                case (true, true):
                    (var offsetsSize, var dataSize, var validitySize) = GetEstimatedResultSizeVarNullable(name);
                    return new ResultSize(offsetsSize, dataSize, validitySize);
                case (true, false):
                    (offsetsSize, dataSize) = GetEstimatedResultSizeVar(name);
                    return new ResultSize(dataSize, offsetsSize);
                case (false, true):
                    (dataSize, validitySize) = GetEstimatedResultSizeNullable(name);
                    return new ResultSize(dataSize, null, validitySize);
                case (false, false):
                    dataSize = GetEstimatedResultSize(name);
                    return new ResultSize(dataSize);
            }
        }

        /// <summary>
        /// Returns the number of written fragments. Applicable only to WRITE queries.
        /// </summary>
        /// <returns></returns>
        public uint FragmentNum()
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            uint num;
            _ctx.handle_error(Methods.tiledb_query_get_fragment_num(ctxHandle, handle, &num));
            return num;
        }

        /// <summary>
        /// Returns the URI of the written fragment with the input index. Applicable only to WRITE queries.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public string FragmentUri(ulong idx)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            sbyte* result;
            _ctx.handle_error(Methods.tiledb_query_get_fragment_uri(ctxHandle, handle, idx, &result));
            return MarshaledStringOut.GetStringFromNullTerminated(result);
        }

        /// <summary>
        /// Retrieves the timestamp range of the written fragment with the input index. Applicable only to WRITE queries.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public Tuple<ulong, ulong> FragmentTimestampRange(ulong idx)
        {
            using var ctxHandle = _ctx.Handle.Acquire();
            using var handle = _handle.Acquire();
            ulong t1;
            ulong t2;
            _ctx.handle_error(Methods.tiledb_query_get_fragment_timestamp_range(ctxHandle, handle, idx, &t1, &t2));
            return new Tuple<ulong, ulong>(t1, t2);
        }

        private void CheckDataType<T>(DataType dataType)
        {
            if (EnumUtil.TypeToDataType(typeof(T)) != dataType)
            {
                if (!(dataType== DataType.StringAscii && (typeof(T)==typeof(byte) || typeof(T) == typeof(sbyte) || typeof(T) == typeof(string)))
                   && !(dataType == DataType.Boolean && typeof(T) == typeof(byte)))
                {
                    throw new ArgumentException("T " + typeof(T).Name + " doesnot match " + dataType.ToString());
                }
            }
        }

        private static DataType GetDataType(string name, ArraySchema schema, Domain domain)
        {
            if (schema.HasAttribute(name))
            {
                using var attribute = schema.Attribute(name);
                return attribute.Type();
            }
            else if (domain.HasDimension(name))
            {
                using var dimension = domain.Dimension(name);
                return dimension.Type();
            }

            if (name == "__coords")
            {
                return domain.Type();
            }

            throw new ArgumentException("No datatype for " + name);
        }

        /// <summary>
        /// Returns the number of elements read into result buffers from a read query.
        ///
        /// Dictionary key: Name of buffer used in call to SetDataBuffer, SetOffsetBuffer, or SetValidityBuffer
        /// Tuple Item1: Number of elements read by the query
        /// Tuple Item2: Number of offset elements read by the query
        /// Tuple Item3: Number of validity bytes read by the query
        ///
        /// If the buffer is not variable-sized, Tuple.Item2 will be set to null
        /// If the buffer is not nullable, Tuple.Item3 will be set to null
        /// </summary>
        /// <returns>Dictionary mapping buffer name to number of results</returns>
        public Dictionary<string, Tuple<ulong, ulong?, ulong?>> ResultBufferElements()
        {
            using var schema = _array.Schema();
            using var domain = schema.Domain();
            var buffers = new Dictionary<string, Tuple<ulong, ulong?, ulong?>>();
            foreach ((string key, BufferHandle dataHandle) in _dataBufferHandles)
            {
                ulong? offsetNum = null;
                ulong? validityNum = null;

                ulong typeSize = EnumUtil.DataTypeSize(GetDataType(key, schema, domain));
                ulong dataNum = dataHandle.SizeInBytes / typeSize;

                if (_offsetsBufferHandles.TryGetValue(key, out BufferHandle? offsetHandle))
                {
                    offsetNum = offsetHandle.SizeInBytes / sizeof(ulong);
                }
                if (_validityBufferHandles.TryGetValue(key, out BufferHandle? validityHandle))
                {
                    validityNum = validityHandle.SizeInBytes;
                }

                buffers.Add(key, new(dataNum, offsetNum, validityNum));
            }

            return buffers;
        }

        /// <summary>
        /// Returns the number of elements read into the data buffer of an attribute or dimension.
        /// </summary>
        /// <param name="name">The name of the attribute or dimension.</param>
        /// <exception cref="KeyNotFoundException">No data buffer has
        /// been registered with <paramref name="name"/>.</exception>
        /// <exception cref="InvalidOperationException">The data buffer had been
        /// set with an overload of <c>UnsafeSetDataBuffer</c>.</exception>
        public ulong GetResultDataElements(string name) => _dataBufferHandles[name].SizeInElements;

        /// <summary>
        /// Returns the number of bytes read into the data buffer of an attribute or dimension.
        /// </summary>
        /// <param name="name">The name of the attribute or dimension.</param>
        /// <returns>
        /// The number of elements read, or the number of bytes read if the data buffer had been
        /// assigned with an overload of the <c>Query.UnsafeSetDataBuffer</c> method.
        /// </returns>
        /// <exception cref="KeyNotFoundException">No data buffer has
        /// been registered with <paramref name="name"/>.</exception>
        public ulong GetResultDataBytes(string name) => _dataBufferHandles[name].SizeInBytes;

        /// <summary>
        /// Returns the number of offsets read into the offsets buffer of a variable-sized attribute or dimension.
        /// </summary>
        /// <param name="name">The name of the attribute or dimension.</param>
        /// <exception cref="KeyNotFoundException">No offsets buffer has
        /// been registered with <paramref name="name"/>.</exception>
        public ulong GetResultOffsets(string name) => _offsetsBufferHandles[name].SizeInElements;

        /// <summary>
        /// Returns the number of validity bytes read into the validity buffer of a nullable attribute.
        /// </summary>
        /// <param name="name">The name of the attribute or dimension.</param>
        /// <exception cref="KeyNotFoundException">No validity buffer has
        /// been registered with <paramref name="name"/>.</exception>
        public ulong GetResultValidities(string name) => _validityBufferHandles[name].SizeInElements;

        /// <summary>
        /// Free all handles.
        /// </summary>
        private void FreeAllBufferHandles()
        {
            DisposeValuesAndClear(_dataBufferHandles);
            DisposeValuesAndClear(_offsetsBufferHandles);
            DisposeValuesAndClear(_validityBufferHandles);

            static void DisposeValuesAndClear(Dictionary<string, BufferHandle> handles)
            {
                foreach (var bh in handles)
                {
                    bh.Value.Dispose();
                }
                handles.Clear();
            }
        }

        private static void AddOrReplaceBufferHandle(Dictionary<string, BufferHandle> dict, string name, BufferHandle handle)
        {
            if (dict.TryGetValue(name, out var existingHandle))
            {
                existingHandle.Dispose();
            }
            dict[name] = handle;
        }

        private sealed class BufferHandle : IDisposable
        {
            private readonly int _elementSize;

            private MemoryHandle DataHandle;

            public ulong* SizePointer { get; private set; }

            public void* DataPointer => DataHandle.Pointer;

            public ulong SizeInBytes => *SizePointer;

            public ulong SizeInElements
            {
                get
                {
                    if (_elementSize == 0)
                    {
                        ThrowHelpers.ThrowBufferUnsafelySet();
                    }
                    return *SizePointer / (ulong)_elementSize;
                }
            }

            public BufferHandle(ref MemoryHandle handle, ulong size, int elementSize)
            {
                DataHandle = handle;
                handle = default;
                _elementSize = elementSize;
                bool successful = false;
                try
                {
                    SizePointer = (ulong*)Marshal.AllocHGlobal(sizeof(ulong));
                    successful = true;
                }
                finally
                {
                    if (!successful)
                    {
                        DataHandle.Dispose();
                    }
                }
                *SizePointer = size;
            }

            public void Dispose()
            {
                DataHandle.Dispose();
                if (SizePointer != null)
                {
                    Marshal.FreeHGlobal((IntPtr)SizePointer);
                }
                SizePointer = null;
            }
        }
    }
}

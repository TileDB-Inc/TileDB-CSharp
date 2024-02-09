using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TileDB.Interop;

namespace TileDB.CSharp.Marshalling.SafeHandles;

internal unsafe sealed class QueryHandle : SafeHandle
{
    // Dictionaries that contain the pinned handles for each buffer.
    // These _must_ be accessed while the handle is being `Acquire()`d.
    // The reason for this is to prevent freeing the handle while these
    // dictionaries are used and to prevent using them after the handle
    // is freed.
    // This does _not_ mean that setting query buffers from multiple threads
    // is safe (it never was), but calling Dispose() on the handle while another
    // thread is using it _is_ safe, according to the guarantees provided by SafeHandle.
    private readonly Dictionary<string, BufferHandle> _dataBufferHandles = [];
    private readonly Dictionary<string, BufferHandle> _offsetsBufferHandles = [];
    private readonly Dictionary<string, BufferHandle> _validityBufferHandles = [];

    public QueryHandle() : base(IntPtr.Zero, true) { }

    public QueryHandle(IntPtr handle, bool ownsHandle) : base(IntPtr.Zero, ownsHandle) { SetHandle(handle); }

    public static QueryHandle Create(Context context, ArrayHandle arrayHandle, tiledb_query_type_t queryType)
    {
        var handle = new QueryHandle();
        var successful = false;
        tiledb_query_t* query = null;
        try
        {
            using var contextHandle = context.Handle.Acquire();
            using var arrayHandleHolder = arrayHandle.Acquire();
            context.handle_error(Methods.tiledb_query_alloc(contextHandle, arrayHandleHolder, queryType, &query));
            successful = true;
        }
        finally
        {
            if (successful)
            {
                handle.InitHandle(query);
            }
            else
            {
                handle.SetHandleAsInvalid();
            }
        }

        return handle;
    }

    protected override bool ReleaseHandle()
    {
        // Free the native query handle. This must happen first.
        fixed (IntPtr* p = &handle)
        {
            Methods.tiledb_query_free((tiledb_query_t**)p);
        }
        // Free the buffer handles. ReleaseHandle is guaranteed to run only
        // once at a point where no one has acquired or can acquire the handle
        // anymore so this is safe.
        DisposeValuesAndClear(_dataBufferHandles);
        DisposeValuesAndClear(_offsetsBufferHandles);
        DisposeValuesAndClear(_validityBufferHandles);
        return true;

        static void DisposeValuesAndClear(Dictionary<string, BufferHandle> handles)
        {
            foreach (var bh in handles)
            {
                bh.Value.Dispose();
            }
            handles.Clear();
        }
    }

    internal void InitHandle(tiledb_query_t* h) { SetHandle((IntPtr)h); }
    public override bool IsInvalid => handle == IntPtr.Zero;

    public SafeHandleHolder<tiledb_query_t> Acquire() => new(this);

    public void UnsafeSetDataBuffer(Context ctx, string name, MemoryHandle memoryHandle, ulong byteSize, int elementSize)
    {
        BufferHandle? bufferHandle = null;
        bool successful = false;
        try
        {
            bufferHandle = new BufferHandle(ref memoryHandle, byteSize, elementSize);

            using var ctxHandle = ctx.Handle.Acquire();
            // This must be in scope while calling AddOrReplaceBufferHandle.
            using var handle = Acquire();
            using var ms_name = new MarshaledString(name);
            ctx.handle_error(Methods.tiledb_query_set_data_buffer(ctxHandle, handle, ms_name, bufferHandle.DataPointer, bufferHandle.SizePointer));

            AddOrReplaceBufferHandle(_dataBufferHandles, name, bufferHandle);
            successful = true;
        }
        finally
        {
            if (!successful)
            {
                memoryHandle.Dispose();
                bufferHandle?.Dispose();
            }
        }
    }

    public void UnsafeSetOffsetsBuffer(Context ctx, string name, MemoryHandle memoryHandle, ulong size)
    {
        BufferHandle? bufferHandle = null;
        bool successful = false;
        try
        {
            bufferHandle = new BufferHandle(ref memoryHandle, size * sizeof(ulong), sizeof(ulong));

            using var ctxHandle = ctx.Handle.Acquire();
            // This must be in scope while calling AddOrReplaceBufferHandle.
            using var handle = Acquire();
            using var ms_name = new MarshaledString(name);
            ctx.handle_error(Methods.tiledb_query_set_offsets_buffer(ctxHandle, handle, ms_name, (ulong*)bufferHandle.DataPointer, bufferHandle.SizePointer));

            AddOrReplaceBufferHandle(_offsetsBufferHandles, name, bufferHandle);
            successful = true;
        }
        finally
        {
            if (!successful)
            {
                memoryHandle.Dispose();
                bufferHandle?.Dispose();
            }
        }
    }

    public void UnsafeSetValidityBuffer(Context ctx, string name, MemoryHandle memoryHandle, ulong byteSize)
    {
        BufferHandle? bufferHandle = null;
        bool successful = false;
        try
        {
            bufferHandle = new BufferHandle(ref memoryHandle, byteSize, sizeof(byte));

            using var ctxHandle = ctx.Handle.Acquire();
            // This must be in scope while calling AddOrReplaceBufferHandle.
            using var handle = Acquire();
            using var ms_name = new MarshaledString(name);
            ctx.handle_error(Methods.tiledb_query_set_validity_buffer(ctxHandle, handle, ms_name, (byte*)bufferHandle.DataPointer, bufferHandle.SizePointer));

            AddOrReplaceBufferHandle(_validityBufferHandles, name, bufferHandle);
            successful = true;
        }
        finally
        {
            if (!successful)
            {
                memoryHandle.Dispose();
                bufferHandle?.Dispose();
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

    public Dictionary<string, Tuple<ulong, ulong?, ulong?>> ResultBufferElements(Array array)
    {
        // Do not remove; the handle must be acquired while accessing the buffer dictionaries.
        using var handle = Acquire();
        using var schema = array.Schema();
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

    public ulong GetResultDataElements(string name)
    {
        // Do not remove; the handle must be acquired while accessing the buffer dictionaries.
        using var handle = Acquire();
        return _dataBufferHandles[name].SizeInElements;
    }

    public ulong GetResultDataBytes(string name)
    {
        // Do not remove; the handle must be acquired while accessing the buffer dictionaries.
        using var handle = Acquire();
        return _dataBufferHandles[name].SizeInBytes;
    }

    public ulong GetResultOffsets(string name)
    {
        // Do not remove; the handle must be acquired while accessing the buffer dictionaries.
        using var handle = Acquire();
        return _offsetsBufferHandles[name].SizeInElements;
    }

    public ulong GetResultValidities(string name)
    {
        // Do not remove; the handle must be acquired while accessing the buffer dictionaries.
        using var handle = Acquire();
        return _validityBufferHandles[name].SizeInElements;
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

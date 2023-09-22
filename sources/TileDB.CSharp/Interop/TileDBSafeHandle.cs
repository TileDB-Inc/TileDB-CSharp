using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using TileDB.CSharp;

namespace TileDB.Interop
{
    [Obsolete(Obsoletions.TileDBInterop2Message, DiagnosticId = Obsoletions.TileDBInterop2DiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public unsafe class ConfigHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure

        public ConfigHandle() : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_config_t*[1];
            var e = stackalloc tiledb_error_t*[1];
            Methods.tiledb_config_alloc(h, e);

            if (h[0] == (void*)0)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(h[0]);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        override protected bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_config_t*)handle;
            Methods.tiledb_config_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public ulong Get() { return (ulong)handle; }
        private protected void SetHandle(tiledb_config_t* h) { SetHandle((IntPtr)h); }
        private protected ConfigHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(ConfigHandle h) => h.handle;
        public static implicit operator tiledb_config_t*(ConfigHandle h) => (tiledb_config_t*)h.handle;
        public static implicit operator ConfigHandle(tiledb_config_t* value) => new ConfigHandle((IntPtr)value);
    }//public unsafe partial class ConfigHandle

    [Obsolete(Obsoletions.TileDBInterop2Message, DiagnosticId = Obsoletions.TileDBInterop2DiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public unsafe class ConfigIteratorHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure

        public ConfigIteratorHandle(ConfigHandle hconfig, string prefix) : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_config_iter_t*[1];
            var e = stackalloc tiledb_error_t*[1];
            var ms_prefix = new MarshaledString(prefix);
            Methods.tiledb_config_iter_alloc(hconfig, ms_prefix, h, e);

            if (h[0] == (void*)0)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(h[0]);
        }

        // Deallocator: call native free with CER guarantees from SafeHTha andle
        override protected bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_config_iter_t*)handle;
            Methods.tiledb_config_iter_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public ulong Get() { return (ulong)handle; }
        private protected void SetHandle(tiledb_config_iter_t* h) { SetHandle((IntPtr)h); }
        private protected ConfigIteratorHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(ConfigIteratorHandle h) => h.handle;
        public static implicit operator tiledb_config_iter_t*(ConfigIteratorHandle h) => (tiledb_config_iter_t*)h.handle;
        public static implicit operator ConfigIteratorHandle(tiledb_config_iter_t* value) => new ConfigIteratorHandle((IntPtr)value);
    }//public unsafe partial class ConfigHandle

    [Obsolete(Obsoletions.TileDBInterop2Message, DiagnosticId = Obsoletions.TileDBInterop2DiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public unsafe class ContextHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure
        public ContextHandle() : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_ctx_t*[1];
            var hconfig = new ConfigHandle();
            Methods.tiledb_ctx_alloc(hconfig, h);

            if (h[0] == (void*)0)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(h[0]);
        }

        public ContextHandle(ConfigHandle hconfig) : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_ctx_t*[1];
            Methods.tiledb_ctx_alloc(hconfig, h);

            if (h[0] == (void*)0)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(h[0]);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        override protected bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_ctx_t*)handle;
            Methods.tiledb_ctx_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public ulong Get() { return (ulong)handle; }
        private protected void SetHandle(tiledb_ctx_t* h) { SetHandle((IntPtr)h); }
        private protected ContextHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(ContextHandle h) => h.handle;
        public static implicit operator tiledb_ctx_t*(ContextHandle h) => (tiledb_ctx_t*)h.handle;
        public static implicit operator ContextHandle(tiledb_ctx_t* value) => new ContextHandle((IntPtr)value);
    }//public unsafe partial class ContextHandle

    [Obsolete(Obsoletions.TileDBInterop2Message, DiagnosticId = Obsoletions.TileDBInterop2DiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public unsafe class FilterHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure

        public FilterHandle(ContextHandle hcontext, tiledb_filter_type_t filterType) : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_filter_t*[1];

            Methods.tiledb_filter_alloc(hcontext, filterType, h);

            if (h[0] == (void*)0)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(h[0]);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        override protected bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_filter_t*)handle;
            Methods.tiledb_filter_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public ulong Get() { return (ulong)handle; }
        private protected void SetHandle(tiledb_filter_t* h) { SetHandle((IntPtr)h); }
        private protected FilterHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(FilterHandle h) => h.handle;
        public static implicit operator tiledb_filter_t*(FilterHandle h) => (tiledb_filter_t*)h.handle;
        public static implicit operator FilterHandle(tiledb_filter_t* value) => new FilterHandle((IntPtr)value);
    }//public unsafe partial class FilterHandle


    [Obsolete(Obsoletions.TileDBInterop2Message, DiagnosticId = Obsoletions.TileDBInterop2DiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public unsafe class FilterListHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure
        public FilterListHandle(ContextHandle hcontext) : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_filter_list_t*[1];

            Methods.tiledb_filter_list_alloc(hcontext, h);

            if (h[0] == (void*)0)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(h[0]);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        override protected bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_filter_list_t*)handle;
            Methods.tiledb_filter_list_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public ulong Get() { return (ulong)handle; }
        private protected void SetHandle(tiledb_filter_list_t* h) { SetHandle((IntPtr)h); }
        private protected FilterListHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(FilterListHandle h) => h.handle;
        public static implicit operator tiledb_filter_list_t*(FilterListHandle h) => (tiledb_filter_list_t*)h.handle;
        public static implicit operator FilterListHandle(tiledb_filter_list_t* value) => new FilterListHandle((IntPtr)value);
    }//public unsafe partial class FilterListHandle

    [Obsolete(Obsoletions.TileDBInterop2Message, DiagnosticId = Obsoletions.TileDBInterop2DiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public unsafe partial class VFSHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure

        public VFSHandle(ContextHandle hcontext, ConfigHandle hconfig) : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_vfs_t*[1];

            int status = TileDB.Interop.Methods.tiledb_vfs_alloc(hcontext, hconfig, h);

            if (h[0] == (void*)0)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(h[0]);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        override protected bool ReleaseHandle()
        {
            // Free the native object
            tiledb_vfs_t* p = (tiledb_vfs_t*)handle;
            TileDB.Interop.Methods.tiledb_vfs_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public UInt64 get() { return (UInt64)this.handle; }
        private protected void SetHandle(tiledb_vfs_t* h) { SetHandle((IntPtr)h); }
        private protected VFSHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => this.handle == IntPtr.Zero;
        public static implicit operator IntPtr(VFSHandle h) => h.handle;
        public static implicit operator tiledb_vfs_t*(VFSHandle h) => (tiledb_vfs_t*)h.handle;
        public static implicit operator VFSHandle(tiledb_vfs_t* value) => new VFSHandle((IntPtr)value);
    }//public unsafe partial class VFSHandle


    [Obsolete(Obsoletions.TileDBInterop2Message, DiagnosticId = Obsoletions.TileDBInterop2DiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public unsafe class AttributeHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure
        public AttributeHandle(ContextHandle hcontext, string name, tiledb_datatype_t datatype) : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_attribute_t*[1];

            var ms_name = new MarshaledString(name);
            Methods.tiledb_attribute_alloc(hcontext, ms_name, datatype, h);

            if (h[0] == (void*)0)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(h[0]);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        override protected bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_attribute_t*)handle;
            Methods.tiledb_attribute_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public ulong Get() { return (ulong)handle; }
        private protected void SetHandle(tiledb_attribute_t* h) { SetHandle((IntPtr)h); }
        private protected AttributeHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(AttributeHandle h) => h.handle;
        public static implicit operator tiledb_attribute_t*(AttributeHandle h) => (tiledb_attribute_t*)h.handle;
        public static implicit operator AttributeHandle(tiledb_attribute_t* value) => new AttributeHandle((IntPtr)value);
    }//public unsafe partial class AttributeHandle

    [Obsolete(Obsoletions.TileDBInterop2Message, DiagnosticId = Obsoletions.TileDBInterop2DiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public unsafe class DimensionHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure

        public DimensionHandle(ContextHandle hcontext, string name, tiledb_datatype_t datatype, void* dimDomain, void* tileExtent) : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_dimension_t*[1];

            var ms_name = new MarshaledString(name);

            Methods.tiledb_dimension_alloc(hcontext, ms_name, datatype, dimDomain, tileExtent, h);

            if (h[0] == (void*)0)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(h[0]);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        override protected bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_dimension_t*)handle;
            Methods.tiledb_dimension_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public ulong Get() { return (ulong)handle; }
        private protected void SetHandle(tiledb_dimension_t* h) { SetHandle((IntPtr)h); }
        private protected DimensionHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(DimensionHandle h) => h.handle;
        public static implicit operator tiledb_dimension_t*(DimensionHandle h) => (tiledb_dimension_t*)h.handle;
        public static implicit operator DimensionHandle(tiledb_dimension_t* value) => new DimensionHandle((IntPtr)value);
    }//public unsafe partial class DimensionHandle


    [Obsolete(Obsoletions.TileDBInterop2Message, DiagnosticId = Obsoletions.TileDBInterop2DiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public unsafe class DomainHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure

        public DomainHandle(ContextHandle hcontext) : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_domain_t*[1];

            Methods.tiledb_domain_alloc(hcontext, h);

            if (h[0] == (void*)0)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(h[0]);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        override protected bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_domain_t*)handle;
            Methods.tiledb_domain_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public ulong Get() { return (ulong)handle; }
        private protected void SetHandle(tiledb_domain_t* h) { SetHandle((IntPtr)h); }
        private protected DomainHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(DomainHandle h) => h.handle;
        public static implicit operator tiledb_domain_t*(DomainHandle h) => (tiledb_domain_t*)h.handle;
        public static implicit operator DomainHandle(tiledb_domain_t* value) => new DomainHandle((IntPtr)value);
    }//public unsafe partial class DomainHandle


    [Obsolete(Obsoletions.TileDBInterop2Message, DiagnosticId = Obsoletions.TileDBInterop2DiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public unsafe class ArraySchemaHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure
        public ArraySchemaHandle(ContextHandle contextHandle, tiledb_array_type_t arrayType) : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_array_schema_t*[1];

            Methods.tiledb_array_schema_alloc(contextHandle, arrayType, h);

            if (h[0] == (void*)0)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(h[0]);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        override protected bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_array_schema_t*)handle;
            Methods.tiledb_array_schema_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public ulong Get() { return (ulong)handle; }
        private protected void SetHandle(tiledb_array_schema_t* h) { SetHandle((IntPtr)h); }
        private protected ArraySchemaHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(ArraySchemaHandle h) => h.handle;
        public static implicit operator tiledb_array_schema_t*(ArraySchemaHandle h) => (tiledb_array_schema_t*)h.handle;
        public static implicit operator ArraySchemaHandle(tiledb_array_schema_t* value) => new ArraySchemaHandle((IntPtr)value);
    }//public unsafe partial class ArraySchemaHandle

    [Obsolete(Obsoletions.TileDBInterop2Message, DiagnosticId = Obsoletions.TileDBInterop2DiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public unsafe class ArrayHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure
        public ArrayHandle(ContextHandle contextHandle, sbyte* uri) : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_array_t*[1];

            Methods.tiledb_array_alloc(contextHandle, uri, h);

            if (h[0] == (void*)0)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(h[0]);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        override protected bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_array_t*)handle;
            Methods.tiledb_array_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public ulong Get() { return (ulong)handle; }
        private protected void SetHandle(tiledb_array_t* h) { SetHandle((IntPtr)h); }
        private protected ArrayHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(ArrayHandle h) => h.handle;
        public static implicit operator tiledb_array_t*(ArrayHandle h) => (tiledb_array_t*)h.handle;
        public static implicit operator ArrayHandle(tiledb_array_t* value) => new ArrayHandle((IntPtr)value);
    }//public unsafe partial class ArrayHandle

    [Obsolete(Obsoletions.TileDBInterop2Message, DiagnosticId = Obsoletions.TileDBInterop2DiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public unsafe class ArraySchemaEvolutionHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure
        public ArraySchemaEvolutionHandle(ContextHandle contextHandle) : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_array_schema_evolution_t*[1];
            Methods.tiledb_array_schema_evolution_alloc(contextHandle, h);

            if (h[0] == (void*)0)
            {
                throw new Exception("Failed to allocate ArraySchemaEvolutionHandle!");
            }
            SetHandle(h[0]);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        protected override bool ReleaseHandle()
        {
            var p = (tiledb_array_schema_evolution_t*)handle;
            Methods.tiledb_array_schema_evolution_free(&p);
            SetHandle(IntPtr.Zero);
            return true;
        }

        // Conversions, getters, operators
        public ulong Get() => (ulong)handle;
        private protected void SetHandle(tiledb_array_schema_evolution_t* h) => SetHandle((IntPtr)h);
        private protected ArraySchemaEvolutionHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(ArraySchemaEvolutionHandle h) => h.handle;
        public static implicit operator tiledb_array_schema_evolution_t*(ArraySchemaEvolutionHandle h) =>
            (tiledb_array_schema_evolution_t*)h.handle;
        public static implicit operator ArraySchemaEvolutionHandle(tiledb_array_schema_evolution_t* value) =>
            new ArraySchemaEvolutionHandle((IntPtr)value);
    }//public unsafe class ArraySchemaEvolutionHandle

    [Obsolete(Obsoletions.TileDBInterop2Message, DiagnosticId = Obsoletions.TileDBInterop2DiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public unsafe class QueryHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure
        public QueryHandle(ContextHandle contextHandle, ArrayHandle arrayHandle, tiledb_query_type_t queryType) : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_query_t*[1];

            Methods.tiledb_query_alloc(contextHandle, arrayHandle, queryType, h);

            if (h[0] == (void*)0)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(h[0]);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        override protected bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_query_t*)handle;
            Methods.tiledb_query_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public ulong Get() { return (ulong)handle; }
        private protected void SetHandle(tiledb_query_t* h) { SetHandle((IntPtr)h); }
        private protected QueryHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(QueryHandle h) => h.handle;
        public static implicit operator tiledb_query_t*(QueryHandle h) => (tiledb_query_t*)h.handle;
        public static implicit operator QueryHandle(tiledb_query_t* value) => new QueryHandle((IntPtr)value);
    }//public unsafe partial class QueryHandle


    [Obsolete(Obsoletions.TileDBInterop2Message, DiagnosticId = Obsoletions.TileDBInterop2DiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public unsafe class QueryConditionHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure
        public QueryConditionHandle(ContextHandle contextHandle) : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_query_condition_t*[1];

            Methods.tiledb_query_condition_alloc(contextHandle, h);

            if (h[0] == (void*)0)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(h[0]);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        override protected bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_query_condition_t*)handle;
            Methods.tiledb_query_condition_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public ulong Get() { return (ulong)handle; }
        private protected void SetHandle(tiledb_query_condition_t* h) { SetHandle((IntPtr)h); }
        private protected QueryConditionHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(QueryConditionHandle h) => h.handle;
        public static implicit operator tiledb_query_condition_t*(QueryConditionHandle h) => (tiledb_query_condition_t*)h.handle;
        public static implicit operator QueryConditionHandle(tiledb_query_condition_t* value) => new QueryConditionHandle((IntPtr)value);
    }//public unsafe partial class QueryConditionHandle

    [Obsolete(Obsoletions.TileDBInterop2Message, DiagnosticId = Obsoletions.TileDBInterop2DiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public unsafe class GroupHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure
        public GroupHandle(ContextHandle contextHandle, sbyte* uri) : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_group_t*[1];

            Methods.tiledb_group_alloc(contextHandle, uri, h);

            if (h[0] == (void*)0)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(h[0]);
        }

        // Deallocator: call native free with CER guarantees from SafeHandle
        override protected bool ReleaseHandle()
        {
            // Free the native object
            var p = (tiledb_group_t*)handle;
            Methods.tiledb_group_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public ulong Get() { return (ulong)handle; }
        private protected void SetHandle(tiledb_group_t* h) { SetHandle((IntPtr)h); }
        private protected GroupHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => handle == IntPtr.Zero;
        public static implicit operator IntPtr(GroupHandle h) => h.handle;
        public static implicit operator tiledb_group_t*(GroupHandle h) => (tiledb_group_t*)h.handle;
        public static implicit operator GroupHandle(tiledb_group_t* value) => new GroupHandle((IntPtr)value);
    }//public unsafe partial class GroupHandle

}//namespace
using System.Runtime.InteropServices;
using System;
namespace TileDB.Interop
{

    public unsafe partial class ConfigHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure
 
        public ConfigHandle() : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_config_t*[1];
            var e = stackalloc tiledb_error_t*[1];
            int status = TileDB.Interop.Methods.tiledb_config_alloc(h, e);
            
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
            tiledb_config_t* p = (tiledb_config_t*)handle;
            TileDB.Interop.Methods.tiledb_config_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public UInt64 get() { return (UInt64)this.handle; }
        private protected void SetHandle(tiledb_config_t* h) { SetHandle((IntPtr)h); }
        private protected ConfigHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => this.handle == IntPtr.Zero;
        public static implicit operator IntPtr(ConfigHandle h) => h.handle;
        public static implicit operator tiledb_config_t*(ConfigHandle h) => (tiledb_config_t*)h.handle;
        public static implicit operator ConfigHandle(tiledb_config_t* value) => new ConfigHandle((IntPtr)value);
    }//public unsafe partial class ConfigHandle


    public unsafe partial class ConfigIteratorHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure
 
        public ConfigIteratorHandle(ConfigHandle hconfig, string prefix) : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_config_iter_t*[1];
            var e = stackalloc tiledb_error_t*[1];
            TileDB.Interop.MarshaledString ms_prefix = new Interop.MarshaledString(prefix);
            int status = TileDB.Interop.Methods.tiledb_config_iter_alloc(hconfig, ms_prefix, h, e);
            
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
            tiledb_config_iter_t* p = (tiledb_config_iter_t*)handle;
            TileDB.Interop.Methods.tiledb_config_iter_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public UInt64 get() { return (UInt64)this.handle; }
        private protected void SetHandle(tiledb_config_iter_t* h) { SetHandle((IntPtr)h); }
        private protected ConfigIteratorHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => this.handle == IntPtr.Zero;
        public static implicit operator IntPtr(ConfigIteratorHandle h) => h.handle;
        public static implicit operator tiledb_config_iter_t*(ConfigIteratorHandle h) => (tiledb_config_iter_t*)h.handle;
        public static implicit operator ConfigIteratorHandle(tiledb_config_iter_t* value) => new ConfigIteratorHandle((IntPtr)value);
    }//public unsafe partial class ConfigHandle


    public unsafe partial class ContextHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure
        public ContextHandle() : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_ctx_t*[1];
            ConfigHandle hconfig = new ConfigHandle();
            int status = TileDB.Interop.Methods.tiledb_ctx_alloc(hconfig, h); 

            if (h[0] == (void*)0)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(h[0]);
        }

        public ContextHandle(ConfigHandle hconfig) : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_ctx_t*[1];
            int status = TileDB.Interop.Methods.tiledb_ctx_alloc(hconfig, h);

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
            tiledb_ctx_t* p = (tiledb_ctx_t*)handle;
            TileDB.Interop.Methods.tiledb_ctx_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public UInt64 get() { return (UInt64)this.handle; }
        private protected void SetHandle(tiledb_ctx_t* h) { SetHandle((IntPtr)h); }
        private protected ContextHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => this.handle == IntPtr.Zero;
        public static implicit operator IntPtr(ContextHandle h) => h.handle;
        public static implicit operator tiledb_ctx_t*(ContextHandle h) => (tiledb_ctx_t*)h.handle;
        public static implicit operator ContextHandle(tiledb_ctx_t* value) => new ContextHandle((IntPtr)value);
    }//public unsafe partial class ContextHandle


    public unsafe partial class FilterHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure
        public FilterHandle() : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_filter_t*[1];
            ContextHandle hcontext = new ContextHandle();
            int status = TileDB.Interop.Methods.tiledb_filter_alloc(hcontext, TileDB.Interop.tiledb_filter_type_t.TILEDB_FILTER_NONE, h);

            if (h[0] == (void*)0)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(h[0]);
        }

        public FilterHandle(tiledb_filter_type_t filter_type) : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_filter_t*[1];
            ContextHandle hcontext = new ContextHandle();
            int status = TileDB.Interop.Methods.tiledb_filter_alloc(hcontext, filter_type, h);

            if (h[0] == (void*)0)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(h[0]);
        }

        public FilterHandle(ContextHandle hcontext, tiledb_filter_type_t filter_type) : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_filter_t*[1];
 
            int status = TileDB.Interop.Methods.tiledb_filter_alloc(hcontext, filter_type, h);

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
            tiledb_filter_t* p = (tiledb_filter_t*)handle;
            TileDB.Interop.Methods.tiledb_filter_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public UInt64 get() { return (UInt64)this.handle; }
        private protected void SetHandle(tiledb_filter_t* h) { SetHandle((IntPtr)h); }
        private protected FilterHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => this.handle == IntPtr.Zero;
        public static implicit operator IntPtr(FilterHandle h) => h.handle;
        public static implicit operator tiledb_filter_t*(FilterHandle h) => (tiledb_filter_t*)h.handle;
        public static implicit operator FilterHandle(tiledb_filter_t* value) => new FilterHandle((IntPtr)value);
    }//public unsafe partial class FilterHandle


    public unsafe partial class FilterListHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure
        public FilterListHandle() : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_filter_list_t*[1];
            ContextHandle hcontext = new ContextHandle();
            int status = TileDB.Interop.Methods.tiledb_filter_list_alloc(hcontext, h);

            if (h[0] == (void*)0)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(h[0]);
        }

        public FilterListHandle(ContextHandle hcontext) : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_filter_list_t*[1];

            int status = TileDB.Interop.Methods.tiledb_filter_list_alloc(hcontext, h);

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
            tiledb_filter_list_t* p = (tiledb_filter_list_t*)handle;
            TileDB.Interop.Methods.tiledb_filter_list_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public UInt64 get() { return (UInt64)this.handle; }
        private protected void SetHandle(tiledb_filter_list_t* h) { SetHandle((IntPtr)h); }
        private protected FilterListHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => this.handle == IntPtr.Zero;
        public static implicit operator IntPtr(FilterListHandle h) => h.handle;
        public static implicit operator tiledb_filter_list_t*(FilterListHandle h) => (tiledb_filter_list_t*)h.handle;
        public static implicit operator FilterListHandle(tiledb_filter_list_t* value) => new FilterListHandle((IntPtr)value);
    }//public unsafe partial class FilterListHandle

 
    public unsafe partial class AttributeHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure
        public AttributeHandle() : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_attribute_t*[1];
            ContextHandle hcontext = new ContextHandle();
            string name = "unknown";
            tiledb_datatype_t datatype = tiledb_datatype_t.TILEDB_ANY;
            MarshaledString ms_name = new MarshaledString(name);
            int status = TileDB.Interop.Methods.tiledb_attribute_alloc(hcontext, ms_name, datatype, h);

            if (h[0] == (void*)0)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(h[0]);
        }

        public AttributeHandle(ContextHandle hcontext, string name, tiledb_datatype_t datatype) : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_attribute_t*[1];

            MarshaledString ms_name = new MarshaledString(name);
            int status = TileDB.Interop.Methods.tiledb_attribute_alloc(hcontext, ms_name, datatype, h);

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
            tiledb_attribute_t* p = (tiledb_attribute_t*)handle;
            TileDB.Interop.Methods.tiledb_attribute_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public UInt64 get() { return (UInt64)this.handle; }
        private protected void SetHandle(tiledb_attribute_t* h) { SetHandle((IntPtr)h); }
        private protected AttributeHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => this.handle == IntPtr.Zero;
        public static implicit operator IntPtr(AttributeHandle h) => h.handle;
        public static implicit operator tiledb_attribute_t*(AttributeHandle h) => (tiledb_attribute_t*)h.handle;
        public static implicit operator AttributeHandle(tiledb_attribute_t* value) => new AttributeHandle((IntPtr)value);
    }//public unsafe partial class AttributeHandle

    public unsafe partial class DimensionHandle: SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure

        public DimensionHandle(ContextHandle hcontext, string name, tiledb_datatype_t datatype, void* dim_domain, void* tile_extent) : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_dimension_t*[1];

            MarshaledString ms_name = new MarshaledString(name);

            int status = TileDB.Interop.Methods.tiledb_dimension_alloc(hcontext, ms_name, datatype, dim_domain,tile_extent, h);

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
            tiledb_dimension_t* p = (tiledb_dimension_t*)handle;
            TileDB.Interop.Methods.tiledb_dimension_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public UInt64 get() { return (UInt64)this.handle; }
        private protected void SetHandle(tiledb_dimension_t* h) { SetHandle((IntPtr)h); }
        private protected DimensionHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => this.handle == IntPtr.Zero;
        public static implicit operator IntPtr(DimensionHandle h) => h.handle;
        public static implicit operator tiledb_dimension_t*(DimensionHandle h) => (tiledb_dimension_t*)h.handle;
        public static implicit operator DimensionHandle(tiledb_dimension_t* value) => new DimensionHandle((IntPtr)value);
    }//public unsafe partial class DimensionHandle


    public unsafe partial class DomainHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure
        public DomainHandle() : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_domain_t*[1];
            ContextHandle hcontext = new ContextHandle();
            int status = TileDB.Interop.Methods.tiledb_domain_alloc(hcontext, h);

            if (h[0] == (void*)0)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(h[0]);
        }

        public DomainHandle(ContextHandle hcontext) : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_domain_t*[1];

            int status = TileDB.Interop.Methods.tiledb_domain_alloc(hcontext, h);

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
            tiledb_domain_t* p = (tiledb_domain_t*)handle;
            TileDB.Interop.Methods.tiledb_domain_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public UInt64 get() { return (UInt64)this.handle; }
        private protected void SetHandle(tiledb_domain_t* h) { SetHandle((IntPtr)h); }
        private protected DomainHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => this.handle == IntPtr.Zero;
        public static implicit operator IntPtr(DomainHandle h) => h.handle;
        public static implicit operator tiledb_domain_t*(DomainHandle h) => (tiledb_domain_t*)h.handle;
        public static implicit operator DomainHandle(tiledb_domain_t* value) => new DomainHandle((IntPtr)value);
    }//public unsafe partial class DomainHandle


    public unsafe partial class ArraySchemaHandle : SafeHandle
    {
        // Constructor for a Handle
        //   - calls native allocator
        //   - exception on failure
        public ArraySchemaHandle() : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_array_schema_t*[1];
            ContextHandle hcontext = new ContextHandle();
            int status = TileDB.Interop.Methods.tiledb_array_schema_alloc(hcontext,tiledb_array_type_t.TILEDB_DENSE, h);

            if (h[0] == (void*)0)
            {
                throw new Exception("Failed to allocate!");
            }
            SetHandle(h[0]);
        }

        public ArraySchemaHandle(ContextHandle hcontext, tiledb_array_type_t arraytype) : base(IntPtr.Zero, ownsHandle: true)
        {
            var h = stackalloc tiledb_array_schema_t*[1];

            int status = TileDB.Interop.Methods.tiledb_array_schema_alloc(hcontext, arraytype, h);

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
            tiledb_array_schema_t* p = (tiledb_array_schema_t*)handle;
            TileDB.Interop.Methods.tiledb_array_schema_free(&p);
            // Invalidate the contained pointer
            SetHandle(IntPtr.Zero);

            return true;
        }

        // Conversions, getters, operators
        public UInt64 get() { return (UInt64)this.handle; }
        private protected void SetHandle(tiledb_array_schema_t* h) { SetHandle((IntPtr)h); }
        private protected ArraySchemaHandle(IntPtr value) : base(value, ownsHandle: false) { }
        public override bool IsInvalid => this.handle == IntPtr.Zero;
        public static implicit operator IntPtr(ArraySchemaHandle h) => h.handle;
        public static implicit operator tiledb_array_schema_t*(ArraySchemaHandle h) => (tiledb_array_schema_t*)h.handle;
        public static implicit operator ArraySchemaHandle(tiledb_array_schema_t* value) => new ArraySchemaHandle((IntPtr)value);
    }//public unsafe partial class ArraySchemaHandle



}//namespace
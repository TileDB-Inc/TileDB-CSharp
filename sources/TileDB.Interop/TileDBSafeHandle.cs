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
        public static implicit operator FilterHandle(tiledb_ctx_t* value) => new FilterHandle((IntPtr)value);
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
        public static implicit operator FilterListHandle(tiledb_ctx_t* value) => new FilterListHandle((IntPtr)value);
    }//public unsafe partial class FilterListHandle


}//namespace
using System;
using System.Collections.Generic;
using TileDB.Interop;

namespace TileDB.CSharp
{
    public sealed unsafe class ArraySchema : IDisposable
    {
        private readonly ArraySchemaHandle _handle;
        private readonly Context _ctx;
        private bool _disposed;

        public ArraySchema(Context ctx, ArrayType arrayType) 
        {
            _ctx = ctx;
            var tiledb_array_type = (tiledb_array_type_t)arrayType;
            _handle = new ArraySchemaHandle(_ctx.Handle, tiledb_array_type);
        }

        private ArraySchema(Context ctx, string uri)
        {
            _ctx = ctx;
            tiledb_array_schema_t* array_schema_p;
            var ms_uri = new MarshaledString(uri);
            _ctx.handle_error(Methods.tiledb_array_schema_load(_ctx.Handle, ms_uri, &array_schema_p));
            _handle = array_schema_p;

        }

        
        internal ArraySchema(Context ctx, ArraySchemaHandle handle) 
        {
            _ctx = ctx;
            _handle = handle;
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

        internal ArraySchemaHandle Handle => _handle;


        #region capi functions

        /// <summary>
        /// Add an attribute.
        /// </summary>
        /// <param name="attr"></param>
        public void add_attribute(Attribute attr)
        {
            _ctx.handle_error(Methods.tiledb_array_schema_add_attribute(_ctx.Handle, _handle, attr.Handle));
        }

        public void add_attributes(params Attribute[] attrs)
        {
            foreach (var t in attrs)
            {
                _ctx.handle_error(Methods.tiledb_array_schema_add_attribute(_ctx.Handle, _handle, t.Handle));
            }
        }

        /// <summary>
        /// Set if duplicate is allowed or not
        /// </summary>
        /// <param name="allowsDups"></param>
        public void set_allows_dups(bool allowsDups)
        {

            var int_allow_dups = allowsDups ? 1 : 0;
            _ctx.handle_error(Methods.tiledb_array_schema_set_allows_dups(_ctx.Handle, _handle, int_allow_dups));
        }

        /// <summary>
        /// Get if duplicate is allowed or not.
        /// </summary>
        /// <returns></returns>
        public bool allows_dups()
        {
            var allowDups = 0;
            _ctx.handle_error(Methods.tiledb_array_schema_get_allows_dups(_ctx.Handle, _handle, &allowDups));
            return allowDups > 0;
        }

        /// <summary>
        /// Set domain.
        /// </summary>
        /// <param name="domain"></param>
        public void set_domain(Domain domain)
        {

            _ctx.handle_error(Methods.tiledb_array_schema_set_domain(_ctx.Handle, _handle, domain.Handle));
        }

        /// <summary>
        /// Set capacity.
        /// </summary>
        /// <param name="capacity"></param>
        public void set_capacity(ulong capacity)
        {
            _ctx.handle_error(Methods.tiledb_array_schema_set_capacity(_ctx.Handle, _handle, capacity));
        }

        /// <summary>
        /// Set cell order.
        /// </summary>
        /// <param name="layoutType"></param>
        public void set_cell_order(LayoutType layoutType)
        {
            var tiledb_layout = (tiledb_layout_t)layoutType;
            _ctx.handle_error(Methods.tiledb_array_schema_set_cell_order(_ctx.Handle, _handle, tiledb_layout));
        }

        /// <summary>
        /// Set tile order.
        /// </summary>
        /// <param name="layoutType"></param>
        public void set_tile_order(LayoutType layoutType)
        {
            var tiledb_layout = (tiledb_layout_t)layoutType;
            _ctx.handle_error(Methods.tiledb_array_schema_set_tile_order(_ctx.Handle, _handle, tiledb_layout));
        }

        /// <summary>
        /// Set coordinates filter list.
        /// </summary>
        /// <param name="filterList"></param>
        public void set_coords_filter_list(FilterList filterList)
        {
            _ctx.handle_error(Methods.tiledb_array_schema_set_coords_filter_list(_ctx.Handle, _handle, filterList.Handle));

        }

        /// <summary>
        /// Set offsets filter list.
        /// </summary>
        /// <param name="filterList"></param>
        public void set_offsets_filter_list(FilterList filterList)
        {
            _ctx.handle_error(Methods.tiledb_array_schema_set_offsets_filter_list(_ctx.Handle, _handle, filterList.Handle));

        }

        /// <summary>
        /// Check if it is valid or not.
        /// </summary>
        /// <returns></returns>
        public void Check()
        {
            _ctx.handle_error(Methods.tiledb_array_schema_check(_ctx.Handle, _handle));

        }

 

        /// <summary>
        /// Get ArrayType.
        /// </summary>
        /// <returns></returns>
        public ArrayType array_type()
        {
            tiledb_array_type_t tiledb_array_type;
            _ctx.handle_error(Methods.tiledb_array_schema_get_array_type(_ctx.Handle, _handle, &tiledb_array_type));

            return (ArrayType)tiledb_array_type;
        }

        /// <summary>
        /// Get capacity.
        /// </summary>
        /// <returns></returns>
        public ulong Capacity()
        {
            ulong capacity;
            _ctx.handle_error(Methods.tiledb_array_schema_get_capacity(_ctx.Handle, _handle, &capacity));
            
            return capacity;
        }

        /// <summary>
        /// Get cell order.
        /// </summary>
        /// <returns></returns>
        public LayoutType cell_order()
        {
            tiledb_layout_t  tiledb_layout;
            _ctx.handle_error(Methods.tiledb_array_schema_get_cell_order(_ctx.Handle, _handle, &tiledb_layout));
            return (LayoutType)tiledb_layout;
        }

        /// <summary>
        /// Get coordinates filter list.
        /// </summary>
        /// <returns></returns>
        public FilterList coords_filter_list()
        {

            tiledb_filter_list_t* filter_list_p;
            _ctx.handle_error(Methods.tiledb_array_schema_get_coords_filter_list(_ctx.Handle, _handle, &filter_list_p));
            return new FilterList(_ctx, filter_list_p);
        }

        /// <summary>
        /// Get offsets filter list.
        /// </summary>
        /// <returns></returns>
        public FilterList offsets_filter_list()
        {
            tiledb_filter_list_t* filter_list_p;
            _ctx.handle_error(Methods.tiledb_array_schema_get_offsets_filter_list(_ctx.Handle, _handle, &filter_list_p));
            return new FilterList(_ctx, filter_list_p);
        }

        /// <summary>
        /// Get domain.
        /// </summary>
        /// <returns></returns>
        public Domain Domain()
        {
            tiledb_domain_t* domain_p = null;  
            _ctx.handle_error(Methods.tiledb_array_schema_get_domain(_ctx.Handle, _handle, &domain_p));

            return new Domain(_ctx,domain_p);
        }

        /// <summary>
        /// Get tile order.
        /// </summary>
        /// <returns></returns>
        public LayoutType tile_order()
        {
            tiledb_layout_t tiledb_layout;
            _ctx.handle_error(Methods.tiledb_array_schema_get_tile_order(_ctx.Handle, _handle, &tiledb_layout));
            return (LayoutType)tiledb_layout;
        }

        /// <summary>
        /// Get number of attributes.
        /// </summary>
        /// <returns></returns>
        public uint attribute_num()
        {
            uint num = 0;
            _ctx.handle_error(Methods.tiledb_array_schema_get_attribute_num(_ctx.Handle, _handle, &num));
            return num;
        }

 

        /// <summary>
        /// Get attribute from index.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Attribute Attribute(uint i)
        {
            tiledb_attribute_t* attribute_p;
            _ctx.handle_error(Methods.tiledb_array_schema_get_attribute_from_index(_ctx.Handle, _handle, i, &attribute_p));

            return new Attribute(_ctx, attribute_p);
        }

        /// <summary>
        /// Get attribute from name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Attribute Attribute(string name)
        {
            tiledb_attribute_t* attribute_p;
            var ms_name = new MarshaledString(name);
            _ctx.handle_error(Methods.tiledb_array_schema_get_attribute_from_name(_ctx.Handle, _handle, ms_name, &attribute_p));

            return new Attribute(_ctx, attribute_p);
        }

        /// <summary>
        /// Test if an attribute with name exists or not.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool has_attribute(string name)
        {
            var has_attr = 0;
            var ms_name = new MarshaledString(name);

            _ctx.handle_error(Methods.tiledb_array_schema_has_attribute(_ctx.Handle, _handle, ms_name, &has_attr));
            return has_attr>0;
        }



        /// <summary>
        /// Load an array schema from uri.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="uri"></param>
        /// <returns></returns>

        public static ArraySchema Load(Context ctx, string uri)
        {
            return new ArraySchema(ctx, uri);
        }


        #endregion capi functions

        /// <summary>
        /// Get attributes dictionary.
        /// </summary>
        /// <returns></returns>
        public SortedDictionary<string, Attribute> Attributes()
        {
            var ret = new SortedDictionary<string, Attribute>();
            var attribute_num = this.attribute_num();
            for (uint i = 0; i < attribute_num; ++i)
            {
                var attr = Attribute(i);
                ret.Add(attr.Name(),attr);
            }
            return ret;
        }

        /// <summary>
        /// Get dimension dictionary.
        /// </summary>
        /// <returns></returns>
        public SortedDictionary<string, Dimension> Dimensions()
        {
            var ret = new SortedDictionary<string, Dimension>();
            var domain = this.Domain();
            var ndim = domain.NDim();
            for (uint i = 0; i < ndim; ++i)
            {
                var dim = domain.Dimension(i);
                ret.Add(dim.Name(), dim);
            }
            return ret;
        }
 

    

    }//class

}//namespace
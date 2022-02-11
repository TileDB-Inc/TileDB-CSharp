using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileDB.Interop;

namespace TileDB
{
    public unsafe class ArraySchema : IDisposable
    {
        private TileDB.Interop.ArraySchemaHandle handle_;
        private Context ctx_;
        private bool disposed_ = false;

        public ArraySchema(Context ctx, ArrayType array_type) 
        {
            ctx_ = ctx;
            TileDB.Interop.tiledb_array_type_t tiledb_array_type = (TileDB.Interop.tiledb_array_type_t)array_type;
            handle_ = new TileDB.Interop.ArraySchemaHandle(ctx_.Handle, tiledb_array_type);
        }

        public ArraySchema(Context ctx, string uri)
        {
            ctx_ = ctx;
            TileDB.Interop.tiledb_array_schema_t* array_schema_p;
            var ms_uri = new TileDB.Interop.MarshaledString(uri);
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_array_schema_load(ctx_.Handle, ms_uri, &array_schema_p));
            handle_ = array_schema_p;

        }

        
        internal ArraySchema(Context ctx, TileDB.Interop.ArraySchemaHandle handle) 
        {
            ctx_ = ctx;
            handle_ = handle;
        }

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {

            if (!disposed_)
            {
                if (disposing && (!handle_.IsInvalid))
                {
                    handle_.Dispose();
                }

                disposed_ = true;
            }

        }


        #region capi functions

        /// <summary>
        /// Add an attribute.
        /// </summary>
        /// <param name="attr"></param>
        public void add_attribute(Attribute attr)
        {
            ctx_.handle_error(Methods.tiledb_array_schema_add_attribute(ctx_.Handle, handle_, attr.Handle));
        }

        public void add_attributes(params Attribute[] attrs)
        {
            for (int idx = 0; idx < attrs.Length; idx++)
            {
                ctx_.handle_error(Methods.tiledb_array_schema_add_attribute(ctx_.Handle, handle_, attrs[idx].Handle));
            }
        }

        /// <summary>
        /// Set if duplicate is allowed or not
        /// </summary>
        /// <param name="allows_dups"></param>
        public void set_allows_dups(bool allows_dups)
        {

            var int_allow_dups = allows_dups ? 1 : 0;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_array_schema_set_allows_dups(ctx_.Handle, handle_, int_allow_dups));
        }

        /// <summary>
        /// Get if duplicate is allowed or not.
        /// </summary>
        /// <returns></returns>
        public bool allows_dups()
        {
            var int_allows_dups = 0;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_array_schema_get_allows_dups(ctx_.Handle, handle_, &int_allows_dups));
            return int_allows_dups > 0;
        }

        /// <summary>
        /// Set domain.
        /// <param name="domain"></param>
        public void set_domain(Domain domain)
        {

            ctx_.handle_error(TileDB.Interop.Methods.tiledb_array_schema_set_domain(ctx_.Handle, handle_, domain.Handle));
            return;
        }

        /// <summary>
        /// Set capacity.
        /// </summary>
        /// <param name="capacity"></param>
        public void set_capacity(ulong capacity)
        {
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_array_schema_set_capacity(ctx_.Handle, handle_, capacity));
            return;
        }

        /// <summary>
        /// Set cell order.
        /// </summary>
        /// <param name="layouttype"></param>
        public void set_cell_order(LayoutType layouttype)
        {
            var tiledb_layout = (TileDB.Interop.tiledb_layout_t)layouttype;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_array_schema_set_cell_order(ctx_.Handle, handle_, tiledb_layout));
            return;
        }

        /// <summary>
        /// Set tile order.
        /// </summary>
        /// <param name="layouttype"></param>
        public void set_tile_order(LayoutType layouttype)
        {
            var tiledb_layout = (TileDB.Interop.tiledb_layout_t)layouttype;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_array_schema_set_tile_order(ctx_.Handle, handle_, tiledb_layout));
            return;
        }

        /// <summary>
        /// Set coordinates filter list.
        /// </summary>
        /// <param name="filter_list"></param>
        public void set_coords_filter_list(FilterList filter_list)
        {
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_array_schema_set_coords_filter_list(ctx_.Handle, handle_, filter_list.Handle));

        }

        /// <summary>
        /// Set offsets filter list.
        /// <param name="filter_list"></param>
        public void set_offsets_filter_list(FilterList filter_list)
        {
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_array_schema_set_offsets_filter_list(ctx_.Handle, handle_, filter_list.Handle));

        }

        /// <summary>
        /// Check if it is valid or not.
        /// </summary>
        /// <returns></returns>
        public void check()
        {
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_array_schema_check(ctx_.Handle, handle_));

        }

 

        /// <summary>
        /// Get ArrayType.
        /// </summary>
        /// <returns></returns>
        public ArrayType array_type()
        {
            TileDB.Interop.tiledb_array_type_t tiledb_array_type;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_array_schema_get_array_type(ctx_.Handle, handle_, &tiledb_array_type));

            return (ArrayType)tiledb_array_type;
        }

        /// <summary>
        /// Get capacity.
        /// <returns></returns>
        public ulong capacity()
        {
            ulong capacity;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_array_schema_get_capacity(ctx_.Handle, handle_, &capacity));
            
            return capacity;
        }

        /// <summary>
        /// Get cell order.
        /// </summary>
        /// <returns></returns>
        public LayoutType cell_order()
        {
            TileDB.Interop.tiledb_layout_t  tiledb_layout;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_array_schema_get_cell_order(ctx_.Handle, handle_, &tiledb_layout));
            return (LayoutType)tiledb_layout;
        }

        /// <summary>
        /// Get coordinates filter list.
        /// </summary>
        /// <returns></returns>
        public FilterList coords_filter_list()
        {

            TileDB.Interop.tiledb_filter_list_t* filter_list_p;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_array_schema_get_coords_filter_list(ctx_.Handle, handle_, &filter_list_p));
            return new FilterList(ctx_, filter_list_p);
        }

        /// <summary>
        /// Get offsets filter list.
        /// </summary>
        /// <returns></returns>
        public FilterList offsets_filter_list()
        {
            TileDB.Interop.tiledb_filter_list_t* filter_list_p;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_array_schema_get_offsets_filter_list(ctx_.Handle, handle_, &filter_list_p));
            return new FilterList(ctx_, filter_list_p);
        }

        /// <summary>
        /// Get domain.
        /// </summary>
        /// <returns></returns>
        public Domain domain()
        {
            TileDB.Interop.tiledb_domain_t* domain_p = null;  
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_array_schema_get_domain(ctx_.Handle, handle_, &domain_p));

            return new Domain(ctx_,domain_p);
        }

        /// <summary>
        /// Get tile order.
        /// </summary>
        /// <returns></returns>
        public LayoutType tile_order()
        {
            TileDB.Interop.tiledb_layout_t tiledb_layout;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_array_schema_get_tile_order(ctx_.Handle, handle_, &tiledb_layout));
            return (LayoutType)tiledb_layout;
        }

        /// <summary>
        /// Get number of attributes.
        /// </summary>
        /// <returns></returns>
        public uint attribute_num()
        {
            uint num = 0;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_array_schema_get_attribute_num(ctx_.Handle, handle_, &num));
            return num;
        }

 

        /// <summary>
        /// Get attribute from index.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Attribute attribute(uint i)
        {
            TileDB.Interop.tiledb_attribute_t* attribute_p;
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_array_schema_get_attribute_from_index(ctx_.Handle, handle_, i, &attribute_p));

            return new Attribute(ctx_, attribute_p);
        }

        /// <summary>
        /// Get attribute from name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Attribute attribute(string name)
        {
            TileDB.Interop.tiledb_attribute_t* attribute_p;
            var ms_name = new TileDB.Interop.MarshaledString(name);
            ctx_.handle_error(TileDB.Interop.Methods.tiledb_array_schema_get_attribute_from_name(ctx_.Handle, handle_, ms_name, &attribute_p));

            return new Attribute(ctx_, attribute_p);
        }

        /// <summary>
        /// Test if an attribute with name exists or not.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool has_attribute(string name)
        {
            var has_attr = 0;
            var ms_name = new TileDB.Interop.MarshaledString(name);

            ctx_.handle_error(TileDB.Interop.Methods.tiledb_array_schema_has_attribute(ctx_.Handle, handle_, ms_name, &has_attr));
            return has_attr>0;
        }



        /// <summary>
        /// Load an array schema from uri.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="uri"></param>
        /// <returns></returns>

        public static ArraySchema load(Context ctx, string uri)
        {
            return new ArraySchema(ctx, uri);
        }


        #endregion capi functions

        /// <summary>
        /// Get attributes dictionary.
        /// </summary>
        /// <returns></returns>
        public SortedDictionary<string, Attribute> attributes()
        {
            var ret = new SortedDictionary<string, Attribute>();
            var attribute_num = this.attribute_num();
            for (uint i = 0; i < attribute_num; ++i)
            {
                var attr = attribute(i);
                ret.Add(attr.name(),attr);
            }
            return ret;
        }

        /// <summary>
        /// Get dimension dictionary.
        /// </summary>
        /// <returns></returns>
        public SortedDictionary<string, Dimension> dimensions()
        {
            var ret = new SortedDictionary<string, Dimension>();
            var domain = this.domain();
            var ndim = domain.ndim();
            for (uint i = 0; i < ndim; ++i)
            {
                var dim = domain.dimension(i);
                ret.Add(dim.name(), dim);
            }
            return ret;
        }
 

    

    }//class

}//namespace